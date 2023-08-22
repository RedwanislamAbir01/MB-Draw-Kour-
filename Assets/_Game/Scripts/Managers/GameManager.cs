using System;
using _Tools.Extensions;
using _Tools.Helpers;
using UnityEngine;

namespace _Game.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        #region Events

        public event Action OnLevelStart;
        public event Action OnLevelComplete;
        public event Action<float> OnLevelFail;
        public event Action OnEolTrigger;
        #endregion

        #region Variables

        [SerializeField, Range(0f, 10f)] private float _levelEndUIDelay = 2f;

        #endregion

        #region Custom Methods

        public void EolTrigger() => OnEolTrigger?.Invoke();
        public void LevelStart() => OnLevelStart?.Invoke();

        public void LevelComplete()
        {
            OnLevelComplete?.Invoke();
            
            //var nextSceneIndex = LevelManager.Instance.GetNextSceneIndex();
            //if (UIManager.Instance.IsNotNull(nameof(UIManager)))
            //{
            //    UIManager.Instance.LevelComplete(_levelLoadDelay, () => SceneUtils.LoadSpecificScene(nextSceneIndex));   
            //}
        }

        public void LevelFail()
        {
            OnLevelFail?.Invoke(_levelEndUIDelay);
            
            //if (UIManager.Instance.IsNotNull(nameof(UIManager))) UIManager.Instance.LevelReloadTransition(_levelLoadDelay, SceneUtils.ReloadScene);
        }

        #endregion
    }
}