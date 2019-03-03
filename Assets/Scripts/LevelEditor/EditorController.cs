using AgistForms.Assets.Scripts.IO;
using AgistForms.Assets.Scripts.Structs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgistForms.Assets.Scripts.LevelEditor
{
    public class EditorController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _editorObjectPrototype;

        private EditorShape _currentShape;
        private EditorFile _editorFile;


        private void Start()
        {
            CreateNewLevel();
        }

        private void CreateNewLevel()
        {
            _editorFile = new EditorFile("test");
        }

        public void CreateNewShape(int shapeType)
        {
            var gObj = Instantiate(_editorObjectPrototype);
            _currentShape = gObj.GetComponent<EditorShape>();
            _currentShape.InjectController(this);
            _currentShape.ShapeType = (Enums.ShapeType)shapeType;
            _editorFile.FreeShapes.Add(_currentShape);
        }

        public void SaveLevel()
        {
            Debug.Log(_editorFile.Serialize());
        }
    }
}
