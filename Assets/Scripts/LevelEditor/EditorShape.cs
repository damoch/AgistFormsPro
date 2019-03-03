using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.Structs;
using System.Collections.Generic;
using UnityEngine;

namespace AgistForms.Assets.Scripts.LevelEditor
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EditorShape : MonoBehaviour
    {
        [SerializeField]
        private ShapeType _shapeType;

        [SerializeField]
        private Direction _startDirection;

        [SerializeField]
        private ShapeSprite[] _spites;

        [SerializeField]
        private Color _color;

        [SerializeField]
        private Color _errorColor;

        [SerializeField]
        private Camera _mainCamera;

        private Dictionary<ShapeType, Sprite> _spritesCache;
        private SpriteRenderer _spriteRenderer;
        private EditorController _controller;


        public ShapeType ShapeType
        {
            get
            {
                return _shapeType;
            }

            set
            {
                _shapeType = value;
                if (_spritesCache != null)
                {
                    _spriteRenderer.sprite = _spritesCache[value];
                    _spriteRenderer.color = _color;
                }
            }
        }

        public bool IsDragging { get; set; }

        private void Start()
        {
            _mainCamera = FindObjectOfType<Camera>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            PrepareSpriteCaches();
            ShapeType = _shapeType;
        }

        private void PrepareSpriteCaches()
        {
            _spritesCache = new Dictionary<ShapeType, Sprite>();

            foreach (var item in _spites)
            {
                _spritesCache.Add(item.ShapeType, item.Sprite);
            }
        }

        private void Update()
        {
            if (!IsDragging) return;

            var stp = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(stp.x, stp.y);
        }

        private void OnMouseDown()
        {
            IsDragging = true;
        }

        private void OnMouseUp()
        {
            IsDragging = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log(collision);
        }

        public void InjectController(EditorController ec)
        {
            _controller = ec;
        }

        public ObjectSaveState GetSaveState()
        {
            return new ObjectSaveState
            {
                StartingPosition = transform.position,
                StartingShapeType = _shapeType
            };
        }
    }
}
