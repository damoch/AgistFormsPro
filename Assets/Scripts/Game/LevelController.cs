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
        private List<TargetShape> _targetShapes;

        [SerializeField]
        private List<GameplayRule> _gameplayRulesList;

        [SerializeField]
        private Player _player;

        [SerializeField]
        private GameState _gameState;

        [SerializeField]
        private KeyCode _restartKeyCode;

        [SerializeField]
        private List<BlockerShape> _blockerShapes;

        private Dictionary<ShapeType, Dictionary<ShapeType, CollisionResult>> _gameplayRules;
        private Dictionary<MonoBehaviour, ObjectSaveState> _startLevelState;

        private void Start()
        {
            GenerateGameplayRules();
            InjectControllerInShapes();
            UpdateAllShapes(_player.ShapeType);

            SaveLevelState();
            StartNewGame();
        }

        private void SaveLevelState()
        {
            _startLevelState = new Dictionary<MonoBehaviour, ObjectSaveState>();

            var playerState = new ObjectSaveState
            {
                StartingPosition = _player.transform.position,
                StartingShapeType = _player.ShapeType
            };
            _startLevelState.Add(_player, playerState);

            foreach (var shape in _shapes)
            {
                _startLevelState.Add(shape, new ObjectSaveState
                {
                    StartingPosition = shape.transform.position,
                    StartingShapeType = shape.ShapeType
                });
            }
        }

        private void Update()
        {
            if(_gameState == GameState.GameOver)
            {
                if (Input.GetKey(_restartKeyCode))
                {
                    StartNewGame();
                }
            }
        }

        private void StartNewGame()
        {
            _gameState = GameState.GamePlaying;
            _player.gameObject.SetActive(true);

            LoadSavedState();
        }

        private void LoadSavedState()
        {
            var playerState = _startLevelState[_player];
            _player.transform.position = playerState.StartingPosition;
            _player.ShapeType = playerState.StartingShapeType;

            foreach (var shape in _shapes)
            {
                var shapeState = _startLevelState[shape];

                shape.transform.position = shapeState.StartingPosition;
                shape.ShapeType = shapeState.StartingShapeType;

                shape.AddForces();
            }

            foreach(var shape in _blockerShapes)
            {
                shape.gameObject.SetActive(true);
            }

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

            foreach (var shape in _targetShapes)
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

        public void SetGameOver()
        {
            _gameState = GameState.GameOver;
            _player.gameObject.SetActive(false);
        }

        public void CheckTargetSprites()
        {
            foreach(var shape in _targetShapes)
            {
                if (!shape.SuccesfullCollison)
                {
                    return;
                }
            }
            SetLevelCompleted();
        }

        private void SetLevelCompleted()
        {
            _gameState = GameState.LevelCompleted;
            foreach (var shape in _shapes)
            {
                shape.gameObject.SetActive(false);
            }
        }

    }
}
