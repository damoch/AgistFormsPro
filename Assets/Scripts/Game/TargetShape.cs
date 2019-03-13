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

            set
            {
                _succesfullCollison = value;
                SetSprite(value);
            }
        }

        [SerializeField]
        private ShapeSprite[] _rightShapes;

        [SerializeField]
        private ShapeType _targetType;

        [SerializeField]
        private bool _succesfullCollison;

        [SerializeField]
        private Color _correctColor;

        [SerializeField]
        private Color _wrongColor;

        private Sprite _rightSprite;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            PrepareSprites();
            SetSprite(false);
        }

        private void SetSprite(bool isCorrect)
        {
            _spriteRenderer.color = isCorrect ? _correctColor : _wrongColor;
        }

        private void PrepareSprites()
        {
            _spriteRenderer.sprite = _rightShapes.First(x => x.ShapeType == _targetType).Sprite;
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

        public void SetSavedState(ObjectSaveState state)
        {
            transform.position = state.StartingPosition;
            _targetType = state.StartingShapeType;
        }
    }
}
