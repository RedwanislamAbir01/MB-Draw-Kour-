
using UnityEngine;
using DG.Tweening;
using System;
using _Game.Managers;

namespace _Game
{
    public class PlayerActivity : MonoBehaviour
    {
        public static event Action OnHeliCopterJump;

        private void OnEnable()
        {
            // Subscribe to the ChopperReachedDestination event
            ChopperActivity.OnChopperReachedDestination += OnChopperReachedDestination;
        }

        private void OnDisable()
        {
            // Unsubscribe from the ChopperReachedDestination event
            ChopperActivity.OnChopperReachedDestination -= OnChopperReachedDestination;
        }
        private void OnChopperReachedDestination()
        {
            OnHeliCopterJump?.Invoke();
            transform.DOLocalJump(FindAnyObjectByType<ChopperActivity>().transform.position, 2, 1, .6f).OnComplete
            (
                () =>
                {
                    GameManager.Instance.LevelComplete();
                }
            );
        }
    }
}