using _Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game
{
    public class LevelStartUi : MonoBehaviour
    {
        // Start is called before the first frame update

        public void StartBtnCallBack()
        {
            GameManager.Instance.LevelStart();
            gameObject.SetActive(false);
           
        }
    }
}
