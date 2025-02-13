using System;
using UnityEngine;

namespace UnityTools.Timer {
    public abstract class Timer : IDisposable {
        public float CurrentTime { get; protected set; }
        public bool IsRunning { get; protected set; }

        protected float initialTime;

        public float Progress => Mathf.Clamp01(CurrentTime / initialTime);

        public Action OnStart = delegate { };
        public Action OnEnd = delegate { };

        protected Timer(float time) {
            initialTime = time;
        }

        public void Start() {
            CurrentTime = initialTime;
            if (IsRunning)
                return;
            IsRunning = true;
            TimerManager.RegisterTimer(this);
            OnStart.Invoke();
        }

        public void Stop() {
            if (!IsRunning)
                return;
            IsRunning = false;
            TimerManager.UnregisterTimer(this);
            OnEnd.Invoke();
        }

        public abstract void Tick();
        public abstract bool IsFinished { get; }

        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;

        public virtual void Reset() => CurrentTime = initialTime;

        public virtual void Reset(float time) {
            initialTime = time;
            Reset();
        }

        #region IDisposable

        private bool _disposed = false;

        ~Timer() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (_disposed)
                return;
            
            if (disposing)
                TimerManager.UnregisterTimer(this);

            _disposed = true;
        }

        #endregion
    }
}