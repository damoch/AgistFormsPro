using AgistForms.Assets.Scripts.Data;
using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.Structs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgistForms.Assets.Scripts.Game
{
    [RequireComponent(typeof(LevelData))]
    public class LevelController : MonoBehaviour
    {
        //[SerializeField]
        //private List<FreeShape> _shapes;

        //[SerializeField]
        //private List<TargetShape> _targetShapes;

        [SerializeField]
        private List<GameplayRule> _gameplayRulesList;

        //[SerializeField]
        //private Player _player;

        [SerializeField]
        private GameState _gameState;

        [SerializeField]
        private KeyCode _restartKeyCode;

        //[SerializeField]
        //private List<BlockerShape> _blockerShapes;

        private Dictionary<ShapeType, Dictionary<ShapeType, CollisionResult>> _gameplayRules;
        private Dictionary<MonoBehaviour, ObjectSaveState> _startLevelState;
        private LevelData _levelData;

        private void Start()
        {
            _levelData = GetComponent<LevelData>();
            GenerateGameplayRules();
            InjectControllerInShapes();
            UpdateAllShapes(_levelData.Player.ShapeType);

            SaveLevelState();
            StartNewGame();
        }

        private void SaveLevelState()
        {
            _startLevelState = new Dictionary<MonoBehaviour, ObjectSaveState>();

            var playerState = new ObjectSaveState
            {
                StartingPosition = _levelData.Player.transform.position,
                StartingShapeType = _levelData.Player.ShapeType
            };
            _startLevelState.Add(_levelData.Player, playerState);

            foreach (var shape in _levelData.Shapes)
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
            _levelData.Player.gameObject.SetActive(true);

            LoadSavedState();
        }

        private void LoadSavedState()
        {
            var playerState = _startLevelState[_levelData.Player];
            _levelData.Player.transform.position = playerState.StartingPosition;
            _levelData.Player.ShapeType = playerState.StartingShapeType;

            foreach (var shape in _levelData.Shapes)
            {
                var shapeState = _startLevelState[shape];

                shape.transform.position = shapeState.StartingPosition;
                shape.ShapeType = shapeState.StartingShapeType;

                shape.AddForces();
            }

            foreach(var shape in _levelData.BlockerShapes)
            {
                shape.gameObject.SetActive(true);
            }

            UpdateAllShapes(_levelData.Player.ShapeType);
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
            foreach(var shape in _levelData.Shapes)
            {
                shape.LevelController = this;
            }

            foreach (var shape in _levelData.TargetShapes)
            {
                shape.LevelController = this;
            }
        }

        public void UpdateAllShapes(ShapeType playerShapeType)
        {
            foreach(var shape in _levelData.Shapes)
            {
                shape.CollisionResult = _gameplayRules[playerShapeType][shape.ShapeType];
            }
        }

        public void SetGameOver()
        {
            _gameState = GameState.GameOver;
            _levelData.Player.gameObject.SetActive(false);
        }

        public void CheckTargetSprites()
        {
            foreach(var shape in _levelData.TargetShapes)
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
            foreach (var shape in _levelData.Shapes)
            {
                shape.gameObject.SetActive(false);
            }
        }

    }
}
