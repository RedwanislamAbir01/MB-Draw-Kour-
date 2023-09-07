using _Game.Managers;
using _Tools.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game
{
    public class LevelCompleteUi : MonoBehaviour
    {
    
        private void Start()
        {
            DisableObj();
            GameManager.Instance.OnLevelComplete += EnableObj;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnLevelComplete -= EnableObj;
        }
        private void DisableObj() => transform.GetChild(0).gameObject.SetActive(false);

        private void EnableObj(float delay) => StartCoroutine(EnableRoutine(delay));

        private IEnumerator EnableRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            transform.GetChild(0).gameObject.SetActive(true);
        }

        private int currentSceneIndex = 0;

        public void NextBtnCallBack()
        {
            var nextSceneIndex = LevelManager.Instance.GetNextSceneIndex();
          
           SceneUtils.LoadSpecificScene(nextSceneIndex);
            
        }

    }
}
