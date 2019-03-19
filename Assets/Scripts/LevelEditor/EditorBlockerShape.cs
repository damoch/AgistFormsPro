using AgistForms.Assets.Scripts.Structs;

namespace AgistForms.Assets.Scripts.LevelEditor
{
    public class EditorBlockerShape : BaseEditorShape
    {
        public ObjectSaveState GetSaveState()
        {
            return new ObjectSaveState
            {
                StartingPosition = transform.position,
                StartingShapeType = ShapeType,
                StartingDirection = Enums.Direction.Down//Unused
            };
        }

        public void SetSavedState(ObjectSaveState state)
        {
            transform.position = state.StartingPosition;
            ShapeType = state.StartingShapeType;
        }
    }
}
