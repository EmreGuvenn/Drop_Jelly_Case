using System;
using System.Collections;
using System.Collections.Generic;
using _Reusable.Singleton;
using UnityEngine;

public class GameManager : NonPersistentSingleton<GameManager>
{
    public Action CheckAllGridForMatch;
    public Action CubeDestroy;
    public Action CheckCubeResize;
    public Action CheckGridList;
    public Action GridParentsControlUpNeighbour;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
