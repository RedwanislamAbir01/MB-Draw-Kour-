using System;
using DG.Tweening;

using UnityEngine;
using Unity.VisualScripting;

namespace _Game
{
    public class EnemyGun : MonoBehaviour
    {
        public static event EventHandler OnGunShoot;
        
        [SerializeField] private GameObject _bullet;
        [SerializeField] private GameObject _muzzlleFlash;
        [SerializeField] private Transform _shootPoint ;
        EnemyAnimationController _controller;
        void Start()
        {
            _controller = GetComponentInParent<EnemyAnimationController>();
            _controller.OnShoot += Shoot;
            _controller.OnGotHit += DropGun;
        }
        private void OnDestroy()
        {
            _controller.OnGotHit -= DropGun;
            _controller.OnShoot -= Shoot;
        }

        void Shoot()
        {
            _muzzlleFlash.SetActive(true);
            GameObject bullet = Instantiate(_bullet, _shootPoint.position, _shootPoint.rotation);
            bullet.transform.DOMove(new Vector3(FindAnyObjectByType<LineFollower>().transform.position.x , 
            bullet.transform.position.y , FindAnyObjectByType<LineFollower>().transform.position.z), .3f);
            
            OnGunShoot?.Invoke(this, EventArgs.Empty);
            // Get the bullet's Rigidbody component
          //  Rigidbody bulletRigidbody = bullet.AddComponent<Rigidbody>();

            // Check if the bullet has a Rigidbody
          //  if (bulletRigidbody != null)
           // {
                // Add an impulse force to the bullet in the forward direction
               // bulletRigidbody.AddForce(_shootPoint.forward * 10 , ForceMode.Impulse);
          //  }
        }

        public void DropGun()
        {
          
            transform.AddComponent<BoxCollider>();
          
            transform.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
  
            transform.GetChild(01).gameObject.SetActive(true);
            transform.GetComponent<MeshRenderer>().enabled = false;

            transform.parent = null;

        }

    }
}
