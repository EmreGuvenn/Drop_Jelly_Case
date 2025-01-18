using System;
using System.Collections;
using System.Collections.Generic;
using _Reusable.GameData;
using _Reusable.Singleton;
using UnityEngine;

public class GameManager : NonPersistentSingleton<GameManager>
{
    public Action CheckAllGridForMatch;
    public Action CubeDestroy;
    public Action CheckCubeResize;
    public Action CheckGridList;
    public Action GridParentsControlUpNeighbour;
    public Action CountersChange;
   
    public int MoveCounter;
    public int GoalCounter;
    [SerializeField] private LevelCanvasManager _levelManager;
    private bool _control = false;

    void Start()
    {
        MoveCounter = (LevelManager.Instance.allLevels[SaveData.LevelCount].MoveCount);
        GoalCounter = (LevelManager.Instance.allLevels[SaveData.LevelCount].GoalCount);
    }

    public void LevelEnd(bool isWon) 
    {
        if (!_control)
        {
            _control = true;
            _levelManager.WinLose(isWon);
        }
       
    }
}