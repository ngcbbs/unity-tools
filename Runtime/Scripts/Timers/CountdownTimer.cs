using UnityEngine;

namespace Tools.Runtime.Timers {
    public class CountdownTimer : Timer {
        public CountdownTimer(float time) : base(time) { }

        public override void Tick() {
            if (IsRunning && CurrentTime > 0)
                CurrentTime -= Time.deltaTime;
            if (IsRunning && CurrentTime <= 0)
                Stop();
        }

        public override bool IsFinished => CurrentTime <= 0;
    }
}