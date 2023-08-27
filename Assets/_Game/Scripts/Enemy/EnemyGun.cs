using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game
{
    public class EnemyGun : MonoBehaviour
    {
        [SerializeField] private GameObject _bullet;
        [SerializeField] private GameObject _muzzlleFlash;
        [SerializeField] private Transform _shootPoint ;
        EnemyAnimationController _controller;
        void Start()
        {
            _controller = GetComponentInParent<EnemyAnimationController>();
            _controller.OnShoot += Shoot;

        }
        private void OnDestroy()
        {
           
            _controller.OnShoot -= Shoot;
        }

        void Shoot()
        {
            _muzzlleFlash.SetActive(true);
            GameObject bullet = Instantiate(_bullet, _shootPoint.position, _shootPoint.rotation);
            bullet.transform.DOMove(new Vector3(FindAnyObjectByType<LineFollower>().transform.position.x , 
            bullet.transform.position.y , FindAnyObjectByType<LineFollower>().transform.position.z), .3f);
            // Get the bullet's Rigidbody component
          //  Rigidbody bulletRigidbody = bullet.AddComponent<Rigidbody>();

            // Check if the bullet has a Rigidbody
          //  if (bulletRigidbody != null)
           // {
                // Add an impulse force to the bullet in the forward direction
               // bulletRigidbody.AddForce(_shootPoint.forward * 10 , ForceMode.Impulse);
          //  }
        }
      
    }
}
