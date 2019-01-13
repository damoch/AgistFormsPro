using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.Structs;
using Assets.Scripts.Game;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgistForms.Assets.Scripts.Game
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        private List<FreeShape> _shapes;

        [SerializeField]
        private List<GameplayRule> _gameplayRulesList;

        [SerializeField]
        private Player _player;

        private Dictionary<ShapeType, Dictionary<ShapeType, CollisionResult>> _gameplayRules;

        private void Start()
        {
            GenerateGameplayRules();
            InjectControllerInShapes();
            UpdateAllShapes(_player.ShapeType);
        }

        private void GenerateGameplayRules()
        {
            _gameplayRules = new Dictionary<ShapeType, Dictionary<ShapeType, CollisionResult>>();
            foreach(var rule in _gameplayRulesList)
            {
                if (!_gameplayRules.Keys.Contains(rule.PlayerShape))
                {
                    _gameplayRules.Add(rule.PlayerShape, new Dictionary<ShapeType, CollisionResult>());
                }

                _gameplayRules[rule.PlayerShape].Add(rule.FreeShape, rule.CollisionResult);
            }
        }

        private void InjectControllerInShapes()
        {
            foreach(var shape in _shapes)
            {
                shape.LevelController = this;
            }
        }

        public void UpdateAllShapes(ShapeType playerShapeType)
        {
            foreach(var shape in _shapes)
            {
                shape.CollisionResult = _gameplayRules[playerShapeType][shape.ShapeType];
            }
        }
    }
}
