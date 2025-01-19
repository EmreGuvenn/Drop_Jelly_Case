using UnityEngine;

namespace _Reusable.ScriptableObjects.Level
{
    [CreateAssetMenu(fileName = "Level Asset", menuName = "Emre/Sample Level", order = 1)]
    public class LevelType1: ScriptableObject
    {
        public GridLevel _gridLevel;
        public int levelIndex;
        public int MoveCount;
        public int GoalCount;
        
    }
}