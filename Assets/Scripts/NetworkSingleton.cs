using UnityEngine;
using Unity.Netcode;

namespace Assets.Scripts
{
    public abstract class NetworkSingleton<T> : NetworkBehaviour where T : NetworkBehaviour
    {
        public static T Instance;

        protected virtual void Awake()
        {
            CreateSingleton();
        }

        protected void CreateSingleton()
        {
            if (transform.parent != null) transform.SetParent(null, false);

            if (Instance == null)
            {
                Instance = GetComponent<T>();
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}