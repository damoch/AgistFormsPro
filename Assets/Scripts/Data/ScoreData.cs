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
        private int _bestTime;

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

        public int BestTime
        {
            get
            {
                return _bestTime;
            }
        }

        public int LowestShapeShifts
        {
            get
            {
                return _lowestShapeShifts;
            }
        }

        public bool ShapeShiftsRecord { get { return _shapeShifts <= _lowestShapeShifts; } }
        public bool IsTimeRecord { get { return _levelTime <= _bestTime; } }
    }
}
