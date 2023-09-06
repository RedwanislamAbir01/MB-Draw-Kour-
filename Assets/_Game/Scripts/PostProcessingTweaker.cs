using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

namespace _Game
{
    public class PostProcessingTweaker : MonoBehaviour
    {
        [SerializeField] private Volume postProcessVolume;
        [SerializeField] private Bloom bloom;

        private void Start()
        {
            postProcessVolume = GetComponent<Volume>();
            postProcessVolume.profile.TryGet(out bloom);
            if (postProcessVolume == null || !postProcessVolume.profile.TryGet(out bloom))
            {
                Debug.LogError("Post Process Volume or Bloom component not properly assigned.");
                return;
            }

            // Subscribe to the event.
            AnimationController.OnLastHit += BloomHike;
        }

        private void OnDestroy()
        {
            // Unsubscribe from the event to prevent memory leaks.
            AnimationController.OnLastHit -= BloomHike;
        }

        private void BloomHike()
        {
            Debug.Log("BloomHike triggered");

            // Tween the bloom intensity to 3f over 1 second.
            DOTween.To(() => bloom.intensity.value, x => bloom.intensity.value = x, 3.2f, .3f)
                .OnComplete(() => ResetBloomIntensity());
        }

        private void ResetBloomIntensity()
        {
            Debug.Log("ResetBloomIntensity triggered");

            // Tween the bloom intensity back to 1.2f over 1 second.
            DOTween.To(() => bloom.intensity.value, x => bloom.intensity.value = x, 1.2f, 1f);
        }
    }
}
