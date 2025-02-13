using System;
using UnityEngine;

namespace Tools.Runtime.Timers {
    public class FrequencyTimer : Timer {
        public int TicksPerSecond { get; private set; }
        public Action OnTick = delegate { };
        private float _timeThreshold;

        public FrequencyTimer(int ticksPerSecond) : base(0f) {
            CalculateTimeThreshold(ticksPerSecond);
        }

        public override void Tick() {
            if (IsRunning && CurrentTime >= _timeThreshold) {
                CurrentTime -= _timeThreshold;
                OnTick.Invoke();
            }

            if (IsRunning && CurrentTime < _timeThreshold)
                CurrentTime += Time.deltaTime;
        }

        public override bool IsFinished => !IsRunning;

        public override void Reset() {
            CurrentTime = 0;
        }

        public void Reset(int newTicksPerSecond) {
            CalculateTimeThreshold(newTicksPerSecond);
            Reset();
        }

        void CalculateTimeThreshold(int ticksPerSecond) {
            TicksPerSecond = ticksPerSecond;
            _timeThreshold = 1f / TicksPerSecond;
        }
    }
}