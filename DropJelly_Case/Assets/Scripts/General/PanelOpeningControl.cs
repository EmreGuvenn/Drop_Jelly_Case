using System.Collections;
using System.Collections.Generic;
using _Reusable.GameData;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelOpeningControl : MonoBehaviour
{
    [SerializeField] private Image BGAlphaOpen;
    [SerializeField] private RectTransform TopPanel;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private bool condition;
    void OnEnable()
    {
        AlphaOpen();
        TopPanelDown();
        SetText();
    }

    void AlphaOpen()
    {
        LeanTween.value(0, 1, 0.3f).setOnUpdate((float value) =>
        {
            BGAlphaOpen.color = new Color(BGAlphaOpen.color.r, BGAlphaOpen.color.g, BGAlphaOpen.color.b, value);
        });
    }

    void TopPanelDown()
    {
        TopPanel.DOAnchorPosY(220f, 0.3f).SetEase(Ease.OutBounce);
    }

    void SetText()
    {
        if (condition)
        {
            LevelText.text = "Level " + SaveData.LevelCountEndless.ToString() + " Completed";
        }
        else
        {
            LevelText.text = "Level " + SaveData.LevelCountEndless.ToString() + " Failed";
        }
       
    }
}
