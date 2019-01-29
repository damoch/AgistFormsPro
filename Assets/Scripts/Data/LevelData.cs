using AgistForms.Assets.Scripts.Game;
using System.Collections.Generic;
using UnityEngine;

namespace AgistForms.Assets.Scripts.Data
{
    public class LevelData : MonoBehaviour
    {
        [SerializeField]
        private List<FreeShape> _shapes;

        [SerializeField]
        private List<TargetShape> _targetShapes;

        [SerializeField]
        private Player _player;

        [SerializeField]
        private List<BlockerShape> _blockerShapes;

        [SerializeField]
        private List<Border> _borders;

        public List<FreeShape> Shapes
        {
            get
            {
                return _shapes;
            }
        }

        public List<TargetShape> TargetShapes
        {
            get
            {
                return _targetShapes;
            }
        }

        public Player Player
        {
            get
            {
                return _player;
            }
        }

        public List<BlockerShape> BlockerShapes
        {
            get
            {
                return _blockerShapes;
            }
        }
    }
}
