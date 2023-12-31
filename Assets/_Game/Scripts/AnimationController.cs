using DG.Tweening;
using System;
using System.Collections;

using UnityEngine;

namespace _Game
{
    [DefaultExecutionOrder(100)]
    public class AnimationController : MonoBehaviour
    {
        public static event EventHandler OnPlayerComboImpact;
        public static event Action OnLastHit;
        [Header(" Settings ")]
        private Animator animator;

        private Enemy currentEnemy;
        private string punchTriggerName;
        private GameObject parentObj;

        [SerializeField] private ParticleSystem _hitImpact;
        [SerializeField] private Transform _hitImpactSpawnPointRigthFoot;
        [SerializeField] private Transform _hitImpactSpawnPointLeftFoot;
        [SerializeField] private Transform _hitImpactSpawnPoinRightHand; 
        [SerializeField] private Transform _hitImpactSpawnPoinLeftHand;
        [SerializeField] private Transform _batHitPoint;
        private bool batCollected;

        void Start()
        {
            PlayerActivity.OnBatCollected += BatCollected;
            parentObj = transform.parent.gameObject;
            animator = GetComponent<Animator>();    
        
        }
        private void OnEnable()
        {
            
            PlayerActivity.OnHeliCopterJump += PlayJumpToHelicopterAnim;
            LineFollower.OnCharacterStartMoving +=  PlayRunAnimation;
            LineFollower.OnCharacterReachDestination += PlayIdleAnimation;
            LineFollower.OnCharacterReachedEnemy += PlayPunchAnimation;
            CamManager.OnCamReseting += ChildParentPosFix;
            LineFollower.OnCharacterStopped += PlayIdleAnimation;
        }
        private void OnDestroy()
        {
            PlayerActivity.OnBatCollected -= BatCollected;
            CamManager.OnCamReseting -= ChildParentPosFix;
            PlayerActivity.OnHeliCopterJump -= PlayJumpToHelicopterAnim;
            LineFollower.OnCharacterReachedEnemy -= PlayPunchAnimation;
            LineFollower.OnCharacterStartMoving -= PlayRunAnimation;
            LineFollower.OnCharacterReachDestination -= PlayIdleAnimation;
            LineFollower.OnCharacterStopped -= PlayIdleAnimation;
        }

        void PlayRunAnimation()
        {
            animator.SetTrigger("Run");
        }
        void PlayIdleAnimation()
        {
            animator.Play("Offensive Idle");
            animator.ResetTrigger("Run");
            animator.SetTrigger("Idle");

        }
        void PlayPunchAnimation(Enemy enemy)
        {
            animator.applyRootMotion = true;
            currentEnemy = enemy;

            if (batCollected)
            {
                // Generate a random number between 0 and 1 to select between "Bat1" and "Bat2"
                int randomBatIndex = UnityEngine.Random.Range(0, 2);
                punchTriggerName = (randomBatIndex == 0) ? "Bat1" : "Bat2";
            }
            else
            {
                // Generate a random number between 0 and 2 (inclusive)
                int randomPunchIndex = UnityEngine.Random.Range(0, 5);

                // Construct the trigger parameter name based on the random index
                punchTriggerName = (randomPunchIndex == 0) ? "Punch" : ("Punch" + randomPunchIndex);

            }
            currentEnemy.punchTriggerName = punchTriggerName;

            AnimPlayedCallBack();
            // Set the trigger to play the randomly selected punch animation
            animator.SetTrigger(punchTriggerName);


        }

        private void BatCollected()
        {
            batCollected = true;
        }
        public void AnimPlayedCallBack()
        {
            AnimationPunchEventCallback(currentEnemy, punchTriggerName);
            StartCoroutine(ResetRootMotionRoutine());
        }
   
        private IEnumerator ResetRootMotionRoutine()
        {  
            yield return new WaitForSeconds(1.5f);
        }
        public void AnimationPunchEventCallback(Enemy enemy, string punchTriggerName)
        {
            if (currentEnemy != null)
            {
                int damageAmount = 100;
                currentEnemy.TakeDamage(damageAmount, punchTriggerName);
            }
        }

        public void PlayJumpToHelicopterAnim()
        {
            animator.SetTrigger("JumpToHelicopter");
        }


        void ChildParentPosFix()
        {
            animator.applyRootMotion = false;

            transform.parent = null;

            Vector3 pos = new Vector3(transform.position.x, parentObj.transform.position.y, transform.position.z);

            parentObj.transform.position = pos;

            transform.parent = parentObj.transform;

            transform.DOLocalRotate(new Vector3(0, 0, 0), .1f).SetDelay(.2f);
        }

        public void PlayHitImpactRigthFoot()
        {
            OnPlayerComboImpact?.Invoke(this, EventArgs.Empty);
            Instantiate(_hitImpact, _hitImpactSpawnPointRigthFoot.position, Quaternion.identity);
        }
        public void PlayHitImpactLeftFoot()
        {
            OnPlayerComboImpact?.Invoke(this, EventArgs.Empty);
            Instantiate(_hitImpact, _hitImpactSpawnPointLeftFoot.position, Quaternion.identity);
        }
        public void PlayHitImpactRightHand()
        {
            OnPlayerComboImpact?.Invoke(this, EventArgs.Empty);
            Instantiate(_hitImpact, _hitImpactSpawnPoinRightHand.position, Quaternion.identity);
        }
        public void PlayHitImpactLeftHand()
        {
            OnPlayerComboImpact?.Invoke(this, EventArgs.Empty);
            Instantiate(_hitImpact, _hitImpactSpawnPoinLeftHand.position, Quaternion.identity);
        }
        public void PlayHitImpacBat()
        {
            OnPlayerComboImpact?.Invoke(this, EventArgs.Empty);
            Instantiate(_hitImpact, _batHitPoint.position, Quaternion.identity);
        }
        private void LastHit()
        {
            OnLastHit?.Invoke();
        }
    }
}
