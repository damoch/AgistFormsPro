using System;
using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace AgistForms.Assets.Scripts.LevelEditor
{
    [RequireComponent(typeof(EditorIOManager))]
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

        private EditorFile _editorFile;
        private BaseEditorShape _currentShape;
        private EditorIOManager _ioManager;

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
            _ioManager = GetComponent<EditorIOManager>();
            _ioManager.Init();

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
            _editorFile.FreeShapes.Add(curr);
            return curr;
        }


        private EditorTargetShape AddTarget(int shapeType)
        {
            var gObj = Instantiate(_targetShapePrototype);
            var curr = gObj.GetComponent<EditorTargetShape>();
            CurrentShape = curr;
            curr.InjectController(this);
            ChangeCurrentShape(shapeType);
            _editorFile.TargetShapes.Add(curr);
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
    }
}
