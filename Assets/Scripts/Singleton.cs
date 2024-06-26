﻿using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
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