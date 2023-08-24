using UnityEngine;

namespace _Game
{
    public class DestroyMe : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 1f;

        private void Start()
        {
            Destroy(gameObject, _lifeTime);
        }
    }
}
