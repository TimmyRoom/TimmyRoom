using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour
{

    public static ModelManager instance;
    public List<Material> colors;
    public List<Material> patterns;
    // Start is called before the first frame update

    private void Awake() {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void SetProfile(int colorId, int patternId)
    {
        this.GetComponent<SkinnedMeshRenderer>().materials = new Material[2] {colors[colorId-1], patterns[patternId]};
    }
}
