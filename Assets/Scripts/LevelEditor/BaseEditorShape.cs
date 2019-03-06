using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.Structs;
using System.Collections.Generic;
using UnityEngine;

namespace AgistForms.Assets.Scripts.LevelEditor
{
    public abstract class BaseEditorShape : MonoBehaviour
    {
        [SerializeField]
        private ShapeType _shapeType;

        [SerializeField]
        private ShapeSprite[] _spites;

        [SerializeField]
        private Color _color;

        [SerializeField]
        private Color _errorColor;

        [SerializeField]
        protected Camera _mainCamera;

        public bool IsDragging { get; set; }

        protected Dictionary<ShapeType, Sprite> _spritesCache;
        protected SpriteRenderer _spriteRenderer;
        protected EditorController _controller;


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

        protected void PrepareSpriteCaches()
        {
            _spritesCache = new Dictionary<ShapeType, Sprite>();

            foreach (var item in _spites)
            {
                _spritesCache.Add(item.ShapeType, item.Sprite);
            }
        }

        public virtual void Start()
        {
            _mainCamera = FindObjectOfType<Camera>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            PrepareSpriteCaches();
            ShapeType = ShapeType;
        }

        private void Update()
        {
            if (!IsDragging) return;

            var stp = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(stp.x, stp.y);
        }

        private void OnMouseDown()
        {
            _controller.CurrentShape = this;
            IsDragging = true;
        }

        private void OnMouseUp()
        {
            IsDragging = false;
        }

        public void InjectController(EditorController ec)
        {
            _controller = ec;
        }
    }
}
