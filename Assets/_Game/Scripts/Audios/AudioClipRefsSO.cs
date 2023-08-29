using UnityEngine;

namespace _Game.Audios
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Audio Clip Refs", fileName = "New AudioClipRefs")]
    public class AudioClipRefsSO : ScriptableObject
    {
        #region Variables

        public AudioClip cashCollection;
        public AudioClip sliding;
        public AudioClip shooting;
        public AudioClip combatHit;
        public AudioClip fallDown;
        #endregion
    }
}