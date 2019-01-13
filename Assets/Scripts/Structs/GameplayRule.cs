using AgistForms.Assets.Scripts.Enums;

namespace AgistForms.Assets.Scripts.Structs
{
    [System.Serializable]
    public struct GameplayRule
    {
        public ShapeType PlayerShape;
        public ShapeType FreeShape;
        public CollisionResult CollisionResult;
    }
}
