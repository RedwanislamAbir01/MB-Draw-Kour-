using DG.Tweening;
using UnityEngine;

namespace _Game
{
    public class Cash : MonoBehaviour, ICollectible
    {
        [SerializeField] private string _type = "Cash";
        [SerializeField] private int _value = 100;
        [SerializeField] private float _yOffset = 1;
        [SerializeField] private float _floatingDuration = 1f;
        [SerializeField] private Transform _visual;
        [SerializeField] private Vector3 _rotationAxis = Vector3.forward;
        [SerializeField] [Range(1f, 5000f)] private float _rotationSpeed = 500f;
        [SerializeField] private ParticleSystem _sparkleEffect;

        private Collider _collider;
        private bool _canRotate;
        
        public string Type => _type;
        public int Value => _value;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            transform.DOMoveY(transform.position.y + _yOffset, _floatingDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            _canRotate = true;
        }

        private void Update()
        {
            // if(!_canRotate) return;
            _visual.Rotate(_rotationAxis * (_rotationSpeed * Time.deltaTime));
        }

        public void Collect(Transform collector)
        {
            _collider.enabled = false;
            _sparkleEffect.Stop();
            _canRotate = false;
            transform.DOKill();

            const float duration = 0.5f;
            
            const float halfDuration = duration * 0.5f;
            transform.DOScale(Vector3.zero, halfDuration).SetDelay(halfDuration);
            
            const float jumpPower = 4f;
            transform.DOJump(collector.transform.position, jumpPower, 1, duration).OnComplete(() =>
            {
                Destroy(gameObject); 
            });
        }
    }
}