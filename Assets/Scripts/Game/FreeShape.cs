using AgistForms.Assets.Scripts.Enums;
using System;
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

        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            AddForces();
        }

        private void AddForces()
        {
            _rigidbody2D.velocity = _startDirection * _speed;
            //_rigidbody2D.AddForce(Vector2.right * _speed, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
        }
    }
}

