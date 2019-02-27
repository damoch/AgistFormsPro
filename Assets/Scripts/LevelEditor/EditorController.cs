using AgistForms.Assets.Scripts.Structs;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AgistForms.Assets.Scripts.LevelEditor
{
    public class EditorController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _editorObjectPrototype;

        private EditorShape _currentShape;

        [SerializeField]
        private List<EditorShape> _shapesOnScene;

        public void CreateNewShape(int shapeType)
        {
            var gObj = Instantiate(_editorObjectPrototype);
            _currentShape = gObj.GetComponent<EditorShape>();
            _currentShape.InjectController(this);
            _currentShape.ShapeType = (Enums.ShapeType)shapeType;
            _shapesOnScene.Add(_currentShape);
        }

        public void SaveLevel()
        {
            var savedShapes = new string[_shapesOnScene.Count];

            for (int i = 0; i < _shapesOnScene.Count; i++)
            {
                var shape = _shapesOnScene[i];
                savedShapes[i] = JsonUtility.ToJson(new ObjectSaveState
                {
                    StartingPosition = shape.gameObject.transform.position,
                    StartingShapeType = shape.ShapeType
                });
            }

            var json = JsonUtility.ToJson(new 
            {
                FreeShapes = savedShapes,
            });
            Debug.Log(savedShapes);
            Debug.Log(JsonUtility.ToJson(savedShapes));
        }
    }
}
