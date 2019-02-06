using UnityEngine;

namespace AgistForms.Assets.Scripts.Data
{
    public class ScoreData : MonoBehaviour
    {
        [SerializeField]
        private float _levelTime;

        [SerializeField]
        private int _shapeShifts;

        [SerializeField]
        private float _bestTime;

        [SerializeField]
        private int _lowestShapeShifts;

        public float LevelTime
        {
            get
            {
                return _levelTime;
            }

            set
            {
                _levelTime = value;
            }
        }

        public int ShapeShifts
        {
            get
            {
                return _shapeShifts;
            }

            set
            {
                _shapeShifts = value;
            }
        }

        public float BestTime
        {
            get
            {
                return _bestTime;
            }
            set
            {
                _bestTime = value;
            }
        }

        public int LowestShapeShifts
        {
            get
            {
                return _lowestShapeShifts;
            }
            set
            {
                _lowestShapeShifts = value;
            }
        }

        public string LevelName { get; set; }

        public bool ShapeShiftsRecord { get { return _shapeShifts <= _lowestShapeShifts; } }
        public bool IsTimeRecord { get { return _levelTime <= _bestTime; } }
    }
}
