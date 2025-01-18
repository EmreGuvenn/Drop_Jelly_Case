using UnityEngine;

namespace _Reusable.GameData
{
    public static class SaveData
    {
        public static int LevelCount
        {
            get => PlayerPrefs.GetInt("LevelCount", 0);
            set => PlayerPrefs.SetInt("LevelCount", value);
        }
        public static int LevelCountEndless
        {
            get => PlayerPrefs.GetInt("LevelCountEndless", 1);
            set => PlayerPrefs.SetInt("LevelCountEndless", value);
        }
    }
}