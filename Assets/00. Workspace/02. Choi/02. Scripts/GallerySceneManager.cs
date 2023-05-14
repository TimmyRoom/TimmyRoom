using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GallerySceneManager : AbstractSceneManager
{
    public GameObject galleryCanvas;
    GameObject gridObject;
    string filePath;
    int startImageIndex = 0;
    public int StartImageIndex { get => startImageIndex; set => startImageIndex = _SetStartIndex(value); }

    // Start is called before the first frame update
    void Start()
    {
        gridObject = galleryCanvas.transform.Find("Grid").gameObject;
        filePath = Application.persistentDataPath;
        SetGallery();
    }

    public void SetGallery()
    {
        string[] dirs = System.IO.Directory.GetDirectories(filePath);
        int childCount = gridObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            if(startImageIndex + i == dirs.Length)
            {
                break;
            }
            Transform child = gridObject.transform.GetChild(i);
            child.GetComponent<GalleryImage>().SetImage(dirs[startImageIndex + i]);
        }
    }

    int _SetStartIndex(int value)
    {
        int childCount = gridObject.transform.childCount;
        int imageCount = System.IO.Directory.GetDirectories(filePath).Length;
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
        SetGallery();
    }

    public void NextPage()
    {
        StartImageIndex += 1;
        SetGallery();
    }

    public void LastPage()
    {
        StartImageIndex -= 1;
        SetGallery();
    }

    public override void MoveScene(int sceneIndex)
    {
        SceneMover.instance.MoveScene(sceneIndex);
    }

    public override void MoveScene(string sceneName)
    {
        SceneMover.instance.MoveScene(sceneName);
    }
}
