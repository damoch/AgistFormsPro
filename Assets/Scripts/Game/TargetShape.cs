using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.Structs;
using System.Linq;
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


        private Sprite _rightSprite;
        private Sprite _wrongSprite;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            PrepareSprites();
            SetSprite(false);
        }

        private void SetSprite(bool isCorrect)
        {
            _spriteRenderer.sprite = isCorrect ? _rightSprite : _wrongSprite;
        }

        private void PrepareSprites()
        {
            _rightSprite = _rightShapes.First(x => x.ShapeType == _targetType).Sprite;
            _wrongSprite = _wrongShapes.First(x => x.ShapeType == _targetType).Sprite;
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
