using UnityEngine;

namespace _Reusable.Singleton
{
    public class NonPersistentSingleton<T> : MonoBehaviour where T : Component

    {
        public static T Instance;
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = GetComponent<T>();
            }else if (Instance != GetComponent<T>())
            {
                Destroy(gameObject);
            }
        }
    }
}
