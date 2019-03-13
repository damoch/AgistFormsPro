using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.Structs;
using System.Collections.Generic;
using UnityEngine;

namespace AgistForms.Assets.Scripts.Game
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private ShapeType _shapeType;

        [SerializeField]
        private ShapeSprite[] _friendlySprites;

        [SerializeField]
        private float _speed;

        private Dictionary<ShapeType, Sprite> _spriteCache;
        private Dictionary<Direction, Vector2> _commandToDirection;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _commandToDirection = new Dictionary<Direction, Vector2>
            {
                {Direction.Up, Vector2.up },
                {Direction.Down, Vector2.down },
                {Direction.Left, Vector2.left },
                {Direction.Right, Vector2.right }
};
            _spriteRenderer = GetComponent<SpriteRenderer>();
            PrepareSpriteCaches();
            _spriteRenderer.sprite = _spriteCache[_shapeType];
        }

        private void PrepareSpriteCaches()
        {
            _spriteCache = new Dictionary<ShapeType, Sprite>();

            foreach (var item in _friendlySprites)
            {
                _spriteCache.Add(item.ShapeType, item.Sprite);
            }
        }

        public ShapeType ShapeType
        {
            get
            {
                return _shapeType;
            }

            set
            {
                _shapeType = value;
                if(_spriteRenderer == null)
                {
                    return;
                }
                _spriteRenderer.sprite = _spriteCache[value];
            }
        }

        internal void GetCommand(Direction command)
        {
            var spd = _speed * Time.deltaTime;
            transform.Translate(_commandToDirection[command] * spd);
        }

        public void SetSavedState(ObjectSaveState state)
        {
            transform.position = state.StartingPosition;
            ShapeType = state.StartingShapeType;
        }
    }
}
