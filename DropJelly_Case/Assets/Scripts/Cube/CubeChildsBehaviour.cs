using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        GameManager.Instance.CheckCubeResize += ReconfigureCube;
    }

    private void OnDestroy()
    {
        GameManager.Instance.CheckCubeResize -= ReconfigureCube;
    }

    void SetColors()
    {
        if (topRightObj != null) topRight = topRightObj.material.color;
        if (topLeftObj != null) topLeft = topLeftObj.material.color;
        if (botRightObj != null) botRight = botRightObj.material.color;
        if (botLeftObj != null) botLeft = botLeftObj.material.color;
    }

    void DoResizeX(GameObject temp)
    {
        temp.transform.DOLocalMoveX(0, 0.1f);
        temp.transform.DOScaleX(1, 0.1f);
    }

    void DoResizeY(GameObject temp)
    {
        temp.transform.DOLocalMoveY(0, 0.1f);
        temp.transform.DOScaleY(1, 0.1f);
    }
    public void ReconfigureCube()
    {
        int activeChildCount = transform.childCount;
        if (activeChildCount == 3)
        {
            if (botRightObj == null)
            {
                botRightObj = botLeftObj;
                botRight = botLeftObj.material.color;
                DoResizeX(botLeftObj.gameObject);
            }
            if (botLeftObj == null)
            {
                botLeftObj = botRightObj;
                botLeft = botRightObj.material.color;
                DoResizeX(botRightObj.gameObject);
            }
            if (topRightObj == null)
            {
                topRightObj = topLeftObj;
                topRight= topLeftObj.material.color;
                DoResizeX(topLeftObj.gameObject);
            }
            if (topLeftObj == null)
            {
                topLeftObj = topRightObj;
                topLeft= topRightObj.material.color;
                DoResizeX(topRightObj.gameObject);
            }
        }

        if (activeChildCount == 2)
        {
            if (topRightObj == null && botRightObj == null)
            {
                topRightObj = topLeftObj;
                topRight = topLeft;
                DoResizeX(topLeftObj.gameObject);
                botRightObj = botLeftObj;
                botRight = botLeft;
                DoResizeX(botLeftObj.gameObject);
             //   Debug.Log("TopRightObj ve BotRightObj yok! TopLeftObj ve BotLeftObj uzayacak.");
            }
      
            if (topLeftObj == null && botLeftObj == null)
            {
                topLeftObj = topRightObj;
                topLeft = topRight;
                DoResizeX(topRightObj.gameObject);

                botLeftObj = botRightObj;
                botLeft = botRight;
                DoResizeX(botRightObj.gameObject);
             //   Debug.Log("TopLeftObj ve BotLeftObj yok! TopRightObj ve BotRightObj uzayacak.");
            }

            if (botLeftObj == null && botRightObj ==null)
            {
                botLeftObj = topLeftObj;
                botLeft = topLeft;
                DoResizeY(topLeftObj.gameObject);
                botRightObj = topRightObj;
                botRight = topRight;
                DoResizeY(topRightObj.gameObject);
            }

            if (topRightObj == null && topLeftObj == null)
            {
                topRightObj = botRightObj;
                topRight = botRight;
                DoResizeY(botRightObj.gameObject);
                topLeftObj = botLeftObj;
                topLeft = botLeft;
                DoResizeY(botLeftObj.gameObject);
            }
            if (topLeftObj == null)
            {
                if (topRightObj == botRightObj)
                {
                    topLeftObj = botLeftObj;
                    topLeft = botLeft;
                    DoResizeY(botLeftObj.gameObject);
                }
                else
                {
                    topLeftObj = topRightObj;
                    topLeft = topRight;
                    DoResizeX(topRightObj.gameObject);
                }
              
             //   Debug.Log("TopLeftObj yok! BotLeftObj uzayacak.");
            }
            
            if (botLeftObj == null)
            {
                if (topRightObj == botRightObj)
                {
                    botLeftObj = topLeftObj;
                    botLeft = topLeft;
                    DoResizeY(topLeftObj.gameObject);
                }
                else
                {
                    botLeftObj = botRightObj;
                    botLeft = botRight;
                    DoResizeX(botRightObj.gameObject);
                }
               
            //    Debug.Log("BotLeftObj yok! TopLeftObj uzayacak.");
            }
            
            if (topRightObj == null)
            {
                if (topLeftObj == botLeftObj)
                {
                    topRightObj = botRightObj;
                    topRight = botRight;
                    DoResizeY(botRightObj.gameObject);
                }
                else
                {
                    topRightObj = topLeftObj;
                    topRight = topLeft;
                    DoResizeX(topLeftObj.gameObject);
                }
              
          //      Debug.Log("TopRightObj yok! botRightObj uzayacak.");
            }

     
            if (botRightObj == null)
            {
                if (topLeftObj == botLeftObj)
                {
                    botRightObj = topRightObj;
                    botRight = topRight;
                    DoResizeY(topRightObj.gameObject);
                }
                else
                {
                    botRightObj = botLeftObj;
                    botRight = botLeft;
                    DoResizeX(botLeftObj.gameObject);
                }
              
           //     Debug.Log("BotRightObj yok! topRightObj uzayacak.");
            }
        }

        if (activeChildCount == 1)
        {
            Transform child = transform.GetChild(0).transform;
            Renderer child0 = transform.GetChild(0).GetComponent<Renderer>();
            Color child0Color = transform.GetChild(0).GetComponent<Renderer>().material.color;
            topRightObj = child0;
            topRight = child0Color;
            topLeftObj = child0;
            topLeft = child0Color;
            botRightObj = child0;
            botRight = child0Color;
            botLeftObj = child0;
            botLeft = child0Color;
            child.DOLocalMove(Vector3.zero, 0.1f);
            child.DOScale(Vector3.one, 0.1f);
        }
    }
}