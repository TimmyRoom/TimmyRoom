# VR 음악센터 기술 명세서

---

VR 음악센터는 다음의 5개 씬으로 구성된다.

최초 프로그램 시작 시 Login Scene에서 시작되며,

User Interaction을 통해 화살표를 따라 각 씬으로 이동 가능하다.

아래는 각 씬에서 정의된 기능들의 목록이다.

1. [Global Function](https://www.notion.so/VR-fd5c155dad6447468978faffcdb4ca4c)
2. [Login Scene](https://www.notion.so/VR-fd5c155dad6447468978faffcdb4ca4c)
3. [Lobby Scene](https://www.notion.so/VR-fd5c155dad6447468978faffcdb4ca4c)
4. [Nanta Scene](https://www.notion.so/VR-fd5c155dad6447468978faffcdb4ca4c)
5. [Dance Scene](https://www.notion.so/VR-fd5c155dad6447468978faffcdb4ca4c)
6. [Gallery Scene](https://www.notion.so/VR-fd5c155dad6447468978faffcdb4ca4c)

# Global Function

---

여러 씬에서 중복적으로 사용되는 기능들이다.

## AbstractSceneManager

---

Abstract class
각 씬의 매니저 클래스가 포함할 멤버와 메서드를 담는 추상 클래스.
using 추가 소요를 줄이고, SceneMover 싱글톤 사용 유도한다.

- void Init()
    - 초기화시 필요한 로직들을 담는 함수

- public void MoveScene(string sceneName)
    - 특정 씬으로 이동하는 로직을 담은 함수

- public void MoveScene(int sceneIndex)
    - 특정 씬으로 이동하는 로직을 담은 함수

- public abstract void SetScenario(int scenarioIndex)
    - 현재 시나리오를 scenario 번호에 따라 설정하고 시나리오에 맞는 오브젝트 및 데이터, UI를 생성하거나 삭제한다.
    - scenarioIndex : 변경할 시나리오의 Index.

## IPlayScene

---

interface

SceneRecorder 싱글톤을 사용해서 플레이를 기록하는 함수.

- public void Record()
    - SceneRecorder 싱글톤을 사용해서 플레이를 기록하는 함수.
    - 난타와 춤 씬에서만 사용 가능하기에 NantaSceneManager와 DanceSceneManager에서만 사용.
    - 어떤 화면을 바라보게 캡처할것인지에 대한 로직을 이 함수 내에 구현한다.
        - 예시.
        ```csharp
        public void Record()
        {
        	bool result;
        	if(바깥에서 찍어야 하는 상황)
        	{
        		result = SceneRecorder.instance.CaptureSelf();
        	}
        	else
        	{
        		result = SceneRecorder.instance.Capture();
        	}
        	
        	if(result) Debug.Log("성공적으로 저장되었습니다.");
        	else Debug.Warning("기록이 실패하였습니다.");
        }
        ```
        

## SceneRecorder

---

class

싱글톤으로 사용함.

씬 내부를 촬영하고 촬영된 영상을 저장하는 클래스.

- string filePath
    - 기록들이 저장될 경로.

- public void SetPath(string path)
    - filePath를 변경한다.
    - path : 변경할 filePath.

- public bool Capture()
    - 사용자가 바라보는 화면을 캡처하는 함수.
    - return : 저장 성공여부.

- public bool CaptureSelf()
    - 사용자를 바라보는 바깥의 가상 카메라를 캡처하는 함수
    - return : 저장 성공여부.

## SceneMover

---

class

싱글톤으로 사용함.

매 SceneManager 마다 using 추가 소요를 줄이기 위해 사용.

씬 이동시마다 공통된 추가 로직이 필요할 시, 이 스크립트에서 수정

- public void MoveScene(string sceneName)
    - 특정 씬으로 이동
    - sceneName : 이동할 씬 이름.

- public void MoveScene(int sceneIndex)
    - 특정 씬으로 이동
    - sceneIndex : 이동할 씬 index.

## UISelecter

---

class

사용자가 컨트롤러 Ray를 통해 상호작용하여 씬 내부의 오브젝트들의 Unity Event를 Invoke하는 UI 오브젝트.

로비, 난타, 댄스 등 각 씬에서 공통적으로 사용되는 UI의 기능을 정의한다.

- public float InteractionTime
    - 인터렉션에 필요한 시간을 정의하는 멤버.
    - [Range (0,3)]으로 에디터 편의성 고려.

- public UnityEvent[] UIEvents
    - 인터렉션 시 발생하는 이벤트를 정의한다.
    
- IEnumerator InteractionRoutine
    - 컨트롤러로 인해 발생하는 Coroutine을 참조하는 멤버.
    - 루틴은 유일해야 한다.

- IEnumerator StartTimer(float startTime)
    - Ray 인터렉션 시작 시간을 받아 타이머를 시작시킨다.
    - class 내 설정된 시간이 지나면 여전히 변수로 할당된 events들을 Invoke한다.
    
    ```cpp
    IEnumerator StartTimer(float startTime)
    {
    	yield return new WaitForSeconds(InteractionTime);
		foreach(var UIEvent in UIEvents)
		{
			UIEvent?.Invoke();
		}
    }
    ```

- public void GetRayCasted()
    - StartCoroutine으로 StartTimer 루틴을 실행하고 InteractionRoutine에 저장한다.
    - 사용자의 컨트롤러 class에서 Raycast 시 해당 Class에서 호출된다.
    
    ```cpp
    public void GetRayCasted()
    {
    	InteractionRoutine = StartTimer();
        StartCoroutine(InteractionRoutine);
    }
    ```
    

- public void GetRayCastStopped()
    - StopCoroutine으로 기존 루틴을 중단한다.
    - 사용자의 컨트롤러 class에서 Raycast가 중단될 시 해당 Class에서 호출된다.
    
    ```cpp
    public void GetRayCastStopped()
    {
    	StopCoroutine(InteractionRoutine);
    }
    ``` 

## MusicContentTool

---

abstract class

음악과 관련하여 BPM을 분석해 박자 단위의 시간 체계를 초 단위 시간으로 변환하며, 박자 단위로 명령을 입력한 csv 파일을 읽어 Dictionary 형태의 자료구조로 변환한다.

각 씬의 매너지 클래스 중 일부가 상속하며 AbstractSceneManager를 상속받는다.

- public float Beat2Second(float beat, float BPM)
    - 박자 단위를 받아 BPM에 따라 정확한 초를 계산한다.
    - beat : 마디 수.
    - BPM : BPM.
    - return : 주어진 마디를 초 단위 시간으로 환산한 값.
    
    ```cpp
    float Beat2Second(float beat, float BPM)
    {
    	return beat * 60 / BPM;
    }
    ```
    

- public float Second2Beat(float second, float BPM)
    - 초 단위를 받아 BPM에 따라 정확한 마디를 계산한다.
    - second : 초 단위 시간.
    - BPM : BPM.
    - return : 주어진 초 단위 시간을 마디 수로 환산한 값.
    
    ```cpp
    float Second2Beat(float second, float BPM) 
    {
    	return second * BPM /60;
    }
    ```
    
- public Dictionary<float, string> GetScript(string json)
    - 각 판정 이벤트들의 정보가 포함된 JSON파일을 받아 Dictionary 형태로 변환한다.
    - jsonData : JSON 데이터.
    - return : Dictionary 자료구조.
    
    ```cpp
    Dictionary<float, T> GetScript(string jsonData) 
    {
    	return JsonUtility.FromJson<Dictionary<float, string>>(jsonData);
    }
    ```
    
    JSON 데이터는 다음과 같은 구조로 구성된다.
    
    ```json
    {
    	"ChartName": "Rooftop_Easy",
    	"SongName": "Rooftop",
    	"Artist": "Nflying", 
    	"SongLength": 216,
    	"Offset": 1.2,
    	"Notes": [
    		{
    			"Time": 3,
    			"Type": "LeftHand"
    		},
    		{
    			"Time": 4,
    			"Type": "RightHand"
    		}
    	]
    }
    ```
    

- public virtual void PlayChart(string json)
    - 각 노트에 대해 CommandExecute(time, command) 호출.
    - json : JSON 데이터.

- public abstract int JudgeNote(int type)
    - 노트 판정을 내린다.
    - type : 노트의 타입.
    - return : 노트 판정 결과.

- public abstract void CommandExecute(float time, string command)
    - switch구문으로 brach를 나눠 command에 따라 적절한 함수를 실행한다.
    - time : command가 실행될 기준 시간.
    - command : command 구문.

- public abstract override void SetScenario(int scenarioIndex);
    - AbstractSceneManager의 메서드를 override받은 메서드.
    - abstract로 처리되어 자식 클래스에서 정의된다.

## EscapeDoor

---

class

비상탈출구 오브젝트 클래스.

상호작용 시 정해진 UnityEvent를 발생시킨다.

- public UnityEvent[] Events
    - 인터렉션 시 발생하는 이벤트를 정의한다.

- private void OnTriggerEnter(Collider other)
    - Events를 발생시킨다.

## UserDataManager

---

class

싱글톤으로 사용함.

사용자 데이터를 담당하는 클래스.

로컬 파일 형태로 사용자 데이터를 관리하고, 컨텐츠 진행 중 저장된 데이터를 활용하도록 한다.

- string currentProfile
    - 현재 프로필 이름을 저장한다.
    - 초기값은 빈 문자열.
    - getter/setter 제공.

- string[] GetAllDataLocations()
    - 모든 프로필에 대해 데이터 저장 위치를 읽는다.
    - return : 모든 프로필의 저장 위치.

- void ReadData(string targetProfile)
    - 지정된 프로필의 데이터 파일을 읽어 currentProfile을 갱신한다.
    - SceneRecorder.SetPath(currentProfile)로 영상 저장 폴더 설정.
    - targetProfile : 프로필 이름.

- void SaveData()
    - currenProfile에 해당하는 파일의 정보를 저장한다.

- void AddNewData(string currentProfile, string jsonData)
    - 새로운 프로필에 해당하는 폴더를 만든다.
    - currentProfile을 setter를 통해 갱신한다.
    - targetProfile : 새로 생성할 프로필 이름.
    - jsonData : 새로 생성할 프로필 정보.

- private void OnApplicationQuit()
    - 어플리케이션 종료 시 발생 예외처리.
    - SaveData()를 호출한다.

## SoundManager

---

class

싱글톤으로 사용함.

원하는 오디오 클립을 재생하는 클래스. 

- public void SoundPlay(AudioClip clip, AudioSource source)
    - 설정한 효과음 clip을 source 위치에서 재생한다.
    - clip : 재생할 AudioClip.
    - source : 클립이 재생될 AudioSource.

## VibrateControl

---

class

싱글톤으로 사용함.

XR Origin의 ActionBasedController와 연결하여 해당 컨트롤러에 원하는 시간과 강도만큼의 진동을 줄 수 있는 클래스. 

- private ActionBasedController rightController
    - XR Origin - Camera Offset 내의 RightHand Controller 오브젝트를 참조해 불러오는 변수

- private ActionBasedController leftController
    - XR Origin - Camera Offset 내의 LeftHand Controller 오브젝트를 참조해 불러오는 변수

- private GameObject XROrigin
    - 새로운/이동한 씬에 존재하는 XR Origin 오브젝트를 참조하여 저장하기 위한 변수

- public void InitializeController()
    - 맨 처음 게임 실행 후 씬에 들어오거나, 다른 씬으로 이동하였을 때 새로운 Origin이 존재하여 업데이트가 필요한 경우에 사용하는 메소드
    - 해당 씬의 새로운 XR Origin 객체를 찾아 Camera Offset 안의 LeftHand Controller와 RightHand Controller를 인스턴스와 연결해줌
    - foreach 문을 통해 XR Origin의 모든 자식 객체들에 접근하여 탐색

- public IEnumerator CustomVibrateRight(float amplitude, float duration)
    - 오른쪽 컨트롤러가 활성화된 상태인 경우 오른쪽 컨트롤러에 진동을 재생하는 코루틴 함수
    - 컨트롤러가 활성화되지 않은 경우 진동 대신 에러 로그를 띄워줌
    - 실제로 진동을 발생시키는 부분은 ActionBasedController 클래스의 메소드에서 이루어짐
    - amplitude : 0.0에서 1.0 사이의 진동 강도를 지정해줌
    - duration : 얼마나 많은 시간 동안 진동을 재생할지 지정해주며 단위는 초를 사용함
    
    ```csharp
    public IEnumerator CustomVibrateRight(float amplitude, float duration)
    {
        if (rightController != null)
        {
            rightController.SendHapticImpulse(amplitude, duration);
        }
        else
        {
            Debug.LogError("right controller isn't avaliable.");
        }
        yield return null;
    }
    ```
    

- public IEnumerator CustomVibrateLeft(float amplitude, float duration)
    - 왼쪽 컨트롤러가 활성화된 상태인 경우 왼쪽 컨트롤러에 진동을 재생하는 코루틴 함수
    - 컨트롤러가 활성화되지 않은 경우 진동 대신 에러 로그를 띄워줌
    - 실제로 진동을 발생시키는 부분은 ActionBasedController 클래스의 메소드에서 이루어짐
    - amplitude : 0.0에서 1.0 사이의 진동 강도를 지정해줌
    - duration : 얼마나 많은 시간 동안 진동을 재생할지 지정해주며 단위는 초를 사용함
    
    ```csharp
    public IEnumerator CustomVibrateLeft(float amplitude, float duration)
    {
    		if (leftController != null)
    		{
    		    leftController.SendHapticImpulse(amplitude, duration);
    		}
    		else
    		{
    		    Debug.LogError("left controller isn't avaliable.");
    		}
    		
    		yield return null;
    }
    ```

# Login Scene

---

로그인 씬에서 사용되는 오브젝트 별 API이다.

각 클래스들의 기능을 호출하는 UI 오브젝트를 통해 인터렉션한다.

## ProfileManager

---

class

프로필을 설정할 떄 사용하는 클래스이다.

- string newUserColor
    - 신규 사용자 등록 시 색을 캐싱한다.

- string newUserPattern
    - 신규 사용자 등록 시 패턴을 캐싱한다.

- public void SelectProfile(string profileName)
    - 설정한 프로필을 선택해 현재 프로필로 설정한다.
    - UserDataManager.ReadData(profileName)를 호출한다.

- public void MakeNewProfile()
    - newUserColor, newUserPattern를 토대로 새로운 프로필을 만든다.
    - 기본 정보를 포함하는 JSON 파일을 생성한다.
    - UserDataManager.AddNewData(profileName, jsonData)를 호출한다.
    - SelectProfile(profileName)을 호출한다.

# Lobby Scene

---

로비 씬에서 사용되는 오브젝트 별 API이다.

각 클래스들의 기능을 호출하는 UI 오브젝트를 통해 인터렉션한다.

## LobbyManager

---

class

로비 씬 시작 시 같이 생성되는 클래스.

사용자 프로필과 현재 시각을 반환한다.

씬 UI에서 해당 클래스를 통해 필요한 데이터를 보여준다.

- public Sprite ShowProfile()
    - 현재 프로필과 매칭되는 이미지 스프라이트를 반환한다.

- public string ShowCurrentTime()
    - DataTime 라이브러리를 통해 현재 시각을 출력한다.
    - 기준은 KST, 형식은 HHmmss.

# Nanta Scene

---

난타 씬에서 사용되는 오브젝트 별 API이다.

각 클래스들의 기능을 호출하는 UI 오브젝트를 통해 인터렉션한다.

## NantaScenarioManager

---

class

난타 씬 시작 시 같이 생성되는 클래스.

난타 내부 시나리오 전개를 위해 여러 오브젝트들을 생성하거나 제거한다.

MusicContentTool을 상속받는다.

- public enum NoteType {LeftHand, RightHand}
    - 노트 타입을 표현하는 열거형이다.

- public enum NoteResult {Miss, Good}
    - 노트 판정을 표현하는 열거형이다.

- public UnityEvent[] HitEventsLeft
    - 왼쪽 난타 북 타격 판정이 성공할 경우에 대한 이벤트.

- public UnityEvent[] HitFailEventsLeft
    - 왼쪽 난타 북 타격 판정이 실패할 경우에 대한 이벤트.

- public UnityEvent[] HitEventsRight
    - 오른쪽 난타 북 타격 판정이 성공할 경우에 대한 이벤트.

- public UnityEvent[] HitFailEventsRight
    - 오른쪽 난타 북 타격 판정이 실패할 경우에 대한 이벤트.

- public AudioClip[] ComboClips
    - 콤보 달성 시 발생하는 효과음 목록이다.

- public AudioSource MusicAudioSource
    - 배경 음원이 재생되는 AudioSource이다.

- public AudioSource ComboAudioSource
    - 콤보 달성 시 발생하는 효과음이 재생되는 AudioSource이다.

- int barCombo
    - 초기 값을 0으로 한다.
    - 모든 노드를 성공으로 판정받은 마디가 있으면 해당 값을 1 증가시킨다.
    - 판정에 실패하면 즉시 값을 0으로 만든다.

- IEnumerator ComboRoutine
    - AddComboLoop 루틴을 저장한다. 컨텐츠가 끝나면 종료된다.

- void StartMusic(AudioClip audioClip, float barSecond)
    - SoundManager.SoundPlay(audioClip, MusicAudioSource)
    - 콤보 루틴을 실행하여 일정 시간마다 콤보가 쌓이도록 한다.
    - ComboRoutine = StartCoroutine(AddComboLoop(barSecond))

- IEnumerator AddComboLoop(float barSecond)
    - barSecond마다 barCombo += 1
    - barCombo의 값에 따라 SoundManager.SoundPlay(ComboClips, ComboAudioSource) 호출.
        
        
        | barCombo | ComboClips[k] |
        | --- | --- |
        | 2 | k = 0 |
        | 4,6,8,… | k = 1 |

- IEnumerator EndComboLoop(float songLength)
    - WaitforSecond로 songLength초 만큼 대기.
    - 콤보 루틴을 종료한다: StopCoroutine(ComboRoutine);

- public void ResetCombo()
    - barCombo를 0으로 설정한다.

- override void PlayChart(string json)
    - base.PlayChart(json)
    - BPM을 통해 마디 당 초 단위 시간을 계산하고 이를 SPB라 둔다.
        - Beat2Second()를 통해 계산.
    - 적절한 음원 파일을 참조하여 song으로 설정, StartMusic(song, SPB) 호출.
    - StartCoroutine(EndComboLoop(songLength)) 루틴 실행.

- override void CommandExecute(float time, string command)
    - switch구문으로 brach를 나눠 command에 따라 적절한 함수를 실행한다.
        
        
        | command | function |
        | --- | --- |
        | LeftHand | NantaJudgingLine.SpawnNote(time, 0) |
        | RightHand | NantaJudgingLine.SpawnNote(time, 1) |

- override int JudgeNote(int type)
    - NantaJudgingLine.JudgeNote(type)를 호출하여 result를 얻는다.
    - type, result를 적절한 NoteType, NoteResult로 치환한다.
    - type, result에 따라 등록된 이벤트를 출력하고 result를 반환한다.
        
        
        | type\result | Good | Bad |
        | --- | --- | --- |
        | LeftHand | HitEventsLeft | HitFailEventsLeft |
        | RightHand | HitEventsRight | HitFailEventsRight |

## NantaInstrument

---

class

실제로 사용자가 컨트롤러를 통해 상호작용하는 오브젝트.

사용자의 인터렉션이 타격인지 아닌지를 판별하며, 어느 쪽 손의 컨트롤러인지 확인한다.

- public AudioClip[] InstrumentClips
    - 악기와 관련된 효과음 목록이다.

- public AudioSource InstrumentAudioSource
    - 악기와 관련된 효과음이 나오는 곳이다.

- public void OnCollisionEnter(Collidsion other)
    - other == 사용자의 컨트롤러 Collider일 경우,
        - 변수 int type를 왼쪽 컨트롤러일 경우 0, 오른쪽 컨트롤러일 경우 1로 설정한다.
        - NantaScenarioManager.JudgeNote(type) 호출.
            - 결과가 Good일 경우 SoundManager.SoundPlay(InstrumentClips[0], InstrumentAudioSource) 호출.

## NantaJudgingLine

---

class

시간에 따라 노트를 생성하고 이를 통해 사용자에게 타이밍을 인지시키는 클래스.

각각의 노트는 Object Pulling을 활용한다.

- float judgingTime
    - 노트가 생성된 후 판정면에 닿을 때까지의 시간.
    - getter/setter 제공
    - Start시 NoteSpawnTransforms와 JudjingLine 위치를 참고하여 초기값을 설정한다.

- public BoxCollider JudjingLine
    - 폴리곤 크기보다 170% 크게 설정한다.
    - 판정면. 노트가 해당 면에 Trigger된 동안은 성공 판정.
    - 한번이라도 판정면에 Trigger된 후 Trigger에서 Exit될 경우 실패 판정.

- public Transform[] NoteSpawnTransforms
    - 노트가 생성되는 위치를 지정한다.

- public void SpawnNote(float time, int type)
    - StartCoroutine으로 SpawnNoteRoutine(time, type) 루틴 실행.
    
- IEnumerator SpawnNoteRoutine(float time, int type)
    - time - judgingTime 만큼  WaitforSecond를 통해 대기.
        - if(time - judgingTime > 0), 0초 대기.
    - 노트를 NoteSpawnTransforms[type].position에 생성하여 NoteSpawnTransforms[type].rotation 방향으로 등속 운동시킨다.

- public int JudgeNote(int type)
    - 노트 타입에 연결되는 라인에서 노트 진행 방향으로 가장 멀리 이동한 노트를 참조한다.
    - 참조한 노트가 해당 면에 Trigger된 상태라면 1을 반환한다.
    - 그 외엔 0을 반환한다.

# Dance Scene

---

댄스 씬에서 사용되는 오브젝트 별 API이다.

각 클래스들의 기능을 호출하는 UI 오브젝트를 통해 인터렉션한다.

## DanceScenarioManager

---

class

댄스 씬 시작 시 같이 생성되는 클래스.

댄스 내부 시나리오 전개를 위해 여러 오브젝트들을 생성하거나 제거한다.

MusicContentTool, AbstarctSceneManager을 상속받는다.

- public enum NoteType {LeftUpper, RightUpper, LeftMiddle, RightMiddle, Front}
    - 노트 타입을 표현하는 열거형이다.

- public enum NoteResult {Miss, Good}
    - 노트 판정을 표현하는 열거형이다.

- public UnityEvent[] TriggerEventsLeftUpper
    - 왼쪽 위 댄스 판정이 성공할 경우에 대한 이벤트.

- public UnityEvent[] TriggerFailEventsLeftUpper
    - 왼쪽 위 댄스 판정이 실패할 경우에 대한 이벤트.

- public UnityEvent[] TriggerEventsRightUpper
    - 오른쪽 위 댄스 판정이 성공할 경우에 대한 이벤트.

- public UnityEvent[] TriggerFailEventsRightUpper
    - 오른쪽 위 댄스 판정이 실패할 경우에 대한 이벤트.

- public UnityEvent[] TriggerEventsLeftMiddle
    - 왼쪽 중간 댄스 판정이 성공할 경우에 대한 이벤트.

- public UnityEvent[] TriggerFailEventsLeftMiddle
    - 왼쪽 중간 댄스 판정이 실패할 경우에 대한 이벤트.

- public UnityEvent[] TriggerEventsRightMiddle
    - 오른쪽 중간 댄스 판정이 성공할 경우에 대한 이벤트.

- public UnityEvent[] TriggerFailEventsRightMiddle
    - 오른쪽 중간 댄스 판정이 실패할 경우에 대한 이벤트.

- public UnityEvent[] TriggerEventsRightMiddle
    - 오른쪽 중간 댄스 판정이 성공할 경우에 대한 이벤트.

- public UnityEvent[] TriggerFailEventsRightMiddle
    - 오른쪽 중간 댄스 판정이 실패할 경우에 대한 이벤트.

- public AudioClip[] ComboClips
    - 콤보 달성 시 발생하는 효과음 목록이다.

- public AudioSource MusicAudioSource
    - 배경 음원이 재생되는 AudioSource이다.

- public AudioSource ComboAudioSource
    - 콤보 달성 시 발생하는 효과음이 재생되는 AudioSource이다.

- int barCombo
    - 초기 값을 0으로 한다.
    - 모든 노드를 성공으로 판정받은 마디가 있으면 해당 값을 1 증가시킨다.
    - 판정에 실패하면 즉시 값을 0으로 만든다.

- IEnumerator ComboRoutine
    - AddComboLoop 루틴을 저장한다. 컨텐츠가 끝나면 종료된다.

- void StartMusic(AudioClip audioClip, float barSecond)
    - SoundManager.SoundPlay(audioClip, MusicAudioSource)
    - 콤보 루틴을 실행하여 일정 시간마다 콤보가 쌓이도록 한다.
    - ComboRoutine = StartCoroutine(AddComboLoop(float barSecond))

- IEnumerator AddComboLoop(float barSecond)
    - barSecond마다 barCombo += 1
    - barCombo의 값에 따라 SoundManager.SoundPlay(ComboClips, ComboAudioSource) 호출.
        
        
        | barCombo | ComboClips[k] |
        | --- | --- |
        | 2 | k = 0 |
        | 4,6,8,… | k = 1 |

- IEnumerator void EndComboLoop(float songLength)
    - WaitforSecond로 songLength초 만큼 대기.
    - 콤보 루틴을 종료한다: StopCoroutine(ComboRoutine);

- public void ResetCombo()
    - barCombo를 0으로 설정한다.

- override void PlayChart(string json)
    - base.PlayChart(json)
    - BPM을 통해 마디 당 초 단위 시간을 계산하고 이를 SPB라 둔다.
        - MusicContentTool.Beat2Second()를 통해 계산.
    - 적절한 음원 파일을 참조하여 song으로 설정, StartMusic(song, SPB) 호출.
    - StartCoroutine(EndComboLoop(songLength)) 루틴 실행.

- override void CommandExecute(float time, string command)
    - switch구문으로 brach를 나눠 command에 따라 적절한 함수를 실행한다.
        
        
        | command | function |
        | --- | --- |
        | Trigger({type, type, …}) | DanceJudjingPoint.SpawnNote(time, [type, type, …])
        DanceInstructor.ShowInstructor(time, [type, type, …]) |

- override NoteResult JudgeNote(int type)
    - DanceJudjingPoint.JudgeNote(type)를 호출하여 result를 얻는다.
    - type, result를 적절한 NoteType, NoteResult로 치환한다.
    - type, result에 따라 등록된 이벤트를 출력하고 result를 반환한다.
        
        
        | type\result | Good | Bad |
        | --- | --- | --- |
        | LeftUpper | TriggerEventsLeftUpper | TriggerFailEventsLeftUpper |
        | RightUpper | TriggerEventsRightUpper | TriggerFailEventsRightUpper |
        | LeftMiddle | TriggerEventsLeftMiddle | TriggerFailEventsLeftMiddle |
        | RightMiddle | TriggerEventsRightMiddle | TriggerFailEventsRightMiddle |
        | Front | TriggerEventsLeftFront | TriggerFailEventsFront |

## DanceInstructor

---

class

사용자가 정확한 인터렉션을 할 수 있도록 기능을 제공한다.

- IEnumerator mirrorRoutine
    - ShowMirrorImageRoutine() 루틴을 참조한다.

- public Sprite[] Instructions
    - 어떤 포즈를 취해야 할지 알려주는 그림 이미지들의 목록.

- public Camera MirrorCam
    - 사용자를 반대 방향에서 촬영하는 카메라.

- public GameObject Mirror
    - MirrorCam에서 촬영한 이미지가 반영되는 상.

- public GameObject InstructorPanel
    - Instructions 이미지가 보여질 UI 패널.

- public void ShowMirrorImage()
    - StartCoroutine으로 ShowMirrorImageRoutine() 루틴을 실행한다.
    - mirrorRoutine = StartCoroutine(ShowMirrorImageRoutine());

- public void TurnOffMirrorImage()
    - StopCoroutine으로 mirrorRoutine 루틴을 종료한다.

- IEnumerator ShowMirrorImageRoutine()
    - MirrorCam을 통해 사용자의 모습을 Mirror에 투영한다.

- void ReadyForInstruction(float time, int[] types)
    - 입력받는 types를 고려해 Instructions에서 적절한 이미지를 찾아 instruction로 둔다.
    - StartCoroutine으로 ShowInstruction(time, instruction) 실행.

- IEnumerator ShowInstruction(float time, Sprite instruction)
    - WaitforSecond로 time만큼 대기.
    - InstructorPanel에 해당 이미지를 반영한다.

## DanceJudjingPoint

---

class

시간에 따라 노트를 생성하고 이를 통해 사용자에게 타이밍을 인지시키는 클래스.

각각의 노트는 Object Pulling을 활용한다.

- float judgingTime
    - 노트가 생성된 후 판정을 가할 때까지의 시간.
    - getter/setter 제공

- public BoxCollider[] Triggers
    - 컨트롤러로 트리거 가능한 범위이다.

- public AudioClip[] InstrumentClips
    - 악기와 관련된 효과음 목록이다.

- public AudioSource InstrumentAudioSource
    - 악기와 관련된 효과음이 나오는 곳이다.

- public Transform[] NoteSpawnTransforms
    - 노트가 생성되는 위치를 지정한다.

- public void SpawnNote(float time, int[] types)
    - StartCoroutine으로 SpawnNoteRoutine(time, types) 루틴 실행.
    
- IEnumerator SpawnNoteRoutine(float time, int[] types)
    - time - judgingTime 만큼 WaitforSecond를 통해 대기.
        - if(time - judgingTime > 0), 0초 대기.
    - foreach(var type in types)
        - 노트를 NoteSpawnTransforms[type].position에 생성하여 StartCoroutine으로 PlayNoteRoutine() 서브루틴을 실행한다.

- IEnumerator PlayNoteRoutine()
    - judgingTime 만큼 WaitforSecond를 통해 대기.
    - DanceScenarioManager.JudgeNote(type) 호출.
        - 결과가 Good일 경우 SoundManager.SoundPlay(InstrumentClips[0], InstrumentAudioSource) 호출.

- public int JudgeNote(int type)
    - Trigger[type]에 컨트롤러가 Trigger중인지 확인한다.
    - Trigger된 상태라면 1을, 그 외엔 0을 반환한다.

# Gallery Scene

---

갤러리 씬에서 사용되는 오브젝트 별 API이다.

각 클래스들의 기능을 호출하는 UI 오브젝트를 통해 인터렉션한다.

## RecordPlayer

---

class

영상 자료를 찾아 반환한다.

- string[] filePaths
    - 반환 가능한 영상들이 저장된 위치이다.

- void GetPath()
    - UserDataManager.ReadAllData()로 filePaths를 갱신한다.

- Sprite[] GetThumbnails()
    - 전체 영상에 해당하는 썸네일 스프라이트를 배열로 반환한다.

- GameObject GetVideo(string videoName)
    - 해당하는 이름의 비디오를 재생 가능한 오브젝트를 생성한다.
    - 해당 API의 IPO는 이후 개발 상황에 따라 변경할 것.