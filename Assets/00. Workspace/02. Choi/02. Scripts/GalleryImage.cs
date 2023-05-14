using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryImage : MonoBehaviour
{
    public string path;
    public string dateTime;
    public RecordInfo info;
    Coroutine coroutine;
    private void Awake(){

    }

    public void SetImage(string dirPath)
    {
        this.path = dirPath;
        RecordInfo info = JsonUtility.FromJson<RecordInfo>(System.IO.File.ReadAllText(dirPath + "/info.json"));
        this.info = info;
        this.dateTime = dirPath.Substring(dirPath.LastIndexOf('/') + 1);
        Texture2D tex = new Texture2D(1920,1080,TextureFormat.RGB24,false);
        tex.LoadImage(System.IO.File.ReadAllBytes(dirPath + "/photo000.png"));
        this.GetComponent<RawImage>().texture = tex;
    }

    public void Hover()
    {
        if(this.info.type == RecordType.Image)
        {
            Debug.Log("이미지입니다.");
        }
        if(this.info.type == RecordType.Video)
        {
            if(coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(ShowVideo());
        }
    }

    IEnumerator ShowVideo()
    {
        List<Texture2D> images = new List<Texture2D>();
        // Load Images on filePath
        string[] files = System.IO.Directory.GetFiles(path);

        foreach (string file in files)
        {
            if(file.Contains(".png"))
            {
                Texture2D tex = new Texture2D(1920,1080,TextureFormat.RGB24,false);
                tex.LoadImage(System.IO.File.ReadAllBytes(file));
                images.Add(tex);
            }
        }
        foreach(var img in images)
        {
            this.GetComponent<RawImage>().texture = img;
            yield return new WaitForSeconds(1.0f / 24.0f);
        }
        Debug.Log("영상 재생 종료");
    }


}
