using _Game.Helpers;
using _Game.Managers;
using TMPro;
using UnityEngine;

namespace _Game
{
    public class LevelText : MonoBehaviour
    {
        TextMeshProUGUI _levelText;
        private void Awake()
        {
            _levelText = GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            var inGameLevelCount = PlayerPrefs.GetInt(ConstUtils.IN_GAME_LEVEL_COUNT) ;
            if(inGameLevelCount == 0)
            {
                inGameLevelCount = 01;
            }
            _levelText.text = "Level " + inGameLevelCount.ToString();

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
