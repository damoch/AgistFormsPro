using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.Structs;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class FreeShape : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        [SerializeField]
        private ShapeType _shapeType;

        [SerializeField]
        private Vector2 _startDirection;

        [SerializeField]
        private ShapeSprite[] _friendlySprites;

        [SerializeField]
        private ShapeSprite[] _enemySprites;

        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private Dictionary<ShapeType, Sprite> _friendlySpritesCache;
        private Dictionary<ShapeType, Sprite> _enemySpritesCache;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            PrepareSpriteCaches();
            ChangeShape();
            AddForces();
        }

        private void PrepareSpriteCaches()
        {
            _friendlySpritesCache = new Dictionary<ShapeType, Sprite>();
            _enemySpritesCache = new Dictionary<ShapeType, Sprite>();

            foreach (var item in _friendlySprites)
            {
                _friendlySpritesCache.Add(item.ShapeType, item.Sprite);
            }
            foreach (var item in _enemySprites)
            {
                _enemySpritesCache.Add(item.ShapeType, item.Sprite);
            }
        }

        private void ChangeShape()
        {
            _spriteRenderer.sprite = _friendlySpritesCache[_shapeType];
        }

        private void AddForces()
        {
            _rigidbody2D.velocity = _startDirection * _speed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

        }
    }
}

