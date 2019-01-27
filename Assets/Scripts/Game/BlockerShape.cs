using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.Structs;
using System.Linq;
using UnityEngine;

namespace AgistForms.Assets.Scripts.Game
{
    public class BlockerShape : MonoBehaviour
    {
        [SerializeField]
        private ShapeType _shapeType;

        [SerializeField]
        private ShapeSprite[] _sprites;

        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            SetSprite();
            
        }

        private void SetSprite()
        {
            _spriteRenderer.sprite = _sprites.First(x => x.ShapeType == _shapeType).Sprite;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<Player>();

            if(player && player.ShapeType == _shapeType)
            {
                gameObject.SetActive(false);
            }
        }


    }
}
