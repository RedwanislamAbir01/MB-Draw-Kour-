using _Tools.Helpers;

namespace _Game
{
    public class PlayerState : Singleton<PlayerState>
    {
        public enum State
        {
            Default,
            Parkour
        }

        private State _currentState;

        private void Start()
        {
            _currentState = State.Default;
        }

        public void SetState(State state) => _currentState = state;

        public bool IsDoingParkour() => _currentState == State.Parkour;
    }
}
