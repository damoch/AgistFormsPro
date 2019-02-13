using AgistForms.Assets.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AgistForms.Assets.Scripts.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        private Text _versionText;

        [SerializeField]
        private string _versionDescription;

        [SerializeField]
        private List<GameObject> _windowsOnlyButtons;

        private void Start()
        {
            _versionText.text = _versionDescription + " " + Application.version;

            if(Application.platform == RuntimePlatform.WebGLPlayer)
            {
                HideWindowsOnlyButtons();
            }
        }

        private void HideWindowsOnlyButtons()
        {
            foreach(var item in _windowsOnlyButtons)
            {
                item.SetActive(false);
            }
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void StartLevel(string levelName)
        {
            PlayerPrefs.SetInt(typeof(GameDifficultyLevel).ToString(), (int)GameDifficultyLevel.Normal);
            SceneManager.LoadScene(levelName);
        }

        public void StartLevelHard(string levelName)
        {
            PlayerPrefs.SetInt(typeof(GameDifficultyLevel).ToString(), (int)GameDifficultyLevel.Hard);
            SceneManager.LoadScene(levelName);
        }
    }
}
