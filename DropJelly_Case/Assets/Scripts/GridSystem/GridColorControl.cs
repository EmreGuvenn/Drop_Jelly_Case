using System;
using UnityEngine;
using UnityEngine.UI;

public class GridColorControl : MonoBehaviour
{
    public Color targetColor;  // Gridin değişeceği hedef renk
    private Image gridImage;   // Gridin görüntüsü
    public bool isRayHit = false;
    private float timer;
   
    void Start()
    {
        gridImage = GetComponent<Image>();
    }

    private void Update()
    {
       
        if (isRayHit)
        {
            isRayHit = false;
        }
        else
        {
            timer += Time.deltaTime;
            if (timer>0.1f)
            {
                ResetColor();
            }
        }
    }

    public void ChangeColor()
    {
        if (gridImage != null)
        {
            gridImage.color = targetColor;
        }
    }

    public void ResetColor() => gridImage.color = Color.white;
}