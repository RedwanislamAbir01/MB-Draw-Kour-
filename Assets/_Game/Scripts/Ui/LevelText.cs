using _Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game
{
    public class LevelText : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            EnableObj();
            GameManager.Instance.OnLevelStart += DisableObj;
        }
        private void OnDestroy()
        {
            GameManager.Instance.OnLevelStart -= DisableObj;
        }
        void DisableObj() => gameObject.SetActive(false);
        void EnableObj() => gameObject.SetActive(true);
    }
}
