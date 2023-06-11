using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour
{

    public static ModelManager instance;
    public static int currentColorId = 0;
    public static int currentPatternId = 0;

    public List<Material> colors;
    public List<Material> patterns;
    
    
    // Start is called before the first frame update

    private void Awake() {
        instance = this;
    }


    void Start()
    {
        SetProfile(currentColorId, currentPatternId);
    }

    public void SetProfile(int colorId, int patternId)
    {
        this.GetComponent<SkinnedMeshRenderer>().materials = new Material[2] {colors[colorId], patterns[patternId]};
        currentColorId = colorId;
        currentPatternId = patternId;
    }
}
