using UnityEngine;

namespace UnityTools {
    public class StopwatchTimer : Timer {
        public StopwatchTimer() : base(0f) { }

        public override void Tick() {
            if (IsRunning)
                CurrentTime += Time.deltaTime;
        }

        public override bool IsFinished => false;
    }
}