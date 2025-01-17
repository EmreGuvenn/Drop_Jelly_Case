using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeChildsBehaviour : MonoBehaviour
{
    public Renderer topRightObj;
    public Renderer topLeftObj;
    public Renderer botRightObj;
    public Renderer botLeftObj;
    public Color topRight;
    public Color topLeft;
    public Color botRight;
    public Color botLeft;

    void Start()
    {
        SetColors();
    }

    void SetColors()
    {
        topRight = topRightObj.material.color;
        topLeft = topLeftObj.material.color;
        botRight = botRightObj.material.color;
        botLeft = botLeftObj.material.color;
    }
}