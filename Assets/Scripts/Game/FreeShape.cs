using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.Game;
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
        private Direction _startDirection;

        [SerializeField]
        private ShapeSprite[] _friendlySprites;

        [SerializeField]
        private ShapeSprite[] _enemySprites;

        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private Dictionary<ShapeType, Sprite> _friendlySpritesCache;
        private Dictionary<ShapeType, Sprite> _enemySpritesCache;
        private Dictionary<Direction, Vector2> _directionToVector = new Dictionary<Direction, Vector2>
        {
            { Direction.Up, Vector2.up },
            { Direction.Down, Vector2.down },
            { Direction.Left, Vector2.left },
            { Direction.Right, Vector2.right },
            { Direction.UpRight, new Vector2(1,1) },
            { Direction.DownRight, new Vector2(-1,1) },
            { Direction.UpLeft, new Vector2(1,-1) },
            { Direction.DownLeft, new Vector2(-1,-1) },
        };
        private CollisionResult _collisionResult;


        public LevelController LevelController { get; set; }
        public CollisionResult CollisionResult
        {
            get { return _collisionResult; }
            set
            {
                _collisionResult = value;
                _spriteRenderer.sprite = _collisionResult == CollisionResult.DestroyPlayer ? _enemySpritesCache[ShapeType] : _friendlySpritesCache[ShapeType];

            }
        }
        public ShapeType ShapeType { get { return _shapeType; } }

        private void Awake()
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
            _rigidbody2D.velocity = _directionToVector[_startDirection] * _speed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var other = collision.collider.gameObject.GetComponent<Player>();

            if (other != null)
            {
                switch (CollisionResult)
                {
                    case CollisionResult.NoEffect:
                        break;
                    case CollisionResult.ShapeShift:
                        var oshType = other.ShapeType;
                        other.ShapeType = _shapeType;
                        _shapeType = oshType;
                        _spriteRenderer.sprite = _friendlySpritesCache[_shapeType];
                        LevelController.UpdateAllShapes(other.ShapeType);
                        break;
                    case CollisionResult.DestroyPlayer:
                        break;
                }
            }
        }
    }
}

