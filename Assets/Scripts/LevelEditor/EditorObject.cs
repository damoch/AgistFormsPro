using UnityEngine;

namespace AgistForms.Assets.Scripts.LevelEditor
{
    public class EditorObject : MonoBehaviour
    {
        [SerializeField]
        private GameObject _originalObject;

        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _collider;
        private Rigidbody2D _rigidbody;
        private bool _isDragging;
        private Camera _mainCamera;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<BoxCollider2D>();
            _mainCamera = FindObjectOfType<Camera>();
            SetOriginalRepresentation();
        }

        private void SetOriginalRepresentation()
        {
            transform.localScale = _originalObject.transform.localScale;
            _spriteRenderer.sprite = _originalObject.GetComponent<SpriteRenderer>().sprite;
            var col = _originalObject.GetComponent<BoxCollider2D>();
            _collider.size = col.size;
        }

        private void Update()
        {
            if (!_isDragging) return;

            var stp = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(stp.x, stp.y);
        }

        private void OnMouseDown()
        {
            _isDragging = true;
        }

        private void OnMouseUp()
        {
            _isDragging = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log(collision);
        }
    }
}
