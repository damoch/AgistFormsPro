using System;
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
        private Text _endLevelText;

        [SerializeField]
        private string _winLevelMessage;

        [SerializeField]
        private string _gameOverMessage;

        [SerializeField]
        private string _shapeShiftsParText;

        [SerializeField]
        private string _timeParText;

        [SerializeField]
        private string _newRecordText;

        [SerializeField]
        private GameObject _pausePanel;

        [SerializeField]
        private string _pauseMessage;

        [SerializeField]
        private string _resumeText;

        [SerializeField]
        private string _exitText;

        [SerializeField]
        private string _restartText;

        [SerializeField]
        private Text _pauseText;

        private string _pauseMsg;

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

        public void SetEndLevelText(bool show, bool victory = false, string keyToPress = null)
        {
            _endLevelText.gameObject.SetActive(show);
            if (!show)
            {
                return;
            }
            var message = victory ? _winLevelMessage : _gameOverMessage;
            _endLevelText.text = string.Format(message, keyToPress);
        }

        public void EnablePausePanel(bool enable)
        {
            _pausePanel.SetActive(enable);
            _pauseText.text = _pauseMsg;
        }

        public void SetPauseMessage(string resumeKey, string exitKey, string restartKey)
        {
            _pauseMsg = _pauseMessage + Environment.NewLine + string.Format(_restartText, restartKey) + Environment.NewLine + string.Format(_exitText, exitKey) +
                Environment.NewLine + string.Format(_resumeText, resumeKey);
        }
    }
}
