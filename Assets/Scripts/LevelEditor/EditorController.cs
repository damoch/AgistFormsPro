using System;
using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace AgistForms.Assets.Scripts.LevelEditor
{
    public class EditorController : MonoBehaviour
    {
        [SerializeField]
        private PlayerEditorShape _playerShape;

        [SerializeField]
        private GameObject _editorObjectPrototype;

        [SerializeField]
        private Dropdown _directionDropdown;

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
            SetUI();
            CreateNewLevel();
            _playerShape.InjectController(this);
        }

        private void SetUI()
        {
            var enumOptions = Enum.GetNames(typeof(Direction)).ToList();
            _directionDropdown.ClearOptions();
            _directionDropdown.AddOptions(enumOptions);
            _directionDropdown.onValueChanged.AddListener(delegate
            {
                OnDroptownValueChanged(_directionDropdown);
            });


        }

        private void CreateNewLevel()
        {
            _editorFile = new EditorFile("test", _playerShape);
        }

        public void CreateNewShape(int shapeType)
        {
            var gObj = Instantiate(_editorObjectPrototype);
            var curr = gObj.GetComponent<EditorShape>();
            CurrentShape = curr;
            curr.InjectController(this);
            ChangeCurrentShape(shapeType);
            _editorFile.FreeShapes.Add(curr);
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
            Debug.Log(_editorFile.Serialize());
        }

        public void OnDroptownValueChanged(Dropdown change)
        {
            if (!_currentShape || !(_currentShape is EditorShape))
            {
                return;
            }
            ((EditorShape)_currentShape).StartDirection = (Direction)change.value;
        }
    }
}
