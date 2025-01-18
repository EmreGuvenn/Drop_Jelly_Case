using System;
using System.Collections;
using System.Collections.Generic;
using _Reusable.GameData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCanvasManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MoveCount;
    [SerializeField] private TextMeshProUGUI GoalCount;
    [SerializeField] private GameObject VictoryPanel;
    [SerializeField] private GameObject DefeatPanel;

    void Start()
    {
        SetText();
        GameManager.Instance.CountersChange += TextRefresh;
    }

    private void OnDestroy()
    {
        GameManager.Instance.CountersChange -= TextRefresh;
    }

    void SetText()
    {
        MoveCount.text = (LevelManager.Instance.allLevels[SaveData.LevelCount].MoveCount).ToString();
        GoalCount.text = (LevelManager.Instance.allLevels[SaveData.LevelCount].GoalCount).ToString();
    }

    void TextRefresh()
    {
        MoveCount.text = (GameManager.Instance.MoveCounter).ToString();
        GoalCount.text = (GameManager.Instance.GoalCounter).ToString();
    }

    public void WinLose(bool condition) => StartCoroutine(WaitMethod(condition));
        IEnumerator WaitMethod(bool condition)
    {
        yield return new WaitForSeconds(1.5f);
        (condition ? VictoryPanel : DefeatPanel).SetActive(true);
    }

    public void LoadNextLevel()
    {
        SaveData.LevelCount++;
        SaveData.LevelCountEndless++;
        if (SaveData.LevelCount >= LevelManager.Instance.allLevels.Length)
        {
            SaveData.LevelCount = 0;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}