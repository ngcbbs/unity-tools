using System.Collections.Generic;
using UnityTools.Extensions;

namespace UnityTools.Timer {
    public static class TimerManager {
        private static readonly List<Timer> Timers = new();
        private static readonly List<Timer> Sweep = new();
        public static void RegisterTimer(Timer timer) => Timers.Add(timer);
        public static void UnregisterTimer(Timer timer) => Timers.Remove(timer);

        public static void UpdateTimers() {
            if (Timers.Count == 0)
                return;
            Sweep.RefreshWith(Timers);
            foreach (var timer in Sweep)
                timer.Tick();
        }

        public static void Clear() {
            Sweep.RefreshWith(Timers);
            foreach (var timer in Sweep)
                timer.Dispose();
            Timers.Clear();
            Sweep.Clear();
        }
    }
}