
using UnityEngine;
using DG.Tweening;
using System;
using _Game.Managers;

namespace _Game
{
    public class PlayerActivity : MonoBehaviour
    {
        public static event Action OnHeliCopterJump;

        Rigidbody rb;

        Collider collider;
        private void Start()
        {
             collider = GetComponent<Collider>();
             rb = GetComponent<Rigidbody>();
        }
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
            PlayerState.Instance.SetState(PlayerState.State.Parkour);
            ChopperActivity chopperActivity = FindAnyObjectByType<ChopperActivity>();


           
            OnHeliCopterJump?.Invoke();
            transform.DOLocalJump(chopperActivity.StandPoint.transform.position  , 2, 1, .6f).OnComplete
            (
                () =>
                {
                    transform.DOLocalMoveY(-1.5f, 01f);
                    rb.isKinematic = true;
                    transform.parent = chopperActivity.transform;
                    GameManager.Instance.LevelComplete();
                }
            );
        }
    }
}
