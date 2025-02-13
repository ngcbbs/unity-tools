using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.Extensions {
    public static class LoopExtensions {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            var sweep = new List<T>(source);
            foreach (var it in sweep)
                action?.Invoke(it);
            sweep.Clear();
        }
    }

    public static class GameObjectExtensions {
        public static T EnsureComponent<T>(this GameObject obj) where T : Component {
            var comp = obj.GetComponent<T>();
            return comp ?? obj.AddComponent<T>();
        }

        public static Component EnsureComponent(this GameObject obj, Type type) {
            var comp = obj.GetComponent(type);
            return comp ?? obj.AddComponent(type);
        }
        
        public static bool IsDontDestroyOnLoad(this GameObject go) {
            return (go.hideFlags & HideFlags.DontSave) == HideFlags.DontSave;
        }

        public static bool IsDontDestroyOnLoad<T>(this T target) where T : Component {
            return (target.gameObject.hideFlags & HideFlags.DontSave) == HideFlags.DontSave;
        }
    }
}