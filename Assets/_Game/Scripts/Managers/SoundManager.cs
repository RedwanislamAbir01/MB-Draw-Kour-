using System;
using _Game.Audios;
using _Tools.Helpers;
using UnityEngine;

namespace _Game.Managers
{
    public class SoundManager : Singleton<SoundManager>
    {
        #region Variables

        private const float VOLUME = 1f;

        [SerializeField] private AudioClipRefsSO _audioClipRefsSO;

        #endregion

        #region Unity Methods

        private void Start()
        {
            PlayerItemCollector.OnItemCollected += PlayerItemCollector_OnItemCollected;
            PlayerSlide.OnSlide += PlayerSlide_OnSlide;
            EnemyGun.OnGunShoot += EnemyGun_OnGunShoot;
        }
        
        private void OnDestroy()
        {
            PlayerItemCollector.OnItemCollected -= PlayerItemCollector_OnItemCollected;
            PlayerSlide.OnSlide -= PlayerSlide_OnSlide;
            EnemyGun.OnGunShoot -= EnemyGun_OnGunShoot;
        }

        #endregion

        #region Custom Methods

        private void PlayerItemCollector_OnItemCollected(object sender, EventArgs e)
        {
            var player = sender as PlayerItemCollector;
            PlaySound(_audioClipRefsSO.cashCollection, player.transform.position);
        }

        private void PlayerSlide_OnSlide(object sender, EventArgs e)
        {
            var player = sender as PlayerSlide;
            PlaySound(_audioClipRefsSO.sliding, player.transform.position);
        }

        private void EnemyGun_OnGunShoot(object sender, EventArgs e)
        {
            var enemy = sender as EnemyGun;
            PlaySound(_audioClipRefsSO.shooting, enemy.transform.position);
        }
        
        private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
        {
            AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * VOLUME);
        }

        #endregion
    }
}
