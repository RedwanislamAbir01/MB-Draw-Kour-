using System;
using _Tools.Helpers;

namespace _Game
{
    public class PlayerState : Singleton<PlayerState>
    {
        public event EventHandler OnMovementComplete;
        
        public enum State
        {
            Default,
            Parkour
        }

        private State _currentState;
        private bool _isMoving;

        private void Start()
        {
            _currentState = State.Default;
        }

        public void SetState(State state) => _currentState = state;

        public bool IsDoingParkour() => _currentState == State.Parkour;
        public void EnableMoving() => _isMoving = true;
        public void DisableMoving()
        {
            _isMoving = false;
            OnMovementComplete?.Invoke(this, EventArgs.Empty);
        }

        public bool IsMoving() => _isMoving;
    }
}
