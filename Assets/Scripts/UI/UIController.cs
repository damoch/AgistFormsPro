using UnityEngine;
using UnityEngine.UI;

namespace AgistForms.Assets.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private Text _timeText;

        public string TimeText
        {
            get
            {
                return _timeText.text;
            }

            set
            {
                _timeText.text = value;
            }
        }

        [SerializeField]
        private Text _shapeShiftsText;

        public string ShapeShiftsText
        {
            get
            {
                return _shapeShiftsText.text;
            }

            set
            {
                _shapeShiftsText.text = value;
            }
        }

        public string TimeDisplayFormat
        {
            get
            {
                return _timeDisplayFormat;
            }
        }

        [SerializeField]
        private string _timeDisplayFormat;

        [SerializeField]
        private Color _goodColor;

        [SerializeField]
        private Color _badColor;

        [SerializeField]
        private Text _parTimeText;

        [SerializeField]
        private Text _parShapeShiftsText;

        [SerializeField]
        private string _shapeShiftsParText;

        [SerializeField]
        private string _timeParText;

        [SerializeField]
        private string _newRecordText;

        private void Start()
        {
            EnableParTexts(false);
        }

        public void EnableParTexts(bool enable)
        {
            _parShapeShiftsText.gameObject.SetActive(enable);
            _parTimeText.gameObject.SetActive(enable);
        }

        public void SetShapeShiftsPar(int count, bool isRecord)
        {
            _parShapeShiftsText.text = _shapeShiftsParText + count.ToString();
            _parShapeShiftsText.color = isRecord ? _goodColor : _badColor;
        }

        public void SetTimePar(float value, bool isRecord)
        {
            _parTimeText.text = _timeParText + value.ToString(_timeDisplayFormat) + (isRecord ? _newRecordText : "");
            _parTimeText.color = isRecord ? _goodColor : _badColor;

        }
    }
}
