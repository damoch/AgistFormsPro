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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log(collision);
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
    }
}
