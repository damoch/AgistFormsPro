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


    }
}
