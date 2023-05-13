using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryManager : AbstractSceneManager
{
    public GameObject galleryCanvas;
    GameObject gridObject;
    string filePath;
    int startImageIndex;
    public int StartImageIndex { get => startImageIndex; set => startImageIndex = _SetStartIndex(value); }

    // Start is called before the first frame update
    void Start()
    {
        gridObject = galleryCanvas.transform.Find("Grid").gameObject;
        filePath = Application.persistentDataPath;
        List<Texture2D> imageList = LoadImages();
        SetGallery(imageList);
    }

    /// <summary> <summary>
    /// filePath에서 이미지를 불러온다.
    /// </summary>
    /// <returns> 저장소에 들어있는 것. </returns>
    private List<Texture2D> LoadImages()
    {
        List<Texture2D> images = new List<Texture2D>();
        // Load Images on filePath
        string[] files = System.IO.Directory.GetFiles(filePath);
        foreach (string file in files)
        {
            Texture2D tex = new Texture2D(1920,1080,TextureFormat.RGB24,false);
            tex.LoadImage(System.IO.File.ReadAllBytes(file));
            images.Add(tex);
        }
        return images;
    }

    private void SetGallery(List<Texture2D> images)
    {
        int childCount = gridObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = gridObject.transform.GetChild(i);
            child.GetComponent<RawImage>().texture = images[startImageIndex + i];
        }
    }

    int _SetStartIndex(int value)
    {
        int childCount = gridObject.transform.childCount;
        int imageCount = System.IO.Directory.GetFiles(filePath).Length;
        if(value > imageCount - childCount)
        {
            value = imageCount - childCount;
        }
        else if(value < 0)
        {
            value = 0;
        }
        return value;
    }

    public void SetStartIndex(int value)
    {
        StartImageIndex = value;
        List<Texture2D> imageList = LoadImages();
        SetGallery(imageList);
    }

    public void NextPage()
    {
        StartImageIndex += 1;
        List<Texture2D> imageList = LoadImages();
        SetGallery(imageList);
    }

    public void LastPage()
    {
        StartImageIndex -= 1;
        List<Texture2D> imageList = LoadImages();
        SetGallery(imageList);
    }
}
