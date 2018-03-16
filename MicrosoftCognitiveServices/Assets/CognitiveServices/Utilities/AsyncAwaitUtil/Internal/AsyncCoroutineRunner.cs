using UnityEngine;

namespace UnityAsyncAwaitUtil
{
    public class AsyncCoroutineRunner : MonoBehaviour
    {
        private static AsyncCoroutineRunner instance;

        public static AsyncCoroutineRunner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AsyncCoroutineRunner>();
                }

                if (instance == null)
                {
                    instance = new GameObject("AsyncCoroutineRunner")
                        .AddComponent<AsyncCoroutineRunner>();
                    instance.gameObject.hideFlags = HideFlags.HideInHierarchy;
#if !UNITY_EDITOR
                    DontDestroyOnLoad(instance);
#endif
                }

                return instance;
            }
        }

        private void Update()
        {
            Debug.Assert(Instance != null);
        }
    }
}
