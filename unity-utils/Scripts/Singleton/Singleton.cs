using UnityEngine;
using UnityTools.Extensions;

namespace UnityTools {
    public class Singleton<T> : MonoBehaviour where T : Component {
        protected static T instance;

        public static bool HasInstance => instance is not null;
        public static T TryGetInstance() => HasInstance ? instance : null;

        public static T Instance {
            get {
                if (instance is null) {
                    instance = FindAnyObjectByType<T>();

                    if (instance is null) {
                        var go = new GameObject($"[{typeof(T).Name}]");
                        instance = go.AddComponent<T>();
                    }
                }

                if (!instance.IsDontDestroyOnLoad())
                    DontDestroyOnLoad(instance);

                return instance;
            }
        }

        /// <summary>
        /// Awake에서 사용 할 때는 반드시 base.Awake()를 오버라이드 상태로 호출.
        /// </summary>
        protected virtual void Awake() {
            Initialize();
        }

        protected void Initialize() {
            if (!Application.isPlaying)
                return;
            instance = this as T;
        }
    }
}