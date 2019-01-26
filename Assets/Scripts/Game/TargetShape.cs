using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.Structs;
using System.Collections.Generic;
using UnityEngine;

namespace AgistForms.Assets.Scripts.Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TargetShape : MonoBehaviour
    {
        public LevelController LevelController { get; set; }
        public bool SuccesfullCollison
        {
            get
            {
                return _succesfullCollison;
            }
        }

        [SerializeField]
        private ShapeSprite[] _wrongShapes;


        [SerializeField]
        private ShapeSprite[] _rightShapes;

        [SerializeField]
        private ShapeType _targetType;

        [SerializeField]
        private bool _succesfullCollison;

        private Dictionary<ShapeType, Sprite> _rightShapesCache;
        private Dictionary<ShapeType, Sprite> _wrongShapesCache;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            PrepareSpriteCaches();
            SetSprite(false);
        }

        private void SetSprite(bool isCorrect)
        {
            _spriteRenderer.sprite = isCorrect ? _rightShapesCache[_targetType] : _wrongShapesCache[_targetType];
        }

        private void PrepareSpriteCaches()
        {
            _rightShapesCache = new Dictionary<ShapeType, Sprite>();
            _wrongShapesCache = new Dictionary<ShapeType, Sprite>();

            foreach (var shape in _wrongShapes)
            {
                _wrongShapesCache.Add(shape.ShapeType, shape.Sprite);
            }

            foreach (var shape in _rightShapes)
            {
                _rightShapesCache.Add(shape.ShapeType, shape.Sprite);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.gameObject.GetComponent<Player>();

            if (player)
            {
                var succesfull = player.ShapeType == _targetType;
                SetSprite(succesfull);
                _succesfullCollison = succesfull;
                if (succesfull)
                {
                    LevelController.CheckTargetSprites();
                }
   
            }
        }
    }
}
