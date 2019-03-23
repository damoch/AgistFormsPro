using System;
using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

namespace AgistForms.Assets.Scripts.LevelEditor
{
    public class EditorController : MonoBehaviour
    {
        [SerializeField]
        private PlayerEditorShape _playerShape;

        [SerializeField]
        private GameObject _editorObjectPrototype;

        [SerializeField]
        private GameObject _targetShapePrototype;

        [SerializeField]
        private GameObject _blockerShapePrototype;

        [SerializeField]
        private Dropdown _directionDropdown;

        [SerializeField]
        private Dropdown _selectLevelDropdown;

        [SerializeField]
        private InputField _filenameField;

        [SerializeField]
        private GameObject _editorPanel;

        [SerializeField]
        private GameObject _newLevelPanel;

        [SerializeField]
        private EditorIOManager _ioManager;

        [SerializeField]
        private string _playSceneName;

        [SerializeField]
        private string _mainMenuSceneName;

        [SerializeField]
        private int _editorVersion;

        [SerializeField]
        private Dropdown _difficultyDropdown;

        [SerializeField]
        private Text _versionText;

        [SerializeField]
        private Button _loadLevelButton;

        private EditorFile _editorFile;
        private BaseEditorShape _currentShape;
        private LevelEditorStartupOption _levelEditorStartupOption;

        public BaseEditorShape CurrentShape
        {
            get
            {
                return _currentShape;
            }
            set
            {

                _currentShape = value;
                if(_currentShape is EditorShape)
                {
                    _directionDropdown.value = (int)((EditorShape)_currentShape).StartDirection;
                }
            }
        }

        private void Start()
        {
            if (!PlayerPrefs.HasKey(typeof(LevelEditorStartupOption).Name))
            {
                _levelEditorStartupOption = LevelEditorStartupOption.NewLevel;
            }
            else
            {
                _levelEditorStartupOption = (LevelEditorStartupOption)PlayerPrefs.GetInt(typeof(LevelEditorStartupOption).Name);
            }

            _ioManager.Init();

            if(_levelEditorStartupOption == LevelEditorStartupOption.NewLevel)
            {
                SetWelcomeScreenUI();

            }
            else if(_levelEditorStartupOption == LevelEditorStartupOption.LoadTemp)
            {
                LoadTempLevel();
            }

            _ioManager.CleanTestData();
            PlayerPrefs.SetInt(typeof(LevelEditorStartupOption).Name, (int)LevelEditorStartupOption.NewLevel);
        }

        private void SetUI()
        {
            _newLevelPanel.SetActive(false);
            _editorPanel.SetActive(true);
            var directionOptions = Enum.GetNames(typeof(Direction)).ToList();
            var difficultyOptions = Enum.GetNames(typeof(GameDifficultyLevel)).ToList();
            _directionDropdown.ClearOptions();
            _directionDropdown.AddOptions(directionOptions);
            _directionDropdown.onValueChanged.AddListener(delegate
            {
                OnDirectionDropdownValueChanged(_directionDropdown);
            });

            _difficultyDropdown.ClearOptions();
            _difficultyDropdown.AddOptions(difficultyOptions);

        }

        private void SetWelcomeScreenUI()
        {
            _versionText.text = string.Format(_versionText.text, _editorVersion);
            var lvls = _ioManager.GetSavedLevels();
            _selectLevelDropdown.ClearOptions();
            if (lvls.Length > 0)
            {
                _selectLevelDropdown.AddOptions(lvls.ToList());
            }
            else
            {
                _loadLevelButton.enabled = false;
            }
        }

        public void CreateNewLevel()
        {
            if (string.IsNullOrEmpty(_filenameField.text))
            {
                return;
            }
            _playerShape.InjectController(this);
            _editorFile = new EditorFile(_filenameField.text, _playerShape, _editorVersion);
            SetUI();
        }

        public void LoadSelectedLevel()
        {
            var json = _ioManager.GetLevelData(_selectLevelDropdown.options[_selectLevelDropdown.value].text);
            _editorFile = EditorFile.FromJson(json, _playerShape, AddShape, AddTarget, AddBlocker, _editorVersion);
            _playerShape.InjectController(this);
            SetUI();
        }

        private void LoadTempLevel()
        {
            var json = _ioManager.GetTempLevelData();
            _editorFile = EditorFile.FromJson(json, _playerShape, AddShape, AddTarget, AddBlocker, _editorVersion);
            _playerShape.InjectController(this);
            SetUI();
        }

        public void CreateNewShape(int shapeType)
        {
            AddShape(shapeType);
        }

        public void CreateNewTarget(int shapetype)
        {
            AddTarget(shapetype);
        }

        public void CreateNewBlocker(int shapeType)
        {
            AddBlocker(shapeType);
        }

        private EditorShape AddShape(int shapeType)
        {
            var gObj = Instantiate(_editorObjectPrototype);
            var curr = gObj.GetComponent<EditorShape>();
            CurrentShape = curr;
            curr.InjectController(this);
            ChangeCurrentShape(shapeType);
            if(_editorFile != null)
            {
                _editorFile.FreeShapes.Add(curr);
            }
            return curr;
        }


        private EditorTargetShape AddTarget(int shapeType)
        {
            var gObj = Instantiate(_targetShapePrototype);
            var curr = gObj.GetComponent<EditorTargetShape>();
            CurrentShape = curr;
            curr.InjectController(this);
            ChangeCurrentShape(shapeType);
            if(_editorFile != null)
            {
                _editorFile.TargetShapes.Add(curr);
            }
            return curr;
        }

        private EditorBlockerShape AddBlocker(int shapeType)
        {
            var gObj = Instantiate(_blockerShapePrototype);
            var curr = gObj.GetComponent<EditorBlockerShape>();
            CurrentShape = curr;
            curr.InjectController(this);
            ChangeCurrentShape(shapeType);
            if (_editorFile != null)
            {
                _editorFile.BlockerShapes.Add(curr);
            }
            return curr;

        }

        public void ChangeCurrentShape(int shapeType)
        {
            if (!_currentShape)
            {
                return;
            }
            CurrentShape.ShapeType = (ShapeType)shapeType;
        }

        public void SaveLevel()
        {
            _ioManager.SaveLevelData(_editorFile);
        }

        public void OnDirectionDropdownValueChanged(Dropdown change)
        {
            if (!_currentShape || !(_currentShape is EditorShape))
            {
                return;
            }
            ((EditorShape)_currentShape).StartDirection = (Direction)change.value;
        }

        public void PlayTestLevel()
        {
            PlayerPrefs.SetInt(typeof(GameDifficultyLevel).ToString(), _difficultyDropdown.value);
            PlayerPrefs.SetInt(typeof(DynamicLevelLoaderOption).Name, (int)DynamicLevelLoaderOption.LoadTempData);
            _ioManager.SaveTempLevelData(_editorFile);
            SceneManager.LoadScene(_playSceneName);
        }

        public void DeleteSelectedShape()
        {
            if(_currentShape == null || _currentShape == _playerShape)
            {
                return;
            }
            if(_currentShape is EditorShape)
            {
                _editorFile.FreeShapes.Remove(_currentShape as EditorShape);
            }
            else if (_currentShape is EditorTargetShape)
            {
                _editorFile.TargetShapes.Remove(_currentShape as EditorTargetShape);
            }
            else if(_currentShape is EditorBlockerShape)
            {
                _editorFile.BlockerShapes.Remove(_currentShape as EditorBlockerShape);
            }
            Destroy(_currentShape.gameObject);
            _currentShape = null;
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene(_mainMenuSceneName);
        }
    }
}
