using System.Collections.Generic;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Tools.Runtime {
    internal static class TimerBootstrapper {
        private static PlayerLoopSystem _timerSystem;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        internal static void Initialize() {
            var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();

            if (!InsertTimerManager<Update>(ref currentPlayerLoop, 0)) {
                Debug.LogWarning("TimerManager not initialized.");
                return;
            }

            PlayerLoop.SetPlayerLoop(currentPlayerLoop);
#if DEBUG
            PrintPlayerLoop(currentPlayerLoop);
#endif

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            static void OnPlayModeStateChanged(PlayModeStateChange state) {
                if (state is not PlayModeStateChange.ExitingPlayMode)
                    return;
                var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
                RemoveTimerManager<Update>(ref currentPlayerLoop);
                PlayerLoop.SetPlayerLoop(currentPlayerLoop);
                TimerManager.Clear();
            }
#endif
        }
        
        static void RemoveTimerManager<T>(ref PlayerLoopSystem loop) {
            RemoveSystem<T>(ref loop, in _timerSystem);
        }
        
        static bool InsertTimerManager<T>(ref PlayerLoopSystem loop, int index) {
            _timerSystem = new PlayerLoopSystem {
                type = typeof(TimerManager),
                updateDelegate = TimerManager.UpdateTimers,
                subSystemList = null
            };
            return InsertSystem<T>(ref loop, _timerSystem, index);
        }

        static void RemoveSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem target) {
            if (loop.subSystemList == null)
                return;
            var playerLoopSystemList = new List<PlayerLoopSystem>(loop.subSystemList);
            for (var i = 0; i < playerLoopSystemList.Count; i++) {
                if (playerLoopSystemList[i].type != target.type ||
                    playerLoopSystemList[i].updateDelegate != target.updateDelegate)
                    continue;
                playerLoopSystemList.RemoveAt(i);
                loop.subSystemList = playerLoopSystemList.ToArray();
                return;
            }

            HandleSubSystemLoopForRemove<T>(ref loop, target);
        }

        static void HandleSubSystemLoopForRemove<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem target) {
            if (loop.subSystemList == null)
                return;

            for (var i = 0; i < loop.subSystemList.Length; i++) {
                RemoveSystem<T>(ref loop.subSystemList[i], in target);
            }
        }

        private static bool InsertSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem targetSystem, int index) {
            if (loop.type != typeof(T))
                return HandleSubSystemLoop<T>(ref loop, in targetSystem, index);

            var playerLoopSystemList = new List<PlayerLoopSystem>();
            if (loop.subSystemList != null)
                playerLoopSystemList.AddRange(loop.subSystemList);
            playerLoopSystemList.Insert(index, targetSystem);
            loop.subSystemList = playerLoopSystemList.ToArray();

            return true;
        }

        private static bool HandleSubSystemLoop<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem targetSystem, int index) {
            if (loop.subSystemList == null)
                return false;

            for (var i = 0; i < loop.subSystemList.Length; i++) {
                if (!InsertSystem<T>(ref loop.subSystemList[i], in targetSystem, index))
                    continue;
                return true;
            }

            return false;
        }

#if DEBUG
        private static void PrintPlayerLoop(PlayerLoopSystem playerLoop) {
            var sb = new StringBuilder();
            sb.AppendLine("Unity PlayerLoop");
            foreach (var subSystem in playerLoop.subSystemList) {
                PrintSubSystems(subSystem, sb, 0);
            }

            Debug.Log(sb.ToString());
        }

        private static void PrintSubSystems(PlayerLoopSystem loopSystem, StringBuilder sb, int level) {
            sb.Append(' ', level * 2).AppendLine(loopSystem.type.ToString());
            if (loopSystem.subSystemList == null || loopSystem.subSystemList.Length == 0)
                return;
            foreach (var subSystem in loopSystem.subSystemList) {
                PrintSubSystems(subSystem, sb, level + 1);
            }
        }
#endif
    }
}
