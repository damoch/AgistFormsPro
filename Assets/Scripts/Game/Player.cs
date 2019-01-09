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

        private Dictionary<ShapeType, Sprite> _spriteCache;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
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
    }
}
