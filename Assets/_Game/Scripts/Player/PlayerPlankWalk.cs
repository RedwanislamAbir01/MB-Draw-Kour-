using UnityEngine;

namespace _Game
{
    public class PlayerPlankWalk : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip _runningAnimationClip;
        [SerializeField] private AnimationClip _walkingAnimationClip;
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        
        private void Start()
        {
            _animatorOverrideController = new AnimatorOverrideController
            {
                runtimeAnimatorController = _animator.runtimeAnimatorController
            };
        }

        public void StartPlankWalk()
        {
            _animatorOverrideController["Running"] = _walkingAnimationClip;
            _animator.runtimeAnimatorController = _animatorOverrideController;
        }

        public void StopPlankWalk()
        {
            _animatorOverrideController["Running"] = _runningAnimationClip;
            _animator.runtimeAnimatorController = _animatorOverrideController;
        }
    }
}