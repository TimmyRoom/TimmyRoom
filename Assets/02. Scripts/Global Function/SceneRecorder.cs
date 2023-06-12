using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 씬 내부를 촬영하고 촬영된 영상을 저장하는 클래스.
/// </summary>
public class SceneRecorder : MonoBehaviour
{
    public static SceneRecorder instance;
    public static string sceneName = "Nanta";
    public static int userId;
    public GameObject selfCameraObject;

    bool isRecording = false;
    float recordTime = 0f;
    float maxRecordTime = 10f;
    Coroutine coroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            Destroy(this.gameObject);
        }
        //DontDestroyOnLoad(this);
        filePath = Application.persistentDataPath;
    }

    string filePath;

    /// <summary>
    /// filePath를 변경한다.
    /// </summary>
    /// <param name="path">변경할 filePath.</param>
    public void SetPath(string path)
    {
        //TODO : 메서드 구현.
        filePath = path;
    }

    /// <summary>
    /// 사용자가 바라보는 화면을 캡처하는 함수.
    /// </summary>
    /// <returns>저장 성공여부.</returns>
    public bool Capture()
    {
        //TODO : Capture Main Camera and save it to filePath.
        try
        {
            Camera mainCamera = Camera.main;
            Camera selfCamera = selfCameraObject.GetComponent<Camera>();
            
            selfCamera.enabled = false;
            bool result = _Capture(mainCamera);
            return result;
        }
        catch (UnityException e)
        {
            Debug.LogWarning("Failed to capture the screen.",this);
            Debug.LogWarning(e.Message);
            return false;
        }
    }

    public void TestCapture()
    {
        Capture();
    }

    /// <summary>
    /// 사용자를 바라보는 바깥의 가상 카메라를 캡처하는 함수.
    /// </summary>
    /// <returns>저장 성공여부.</returns>
    public bool CaptureSelf()
    {
        //TODO : Capture Self Camera and save it to filePath.
        try
        {
            Camera selfCamera = selfCameraObject.GetComponent<Camera>();
            Camera mainCamera = Camera.main;
            
            bool result = _Capture(selfCamera);
            mainCamera.enabled = true;
            selfCamera.enabled = false;
            return result;
        }
        catch
        {
            Debug.LogWarning("Failed to capture the screen.",this);
            return false;
        }
    }

    public void TestCaptureSelf()
    {
        CaptureSelf();
    }

    bool _Capture(Camera camera) 
    {
        string dirName = System.DateTime.Now.ToString("yyyy-MM-dd-mm-ss");
        string curFilePath = filePath + "/" + dirName;
        if(!System.IO.Directory.Exists(curFilePath))
        {
            System.IO.Directory.CreateDirectory(curFilePath);
        }
        else
        {
            Debug.LogWarning("Too fast to capture the screen.");
            throw new UnityException("Directory already exists.");
        }

        // Create a RenderTexture object
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        // Read screen contents into the texture
        camera.targetTexture = rt;
        camera.Render();
        RenderTexture.active = rt;

        // Create a new Texture2D and read the RenderTexture image into it
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        // Encode texture into PNG
        byte[] bytes = screenShot.EncodeToPNG();

        // For testing purposes, also write to a file in the project folder
        string photoName = "photo" + "000" +".png";
        System.IO.File.WriteAllBytes(curFilePath + "/" + photoName, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", curFilePath));

        string infoName = "info.json";
        string info = MakeRecordInfo(RecordType.Image);
        System.IO.File.WriteAllText(curFilePath + "/" + infoName, info);

        Debug.Log("Main Camera", Camera.main);

        return true;
    }
    
    public bool Record()
    {
        if(isRecording)
        {
            Debug.LogWarning("Already recording.",this);
            return false;
        }
        Camera camera = Camera.main;
        bool result = _Record(camera);
        return result;
    }

    public void TestRecord() { Record();}

    public bool RecordSelf()
    {
        if(isRecording)
        {
            Debug.LogWarning("Already recording.",this);
            return false;
        }
        Camera camera = selfCameraObject.GetComponent<Camera>();
        bool result = _Record(camera);
        return result;
    }

    public void TestRecordSelf() { RecordSelf();}

    bool _Record(Camera camera)
    {
        //TODO : Record Camera and save it to filePath.
        RecordSceneType sceneType = RecordSceneType.Nanta;
        if(sceneName == "Nanta" || sceneName == "nanta")
        {
            sceneType = RecordSceneType.Nanta;
        }
        else if (sceneName == "Dance" || sceneName == "dance")
        {
            sceneType = RecordSceneType.Dance;
        }

        coroutine = StartCoroutine(_RecordCoroutine(camera, sceneType));
        return true;
    }

    IEnumerator _RecordCoroutine(Camera camera, RecordSceneType sceneType)
    {
        Camera mainCamera = Camera.main;
        Camera selfCamera = selfCameraObject.GetComponent<Camera>();


        //TODO : Record Camera and save it to filePath.
        //Make Directory
        string dirName = System.DateTime.Now.ToString("yyyy-MM-dd-mm-ss");
        string curFilePath = filePath + "/" + dirName;
        if(!System.IO.Directory.Exists(curFilePath))
        {
            System.IO.Directory.CreateDirectory(curFilePath);
        }
        else
        {
            Debug.LogWarning("Too fast to record the screen.");
            throw new UnityException("Directory already exists.");
        }
        for(int i = 0; i < 240; i++)
        {
            // Create a RenderTexture object
            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
            // Read screen contents into the texture
            camera.targetTexture = rt;
            camera.Render();
            RenderTexture.active = rt;

            // Create a new Texture2D and read the RenderTexture image into it
            Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

            camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            // Encode texture into PNG
            byte[] bytes = screenShot.EncodeToPNG();

            // For testing purposes, also write to a file in the project folder
            string photoName = "photo" + i.ToString("D3") + ".png";
            System.IO.File.WriteAllBytes(curFilePath + "/" + photoName, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", curFilePath));

            if(camera == selfCamera)
            {
                mainCamera.enabled = true;
                selfCamera.enabled = false;
            }

            yield return new WaitForSeconds(1.0f/24.0f);
        }

        string infoName = "info.json";
        string info = MakeRecordInfo(RecordType.Video);
        System.IO.File.WriteAllText(curFilePath + "/" + infoName, info);

        Debug.Log("Recorded");
    }

    string MakeRecordInfo(RecordType type)
    {
        RecordInfo info = new RecordInfo();
        info.type = type;

        if(sceneName == "Nanta" || sceneName == "nanta")
        {
            info.sceneType = RecordSceneType.Nanta;
        }
        else if(sceneName == "Dance" || sceneName == "dance")
        {
            info.sceneType = RecordSceneType.Dance;
        }

        string json = JsonUtility.ToJson(info);
        return json;
    }

    public void StopRecord()
    {
        if(!isRecording)
        {
            Debug.LogWarning("Not recording.",this);
            return;
        }
        StopCoroutine(coroutine);
    }
}
