
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
            GameManager.Instance.OnEolTrigger += OnChopperReachedDestination;
            collider = GetComponent<Collider>();
             rb = GetComponent<Rigidbody>();
        }
 


        private void OnDisable()
        {
            // Unsubscribe from the ChopperReachedDestination event
            GameManager.Instance.OnEolTrigger -= OnChopperReachedDestination;
        }
        private void OnChopperReachedDestination()
        {
            PlayerState.Instance.SetState(PlayerState.State.Parkour);
            ChopperActivity chopperActivity = FindAnyObjectByType<ChopperActivity>();
            print(chopperActivity);
            OnHeliCopterJump?.Invoke();
            transform.DOMove(new Vector3(chopperActivity.StandPoint.transform.position.x , chopperActivity.StandPoint.transform.position.y, chopperActivity.StandPoint.transform.position.z) , 2f).OnComplete
            (
                () =>
                {
                   // transform.DOLocalMoveY(-1.5f, 01f);
                    rb.isKinematic = true;
                    transform.parent = chopperActivity.transform;
                    GameManager.Instance.LevelComplete();
                }
            );
        }
    }
}
