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

        private EditorFile _editorFile;
        private BaseEditorShape _currentShape;

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
            _ioManager.Init();
            SetWelcomeScreenUI();
        }

        private void SetUI()
        {
            _newLevelPanel.SetActive(false);
            _editorPanel.SetActive(true);
            var enumOptions = Enum.GetNames(typeof(Direction)).ToList();
            _directionDropdown.ClearOptions();
            _directionDropdown.AddOptions(enumOptions);
            _directionDropdown.onValueChanged.AddListener(delegate
            {
                OnDroptownValueChanged(_directionDropdown);
            });
        }

        private void SetWelcomeScreenUI()
        {
            var lvls = _ioManager.GetSavedLevels();
            _selectLevelDropdown.ClearOptions();
            _selectLevelDropdown.AddOptions(lvls.ToList());
        }

        public void CreateNewLevel()
        {
            _playerShape.InjectController(this);
            _editorFile = new EditorFile(_filenameField.text, _playerShape);
            SetUI();
        }

        public void LoadSelectedLevel()
        {
            var json = _ioManager.GetLevelData(_selectLevelDropdown.options[_selectLevelDropdown.value].text);
            _editorFile = EditorFile.FromJson(json, _playerShape, AddShape, AddTarget);
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

        public void OnDroptownValueChanged(Dropdown change)
        {
            if (!_currentShape || !(_currentShape is EditorShape))
            {
                return;
            }
            ((EditorShape)_currentShape).StartDirection = (Direction)change.value;
        }

        public void PlayTestLevel()
        {
            _ioManager.SaveTempLevelData(_editorFile);
            SceneManager.LoadScene(_playSceneName);
        }
    }
}
