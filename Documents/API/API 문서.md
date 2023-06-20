# VR 음악센터 기술 명세서

---

VR 음악센터는 다음의 5개 씬으로 구성된다.

최초 프로그램 시작 시 Login Scene에서 시작되며,

User Interaction을 통해 화살표를 따라 각 씬으로 이동 가능하다.

아래는 각 씬에서 정의된 기능들의 목록이다.

1. [Global Function](https://github.com/TimmyRoom/TimmyRoom/blob/develop/Documents/API/API%20%EB%AC%B8%EC%84%9C.md#global-function)
2. [Login Scene](https://github.com/TimmyRoom/TimmyRoom/blob/develop/Documents/API/API%20%EB%AC%B8%EC%84%9C.md#login-scene)
3. [Lobby Scene](https://github.com/TimmyRoom/TimmyRoom/blob/develop/Documents/API/API%20%EB%AC%B8%EC%84%9C.md#lobby-scene)
4. [Nanta Scene](https://github.com/TimmyRoom/TimmyRoom/blob/develop/Documents/API/API%20%EB%AC%B8%EC%84%9C.md#nanta-scene)
5. [Dance Scene](https://github.com/TimmyRoom/TimmyRoom/blob/develop/Documents/API/API%20%EB%AC%B8%EC%84%9C.md#dance-scene)
6. [Gallery Scene](https://github.com/TimmyRoom/TimmyRoom/blob/develop/Documents/API/API%20%EB%AC%B8%EC%84%9C.md#gallery-scene)

# Global Function

---

여러 씬에서 중복적으로 사용되는 기능들이다.

## AbstractSceneManager

---

Abstract class

각 씬의 매니저 클래스가 포함할 멤버와 메서드를 담는 추상 클래스.
using 추가 소요를 줄이고, SceneMover 싱글톤 사용 유도한다.

- public Transform XRCamera
    - XR Origin의 Main Camera 위치를 담는 변수.

- public Transform StartTransform
    - 씬의 초기 위치를 담는 변수. 기준은 XR Origin의 Main Camera.

- public Transform XRCameraOffset
    - XR Origin의 Camera Offset을 담는 변수.

- public void ResetPosition()
    - 사용자 위치를 초기화하는 함수

- public void MoveScene(string sceneName)
    - 특정 씬으로 이동하는 로직을 담은 함수

- public void MoveScene(int sceneIndex)
    - 특정 씬으로 이동하는 로직을 담은 함수

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
## IScenario

---

interface

AbstractSceneManager에서 시나리오로 분류되는 클래스들의 인터페이스.

- public Dictionary<int, UnityAction> GetActions()
    - 해당 인스트럭션에서 발생하는 액션들의 리스트를 반환한다.
    - returns : 인스트럭션에서 발생하는 액션들의 리스트

## Action

---

class 

Note에서 다루는 각 노트의 액션.

- public string Name
    - 액션의 이름.

- public int Type
    - 액션의 타입.

## Note

---

class

GameChart에서 다루는 각 노트의 클레스.

- public float Time
    - 노트 등장 시간.

- public List<Action> Actions
    - 노트에 포함된 액션 목록.

## GameChart

---

class

채보 데이터를 나타내는 클래스. JSON 파싱 후 해당 형태로 변환된다.

- public string ChartName
    - 차트의 이름.

- public string SongName
    - 음악의 이름.

- public string ArtistName
    - 아티스트의 이름.

- public float SongLength
    - 음악의 길이.

- public float Offset
    - 음악의 초기 Offset.

- public float BPM
    - 음악의 BPM.

- public List<Note> Notes
    - 음악에 등장하는 모든 노트들의 리스트.

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

- public bool Record()
    - 사용자가 바라보는 화면을 녹화하는 함수
    - return : 저장 성공여부

- public bool RecordSelf()
    - 사용자를 바라보는 바깥의 가상 카메라 화면을 녹화하는 함수
    - return : 저장 성공여부


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

사용자가 컨트롤러 Ray를 통해 상호작용하여

캔버스 내부의 오브젝트들의 Unity Event를 Invoke하는 클래스.

로비, 난타, 댄스 등 각 씬에서 공통적으로 사용되는 UI의 기능을 정의한다.

사용 시에는 각 UI 오브젝트에 스크립트 컴포넌트를 추가하고 이벤트를 지정해준다.

- public float InteractionTime
    - 인터랙션에 필요한 시간을 정의하는 멤버.
    - [Range (0,3)]으로 에디터 편의성 고려. 기본값은 2.0f

- public UnityEvent[] UIEvents
    - 인터랙션 시 발생하는 이벤트를 정의한다.
    
- private bool buttonPressed
    - 컨트롤러의 오브젝트 중복 입력을 막기 위해 설정한 멤버.
    - 포인터가 오브젝트를 나가면 초기화된다.

- public void OnPointerEnter(PointerEventData eventData)
    - 사용자의 컨트롤러 Ray가 UI 오브젝트(버튼 등)로 들어왔을 때 시간을 잰다.
    - 버튼이 한 번이라도 눌리지 않았을 경우에만 시간을 측정한다.
    
    ```cpp
    public void OnPointerEnter(PointerEventData eventData)
    {
    		if (!buttonPressed)
    		{
    				pressTime = Time.time;
    		}
    }
    ```
    

- public void OnPointerExit(PointerEventData eventData)
    - 사용자의 컨트롤러 Ray가 UI 오브젝트를 나갔을 때 기존 루틴을 중단한다.
    - 눌린 시간을 초기화하고 기존에 버튼이 눌린 적이 있으면 그 기록을 초기화한다.
    
    ```cpp
    public void OnPointerExit(PointerEventData eventData)
    {
          pressTime = 0.0f;
          buttonPressed = false;
    }
    ```
    

- private void Update()
    - Ray가 오브젝트에 들어온 시간을 측정하여 일정 시간동안 입력이 들어왔는지 체크한다.
    - class 내 설정된 시간이 지나면 변수로 할당된 events들을 Invoke한다.
    - 중복 입력을 막기 위해 버튼이 입력되었음을 기록해준다.
    
    ```cpp
    private void Update()
    {
    		if(!buttonPressed && pressTime > 0.0f)
    		{
    				if(Time.time - pressTime >= InteractionTime)
    				{
    						foreach(var UIEvent in UIEvents)
    						{
    								UIEvent?.Invoke();
    						}
    						buttonPressed = true;
    				}
    		}
    }
    ```

## MusicContentTool

---

abstract class

음악과 관련하여 BPM을 분석해 박자 단위의 시간 체계를 초 단위 시간으로 변환하며, 박자 단위로 명령을 입력한 csv 파일을 읽어 Dictionary 형태의 자료구조로 변환한다.

각 씬의 매너지 클래스 중 일부가 상속하며 AbstractSceneManager를 상속받는다.

- protected IEnumerator RecordAndCapture()
    - 콘텐츠 실행 중 무작위 시간에 SceneRecorder를 사용자의 화면을 캡쳐한다.

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

- public abstract float GetWaitTime()
    - 음악 재생 전 대기하는 시간을 기록한다. 
    - 이는 노트 낙하 및 등장 시간으로 인해 WaitforSecond의 파라미터로 음수 시간이 할당되는 것을 방지하기 위함이다.
    - returns : 노트 이펙트 생성에 필요한 최대 시간

- public abstract int JudgeNote(int type)
    - 노트 판정을 내린다.
    - type : 노트의 타입.
    - return : 노트 판정 결과.

- public abstract void CommandExecute(float time, List<Action> actions)
    - switch구문으로 branch를 나눠 적절한 함수를 실행한다.
    - time : 액션이 실행될 기준 시간.
    - command : 실행될 액션 목록.

- public abstract void ResetAll()
    - 씬의 상태를 채보 시작 이전 상태로 되돌린다.
    
- public abstract void SetScenario(int scenarioIndex)
    - 현재 시나리오를 scenario 번호에 따라 설정하고 시나리오에 맞는 오브젝트 및 데이터, UI를 생성하거나 삭제한다.
    - scenarioIndex : 변경할 시나리오의 Index.

## EscapeDoor

---

class

비상탈출구 오브젝트 클래스.

상호작용 시 정해진 UnityEvent를 발생시킨다.

- public AudioClip EscapeSound
    - 탈출 버튼 작동 시 재생되는 효과음.

- public UnityEvent EscapeEvent
    - 탈출 버튼 작동 시 실행되는 이벤트.

- IEnumerator Escape()
    - 일정 시간을 두고 EscapeEvent를 발생시킨다.

## UserDataManager

---

class

싱글톤으로 사용함.

사용자 데이터를 담당하는 클래스.

로컬 파일 형태로 사용자 데이터를 관리하고, 컨텐츠 진행 중 저장된 데이터를 활용하도록 한다.

- int currentProfile
    - 현재 프로필 아이디을 저장한다.
    - 초기값은 빈 정수.
    - getter/setter 제공.

- void ReadData(int targetProfile)
    - set currentProfile
    - targetProfile을 받아서 SceneRecorder의 userData 설정
    - targetProfile : 프로필 아이디.

- void SaveData()
    - 유저 정보들을 저장한다.

- void AddNewData(int colorId, int patternId)
    - 새로운 프로필을 만들어서 GameData에 추가한다.
    - colorId, patternId : 새로운 프로필의 색상과 패턴

- private void OnApplicationQuit()
    - 어플리케이션 종료 시 발생 예외처리.
    - SaveData()를 호출한다.

## GameData & UserData

---

class
- UserData: System.Serializable / 사용자 정보
- GameData: static / List<UserData> 저장중

사용자 데이터 형식을 다루는 클래스

- bool CheckPurity(int colorId, int patternId)
    - colorId, patternId : 사용자가 고른 컬러와 패턴
    - returns : 하나도 없다면 true, 아니면 false

- int AddUser(int colorId, int patternId)
    - CheckPurity 호출해서 검사
    - 새로운 userData 생성
    - 새로운 userid 세팅


## SoundManager

---

class

싱글톤으로 사용함.

원하는 오디오 클립을 재생하는 클래스. 

- public void SoundPlay(AudioClip clip, AudioSource source)
    - 설정한 효과음 clip을 source 위치에서 재생한다.
    - clip : 재생할 AudioClip.
    - source : 클립이 재생될 AudioSource.

- public void StopSound(AudioSource source)
    - 설정한 AudioSource의 재생 음악을 멈춘다.
    - source : 멈추고자 하는 AudioSource.

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

## SignUpManager

---

class

프로필을 설정할 떄 사용하는 클래스이다.

- public void SetColor(UserColor color)
    - 사용자가 선택한 컬러를 캐싱한다.

- public void SetPattern(UserPattern pattern)
    - 사용자가 선택한 패턴을 캐싱한다.

- public void ConfirmColor()
    - 캐싱된 색상을 확정하고 패턴 창을 보여준다.

- public void ConfirmPattern()
    - 캐싱된 패턴을 확정하고, UserDataManager를 활용하여 새로운 프로필을 생성한다.
    - 생성되지 않는다면 회원가입창을 원상복구 시킨다.

## Profile

---

class

로그인을 할때 프로필 버튼에 할당되어 사용하는 클래스이다.

- public void SelectProfile(int id, int colorId, int patternId)
    - 현재 프로필 버튼 오브젝트를 해당 매개변수로 설정한다.

- public void ClickProfile()
    - UserDataManager의 프로필을 현재 프로필로 맞춘다.
    - ModelManager의 아바타를 현재 프로필로 변경시킨다.
    - LobbySceneManager를 통해 로비 씬으로 이동시킨다.


# Lobby Scene

---

로비 씬에서 사용되는 오브젝트 별 API이다.

각 클래스들의 기능을 호출하는 UI 오브젝트를 통해 인터렉션한다.

## LobbySceneManager

---

class

로비 씬 시작 시 같이 생성되는 클래스.

사용자 프로필과 현재 시각을 반환한다.

씬 UI에서 해당 클래스를 통해 필요한 데이터를 보여준다.

- public Sprite ShowProfile()
    - 현재 프로필과 매칭되는 이미지 스프라이트를 반환한다.

- public static string GetCurrentTime()
    - DataTime 라이브러리를 통해 현재 시각을 출력한다.
    - 기준은 KST, 형식은 HHmmss.

# Nanta Scene

---

난타 씬에서 사용되는 오브젝트 별 API이다.

NantaScenarioManager가 설정한 시나리오에 따라 오브젝트를 배치한다.
사용자는 NantaInstrument를 통해 발생한 인터렉션을 발생시킨다.
해당 인터렉션은 NantaJudgingLine을 통해 판정한다. 
판정 결과는 NantaScenarioManager에게 전송되어 적절한 이벤트를 발생시킨다.

## AbstractNantaInstrument

---

abstract class

난타 씬에서 악기로 사용되는 오브젝트 스크립트의 추상 클래스.

- public AudioClip[] InstrumentClips
    - 악기와 관련된 효과음 목록.

- public AudioSource InstrumentAudioSource
    - 악기와 관련된 효과음이 나오는 곳.

- protected NantaJudgingLine Judge
    - 판정을 위해 참조하는 클래스.

- public abstract void Initialize()
    - 악기의 초기화를 위한 함수.

- public abstract void OnDisappear()
    - 악기가 비활성화될 때 호출되는 함수.

- public abstract void GetHitted(int type)
    - 악기가 사용자에 의해 인터렉션되었을 때 호출되는 함수.

## NantaScenarioManager

---

class

난타 씬 시작 시 같이 생성되는 클래스.

난타 내부 시나리오 전개를 위해 여러 오브젝트들을 생성하거나 제거한다.

MusicContentTool, AbstarctSceneManager을 상속받는다.

- private NantaJudgingLine nantaJudgeLine
    - 난타 북의 판정을 담당하는 클래스이다.

- private NantaInstrumentManager nantaInstrumentManager
    - 난타 악기 오브젝트들을 관리하는 클래스이다.

- public float VibrateTime
    - 난타 판정 발생 시 진동이 발생하는 시간.

- public float VibrateAmplifier
    - 난타 판정 발생 시 진동의 세기.

- public GameObject[] Scenarios
    - 각 상황마다 등장하는 UI이다.

- public enum EventType
    - Instruction에서 발생 가능한 이벤트 타입을 표현하는 열거형이다.

- Dictionary<EventType, UnityEvent> ScenarioEvents
    - 난타 북 타격 판정에 따른 이벤트 목록.
    - EventType에 따라 적절한 이벤트를 발생시킨다.

- public AudioSource MusicAudioSource
    - 배경 음원이 재생되는 AudioSource이다.

- IEnumerator SongRoutine
    - 음악을 재생하는 루틴을 저장한다. 컨텐츠가 끝나면 종료된다.

- IEnumerator StopRoutine
    - 음악 종료 후 발생하는 이벤트를 처리하는 루틴을 저장한다. 컨텐츠가 끝나면 종료된다.

- void Initialize()
    - 초기 설정을 위한 함수.

- void StartMusic(AudioClip audioClip, float barSecond)
    - SoundManager.SoundPlay(audioClip, MusicAudioSource)
    - 콤보 루틴을 실행하여 일정 시간마다 콤보가 쌓이도록 한다.
    - ComboRoutine = StartCoroutine(AddComboLoop(float barSecond))

- void StopMusic()
  - 음악을 멈추고 루틴을 종료한다. 

- override void PlayChart(string json)
    - base.PlayChart(json)
    - BPM을 통해 마디 당 초 단위 시간을 계산하고 이를 SPB라 둔다.
        - MusicContentTool.Beat2Second()를 통해 계산.
    - 적절한 음원 파일을 참조하여 song으로 설정, StartMusic(song, SPB) 호출.
    - StartCoroutine(EndComboLoop(songLength)) 루틴 실행.

- public override float GetWaitTime()
    - NantaJudgingLine의 judgingTime을 반환한다.

- IEnumerator PlayChartRoutine(AudioClip audioClip, float waitTime, float barSecond)
    - 일정 시간 후 음악을 재생하는 코루틴.
    - audioClip : 재생할 음악.
    - waitTime : 음악 재생 전 대기 시간.
    - barSecond : 마디 당 초 단위 시간.

- public override void CommandExecute(float time, string command)
    - switch구문으로 brach를 나눠 command에 따라 적절한 함수를 실행한다.
        | command | function |
        | --- | --- |
        | LeftHand | NantaJudgingLine.SpawnNote(time, 0) |
        | RightHand | NantaJudgingLine.SpawnNote(time, 1) |
        | ^ChangeInstrument .$ | NantaInstrumentManager.ChangeInstrument(time, int.Parse(command.Split(' ')[1])) |

- public void ChangeInstrumentInstantly(int type)
    - 악기를 즉시 교체한다.
    - type : 악기의 종류

- public override NoteResult JudgeNote(int type)
    - NantaJudgingLine.JudgeNote(type)를 호출하여 result를 얻는다.
    - type, result를 적절한 NoteType, NoteResult로 치환한다.
    - type, result에 따라 등록된 이벤트를 출력하고 result를 반환한다.

- public override void SetScenario(int scenarioIndex)
    - 각 시나리오에 맞는 적절한 인터페이스 오브젝트를 활성화한다.
    - 시나리오가 끝날 때 발생하는 이벤트들을 Invoke한다.
    - 0번쨰 인덱스의 Instrument를 활성화한다.
    - 각 시나리오에 맞게 판정 이벤트를 초기화 후 새롭게 매핑한다.
    - 시나리오 시작 시 발생하는 이벤트들을 Invoke한다.

- public override void ResetAll()
    - 씬 시작 상태로 되돌리는 함수.


## NantaInstrumentManager

---

class

난타 씬에서 사용되는 악기 오브젝트를 관리하는 클래스.

- public AbstractNantaInstrument[] Instruments
    - 씬 진행에 사용되는 모든 악기들의 집합.

- private List<IEnumerator> changeRoutines
    - 악기 교체에 사용되는 코루틴의 리스트.

- public void Initialize()
    - 초기 설정을 위한 함수.

- public void ChangeInstrument(float time, int instrumentIndex)
    - 악기를 교체하는 함수.
    - time : 악기 교체가 일어나는 시간.
    - instrumentIndex : 교체할 악기의 인덱스.

- IEnumerator ChangeRoutine(float time, int instrumentIndex)
    - 악기를 교체하는 코루틴.
    - time : 악기 교체가 일어나는 시간.
    - instrumentIndex : 교체할 악기의 인덱스.

- public void ResetAll()
    - 씬 시작 상태로 되돌리는 함수.


## NantaJudgingLine

---

class

시간에 따라 노트를 생성하고 이를 통해 사용자에게 타이밍을 인지시키는 클래스.

- public float FallingTime
    - 노트가 생성된 후 판정면에 닿을 때까지의 시간.
    - getter/setter 제공

- float noteVelocity
    - 노트의 등속 운동 속도.
    - getter/setter 제공

- public Transform[] RayPosition;
    - 판정을 위한 Raycast가 발생하는 Transform. 노트가 낙하하는 각 라인 별로 존재한다.

- public Transform[] JudgePosition;
    - 실제 사용자가 보는 판정선 Transform. 노트가 낙하하는 각 라인 별로 존재한다.

- public Transform[] NoteSpawnTransforms
    - 노트가 생성되는 위치. 노트가 낙하하는 각 라인 별로 존재한다.

- public Rigidbody NotePrefab
    - 생성하는 노트의 프리팹.
    - Rigidbody로 등록하여 코드 내에서 사용하기 편하게 한다.

- private List<Rigidbody> notes
    - 생성된 모든 노트들을 저장하는 리스트.

- private List<IEnumerator> noteRoutines
    - 실행되는 모든 노트 루틴을 저장하는 리스트.

- public void SetVelocity()
    - 노트의 속력을 설정한다.

- public void SpawnNote(float time, int type)
    - StartCoroutine으로 SpawnNoteRoutine(time, type) 루틴 실행.
    - time : 노트가 생성되는 시간.
    - type : 노트가 생성되는 라인.
    
- public void SpawnNoteWithChange(float time, int type)
    - SpawnNoteRoutineWithChange(time, type) 루틴 실행.
    - time : 루틴이 시작될 시간.
    - type : 노트가 생성되는 라인.

- IEnumerator SpawnNoteRoutine(float time, int type)
    - time - judgingTime 만큼  WaitforSecond를 통해 대기.
        - if(time - judgingTime > 0), 0초 대기.
    - 노트를 NoteSpawnTransforms[type].position에 생성하여 NoteSpawnTransforms[type].rotation 방향으로 등속 운동시킨다.
    - time : 노트 생성까지 대기하는 시간.
    - type : 노트가 생성되는 라인.

- IEnumerator SpawnNoteRoutineWithChange(float time, int type)
    - 일정 시간동안 대기 후 악기 변환 노트를 생성하여 움직이게 한다.
    - time : 생성까지 대기하는 시간.
    - type : 노트가 생성되는 라인.

- public int JudgeNote(int type)
    - 노트 타입에 연결되는 라인에서 노트 진행 방향으로 가장 멀리 이동한 노트를 참조해 판정을 한다.
    - 이후 NantaScenarioManager.JudgeNote()를 호출하여 적절한 이벤트를 발생시킨다.
    - type : 판정을 진행할 라인.
    - returns : 노트 판정 결과.

- Rigidbody GetNote(int type)
    - 노트를 생성하고 해당 오브젝트의 RigidBody 컴포넌트를 반환한다.
    - returns : 생성된 노트의 RigidBody.

- Rigidbody GetNoteWithChange(int type)
    - 악기 변환 노트를 생성하고 해당 오브젝트의 RigidBody 컴포넌트를 반환한다.
    - returns : 생성된 노트의 RigidBody.

- public void ResetAll()
    - 씬 시작 상태로 되돌리는 함수.

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

- public DanceJudgingPoint danceJudgingPoint
    - 댄스 포즈 판정을 담당하는 클래스이다.

- public DanceAreaManager danceAreaManager
    - 댄스 포즈 트리거 구역을 담당하는 클래스이다.

- public GameObject[] Scenarios;
    - 각 상황마다 등장하는 UI이다.

- public float correctVTime
- public float correctVAmplifier
    - 정답 판정 시의 진동 지속시간 및 세기이다.
    - 0.0에서 1.0 사이의 float 값을 임의로 지정해줄 수 있다.

- public float wrongVTime
- public float wrongVAmplifier
    - 정답 판정 시의 진동 지속시간 및 세기이다.
    - 0.0에서 1.0 사이의 float 값을 임의로 지정해줄 수 있다.

- public enum EventType {Hit, Fail, Start, End}
    - Instruction에서 발생 가능한 이벤트 타입을 표현하는 열거형이다.

- public AudioSource MusicAudioSource
    - 배경 음원이 재생되는 AudioSource이다.

- public int currentScenarioNum
    - 현재 진행중인 시나리오의 번호이다.

- IEnumerator SongRoutine
    - 음악을 재생하는 루틴을 저장한다. 컨텐츠가 끝나면 종료된다.

- private Dictionary<EventType, UnityEvent> ScenarioEvents
    - 댄스 포즈 판정에 따른 이벤트.
    - EventType에 따라 적절한 이벤트를 발생시킨다.

- void Initialize()
    - 초기 설정을 위한 함수.

- public override GameChart PlayChart(string json, AudioClip audioClip)
    - 각 노트에 대해 CommandExecute(time, command) 호출.
    - json : 차트 JSON 데이터.
    - audioClip : 재생할 음원.

- public override float GetWaitTime()
    - DanceJudgingPoint의 의 fallingTime을 반환한다.

- public override void CommandExecute(float time, string command)
    - switch구문으로 brach를 나눠 command에 따라 적절한 함수를 실행한다.
        | command | function |
        | --- | --- |
        | Hit | DanceJudgingPoint.SpawnNote(time, action.Type) |

- public override int JudgeNote(int type, int result)
    - 노트 판정에 따른 이벤트를 발생시킨다.
    - type, result에 따라 등록된 이벤트를 출력하고 result를 반환한다.
        | type | mean |
        | --- | --- |
        | 11 | Left Down - Right Down |
        | 12 | Left Down - Right Middle |
        | 13 | Left Down - Right Up |
        | 21 | Left Middle - Right Down |
        | 22 | Left Middle - Right Middle |
        | 23 | Left Middle - Right Up |
        | 31 | Left Up - Right Down |
        | 32 | Left Up - Right Middle |
        | 33 | Left Up - Right Up |

- public override void SetScenario(int scenarioIndex)
    - 각 시나리오에 맞는 적절한 인터페이스 오브젝트를 활성화한다.
    - 시나리오가 끝날 때 발생하는 이벤트들을 Invoke한다.
    - 변경할 시나리오의 index를 현재 시나리오 번호로 지정해준다.
    - 각 시나리오에 맞게 판정 이벤트를 초기화 후 새롭게 매핑한다.
    - 시나리오 시작 시 발생하는 이벤트들을 Invoke한다.

- public override void ResetAll()
    - 씬 시작 상태로 되돌리는 함수.

- IEnumerator PlayChartRoutine(AudioClip audioClip, float waitTime, float barSecond)
    - 일정 시간 후 음악을 재생하는 코루틴.
    - audioClip : 재생할 오디오 클립.
    - waitTime : 대기 시간.
    - barSecond : 마디당 소요 시간.

- void StartMusic(AudioClip audioClip, float barSecond)
    - SoundManager를 통해 음악을 재생한다.
    - audioClip : 재생할 오디오 클립.
    - barSecond : 마디당 소요 시간.

## DanceJudgingPoint

---

class

시간에 따라 노트를 생성하고 이를 통해 사용자에게 타이밍을 인지시키는 클래스.

- float fallingTime
    - 노트가 생성된 후 판정면에 닿을 때까지의 시간.
    - getter/setter 제공

- float noteVelocity
    - 노트의 등속 운동 속도.
    - getter/setter 제공

- public Transform JudgePosition
    - 실제 사용자가 보는 판정선 Transform.

- public GameObject JudgePointGuide
    - 사용자들이 눈으로 확인 가능한 판정 범위 오브젝트.

- public Transform NoteSpawnTransforms
    - 노트가 생성되는 위치.

- public Rigidbody NotePrefab
    - 생성하는 노트의 프리팹.
    - Rigidbody로 등록하여 코드 내에서 사용하기 편하게 한다.

- public List<Sprite> sprites
    - 노트의 포즈 스프라이트들을 저장하는 리스트.

- private List<Rigidbody> notes
    - 생성된 모든 노트들을 저장하는 리스트.

- private List<IEnumerator> noteRoutines
    - 실행되는 모든 노트 루틴을 저장하는 리스트.

- private bool[] current
    - 판정을 위해 현재 발동된 트리거 정보를 자체적으로 저장하는 bool 배열.
    - 판정 트리거의 개수인 6을 크기로 한다.

- public void SetVelocity()
    - 노트의 속력을 설정한다.

- public void SpawnNote(float time, int type)
    - StartCoroutine으로 SpawnNoteRoutine(time, type) 루틴 실행.
    - time : 노트가 생성되는 시간.
    - type : 노트가 생성되는 라인.
    
- IEnumerator SpawnNoteRoutine(float time, int type)
    - time - judgingTime 만큼  WaitforSecond를 통해 대기.
        - if(time - judgingTime > 0), 0초 대기.
    - 노트를 NoteSpawnTransforms[type].position에 생성하여 NoteSpawnTransforms[type].rotation 방향으로 등속 운동시킨다.
    - time : 노트 생성까지 대기하는 시간.
    - type : 노트가 생성되는 라인.

- public void UsingTypeForScenario()
    - 노트 판정이 필요없는 시나리오를 위한 자체 판정 요청을 수행함.
    - 포즈를 따라하기만 하는 시나리오 1, 2에서 사용됨.

- public int JudgeNote(int type)
    - 판정면에 도달한 노트 오브젝트를 참조해 판정을 한다.
    - 트리거 영역을 다루는 Manager에서 활성화 여부 배열을 받아와 판정에 이용한다.
    - type : 노트의 종류.
    - 노트 판정 결과를 반환한다.

- public void ResetAll()
    - 씬 시작 상태로 되돌리는 함수.

- RigidBody GetNote(int type)
    - 노트를 생성하고 해당 오브젝트의 Rigidbody 컴포넌트를 반환한다.
    - 이 때, 노트의 타입에 맞는 스프라이트를 노트에 적용한다.
    - type : 노트의 타입.

- private void OnTriggerEnter(Collider other)
    - 판정 콜라이더에 노트가 들어오면 해당 노트의 판정과 삭제를 수행함.

## DanceAreaManager

---

class

댄스 포즈 트리거 구역을 담당한다.

- public AreaSet area
    - 씬 진행에 사용되는 모든 구역들의 집합.

- public void Initialize()
    - 초기 설정을 위한 함수.

- public void ResetAll
    - 씬 시작 상태로 되돌리는 함수.

## AreaSet

---

class

씬 진행에 사용되는 트리거 구역을 관리하는 클래스이다.

AbstractDanceArea를 상속받는다.

- public DanceTriggerArea[] TriggerAreas
    - 각 판정 영역에 대한 정보를 담고 있는 구조체.
        | index | object |
        | --- | --- |
        | 0 | Left Down |
        | 1 | Left Middle |
        | 2 | Left Up |
        | 3 | Right Down |
        | 4 | Right Middle |
        | 5 | Right Up |

- public bool[] isTriggered
    - 각 판정 영역의 활성화 여부를 담고 있는 배열.
    - 판정 트리거의 개수인 6을 크기로 한다.

- public ReflectionProbe mirror
    - 컬링 마스크 적용을 위한 거울 오브젝트.

- public Material blue
- public Material origin
- public Material transparent
    - 각 판정 영역의 색을 변경하거나 감추는데 사용하는 머테리얼.
    - blue : 판정 영역이 강조될 때의 색.
    - origin : 판정 영역의 기본 색.
    - transparent : 판정 영역이 가려질 때의 투명 머테리얼.

- public void EnableGuide()
    - 각 판정 영역의 가이드를 활성화시킨다.

- public void DisableGuide()
    - 각 판정 영역의 가이드를 비활성화시킨다.

- public override void GetEntered(int type, GameObject areaObject)
    - 판정 영역이 사용자에 의해 인터랙션되었을 때 호출되는 함수.
    - 시나리오 1일 경우 영역의 머테리얼을 강조색으로 바꾼다.
    - 현재 판정 영역 활성화 여부를 업데이트한다.
    - type : 판정 영역의 고유번호.
    - areaObject : 판정 영역 오브젝트.
        | type | object |
        | --- | --- |
        | 11 | Left Down |
        | 12 | Left Middle |
        | 13 | Left Up |
        | 21 | Right Down |
        | 22 | Right Middle |
        | 23 | Right Up |

- public override void GetExited(int type, GameObject areaObject)
    - 사용자가 판정 영역을 벗어났을 때 호출되는 함수.
    - 시나리오 1일 경우 영역의 머테리얼을 원래 색으로 바꾼다.
    - type : 판정 영역의 고유번호.
    - areaObject : 판정 영역 오브젝트.

- public override void Initialize()
    - 판정 영역의 초기화를 위한 함수.

# Gallery Scene

---

갤러리 씬에서 사용되는 오브젝트 별 API이다.

각 클래스들의 기능을 호출하는 UI 오브젝트를 통해 인터렉션한다.

## GalleryManager

---

class
현재 갤러리에 불러올 이미지들을 결정한다.

- public GameObject galleryCanvas
    - 갤러리가 렌더링 될 캔버스 오브젝트이다.
- string filePath
    - 캡처, 녹화한 영상이 저장되는 위치이다.
- StartImageIndex
    - 갤러리에 띄울 이미지의 시작 인덱스이다.

- int _SetStartIndex(int value)
    - 시작 인덱스가 IndexError를 일으키지 않도록 방지하는 함수이다.
    - returns: IndexError을 일으키지 않는 범위 내의 값.

- void SetGallery()
    - GalleryImage.SetImage(string dirPath)를 통해 각 이미지를 갱신한다.

## GalleryImage
---

class
갤러리의 한 이미지.

- string path
    - 현재 영상, 이미지가 저장된 위치이다.
- SceneRecorder.RecordInfo info
    - 현재 영상, 이미지의 정보이다.
- Coroutine coroutine
    - 영상 재생의 중복 실행을 방지하기 위한 변수이다.

- void SetImage(string dirPath)
    - 해당 디렉토리 내의 info.json 을 읽어와 해당 영상의 정보를 갱신하고 RawImage 의 내용을 갱신한다.

- void Hover()
    - 이미지라면 해당 이미지에 대한 정보를 출력한다.
    - 영상이라면 해당 영상을 플레이한다.

- IEnumerator ShowVideo()
    - 연속된 이미지를 시간에 따라 다음 이미지로 변경하는 함수.

