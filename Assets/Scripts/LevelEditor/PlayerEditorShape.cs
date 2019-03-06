using AgistForms.Assets.Scripts.Structs;

namespace AgistForms.Assets.Scripts.LevelEditor
{
    public class PlayerEditorShape : BaseEditorShape
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
    }
}
