using _Game.Managers;
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

        private void EnableObj() => transform.GetChild(0).gameObject.SetActive(true);

        public void NextBtnCallBack()
        {
            SceneManager.LoadScene(0);
        }
    }
}
