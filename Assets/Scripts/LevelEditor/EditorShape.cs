using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.Structs;
using UnityEngine;

namespace AgistForms.Assets.Scripts.LevelEditor
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EditorShape : BaseEditorShape
    {
        [SerializeField]
        private Direction _startDirection;

        public Direction StartDirection
        {
            get
            {
                return _startDirection;
            }

            set
            {
                _startDirection = value;
            }
        }

        public ObjectSaveState GetSaveState()
        {
            return new ObjectSaveState
            {
                StartingPosition = transform.position,
                StartingShapeType = ShapeType,
                StartingDirection = _startDirection
            };
        }


        public void SetSavedState(ObjectSaveState state)
        {
            transform.position = state.StartingPosition;
            ShapeType = state.StartingShapeType;
            _startDirection = state.StartingDirection;
        }
    }
}
