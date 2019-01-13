using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.Structs;
using System;
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
        private Dictionary<Commands, Vector2> _commandToDirection;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _commandToDirection = new Dictionary<Commands, Vector2>
            {
                {Commands.Up, Vector2.up },
                {Commands.Down, Vector2.down },
                {Commands.Left, Vector2.left },
                {Commands.Right, Vector2.right }
};
            _spriteRenderer = GetComponent<SpriteRenderer>();
            PrepareSpriteCaches();
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
                _spriteRenderer.sprite = _spriteCache[value];
            }
        }

        internal void GetCommand(Commands command)
        {
            var spd = _speed * Time.deltaTime;
            transform.Translate(_commandToDirection[command] * spd);
        }
    }
}
