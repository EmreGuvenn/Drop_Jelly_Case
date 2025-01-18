using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        if (GameManager.Instance.GoalCounter>0)
        {
            GameManager.Instance.GoalCounter--;
        }
        GameManager.Instance.CountersChange?.Invoke();
        if (GameManager.Instance.GoalCounter<=0)
        {
            GameManager.Instance.LevelEnd(true);
        }
    }
}
