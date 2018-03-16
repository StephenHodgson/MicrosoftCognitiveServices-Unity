using System.Threading;
using UnityEngine;

namespace UnityAsyncAwaitUtil
{
    public static class SyncContextUtil
    {
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#endif
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            UnitySynchronizationContext = SynchronizationContext.Current;
            UnityThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public static int UnityThreadId
        {
            get; private set;
        }

        public static SynchronizationContext UnitySynchronizationContext
        {
            get; private set;
        }
    }
}

