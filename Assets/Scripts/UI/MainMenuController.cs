using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.IO;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField]
        private GameObject _loadCustomLevelPanel;

        [SerializeField]
        private GameObject _mainMenuPanel;

        [SerializeField]
        private Dropdown _levelsDropdown;

        [SerializeField]
        private Dropdown _difficultyDropdown;

        [SerializeField]
        private EditorIOManager _ioManager;

        [SerializeField]
        private string _dynamicLevelLoaderSceneName;

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

        public void ShowSelectLevelPanel()
        {
            _loadCustomLevelPanel.SetActive(true);
            _mainMenuPanel.SetActive(false);
            SetLoadLevelUI();
        }

        private void SetLoadLevelUI()
        {
            _ioManager.Init();
            var levelsList = _ioManager.GetSavedLevels();
            var difficultyOptions = Enum.GetNames(typeof(GameDifficultyLevel)).ToList();

            _levelsDropdown.ClearOptions();
            _difficultyDropdown.ClearOptions();

            _levelsDropdown.AddOptions(levelsList.ToList());
            _difficultyDropdown.AddOptions(difficultyOptions);
        }

        public void StartSelectedLevel()
        {
            var levelName = _levelsDropdown.options[_levelsDropdown.value].text;

            PlayerPrefs.SetInt(typeof(GameDifficultyLevel).ToString(), _difficultyDropdown.value);
            PlayerPrefs.SetInt(typeof(DynamicLevelLoaderOption).Name, (int)DynamicLevelLoaderOption.LoadLevelName);
            PlayerPrefs.SetString(DynamicLevelLoaderOption.LoadLevelName.ToString(), levelName);

            SceneManager.LoadScene(_dynamicLevelLoaderSceneName);
        }

        public void ReturnToMainMenu()
        {
            _loadCustomLevelPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
        }
    }
}
