using Unity.Netcode;

namespace Assets.Scripts
{
    public abstract class NetworkSingleton<T> : NetworkBehaviour where T : NetworkBehaviour
    {
        public static T Instance;

        public virtual void Start()
        {
            CreateSingleton();
        }

        protected void CreateSingleton()
        {
            if (transform.parent != null) transform.SetParent(null, false);

            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

        }

        public void DestroyInstance()
        {
            //Instance = null;
            Destroy(gameObject);
        }
    }
}