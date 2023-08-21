using UnityEngine;

namespace _Game
{
    public class Box : MonoBehaviour, IJumpable
    {
        [SerializeField] private float _jumpHeightOffset;
        [SerializeField] private float _totalJumpDuration = 1f;

        public float JumpHeightOffset => _jumpHeightOffset;
        public float TotalJumpDuration => _totalJumpDuration;
    }
}
