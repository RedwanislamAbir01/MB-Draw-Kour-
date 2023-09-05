using System.Collections;
using _Game.Managers;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace _Game
{
    public class FailUi : MonoBehaviour
    {
        private void Start()
        {
            DisableObj();
            GameManager.Instance.OnLevelFail += EnableObj;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnLevelFail -= EnableObj;
        }
        private void DisableObj() => transform.GetChild(0).gameObject.SetActive(false);

        private void EnableObj(float delay)
        {
            StartCoroutine(EnableRoutine(delay));
        }

        private IEnumerator EnableRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            transform.GetChild(0).gameObject.SetActive(true);
        }

        public void RetryBtnCallBack()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            // Reload the current scene
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
