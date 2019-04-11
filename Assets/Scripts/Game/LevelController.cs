using AgistForms.Assets.Scripts.Data;
using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.IO;
using AgistForms.Assets.Scripts.Structs;
using AgistForms.Assets.Scripts.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AgistForms.Assets.Scripts.Game
{
    [RequireComponent(typeof(LevelData), typeof(UIController), typeof(ScoreData))]
    [RequireComponent(typeof(SaveManager))]
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        private List<GameplayRule> _gameplayRulesList;

        [SerializeField]
        private GameState _gameState;

        [SerializeField]
        private KeyCode _restartKeyCode;

        [SerializeField]
        private KeyCode _confirmNextLevelKeyCode;

        [SerializeField]
        private KeyCode _pauseKeyCode;

        [SerializeField]
        private string _mainMenuScene;

        [SerializeField]
        private GameDifficultyLevel _difficultyLevel;

        private Dictionary<ShapeType, Dictionary<ShapeType, CollisionResult>> _gameplayRules;
        private Dictionary<MonoBehaviour, ObjectSaveState> _startLevelState;
        private LevelData _levelData;
        private UIController _uiController;
        private ScoreData _scoreData;
        private SaveManager _saveManager;
        private float _defaultGameSpeed;
        private DynamicLevelLoader _levelLoader;

        public GameDifficultyLevel DifficultyLevel
        {
            get
            {
                return _difficultyLevel;
            }
        }

        private void Start()
        {
            _uiController = GetComponent<UIController>();
            _levelData = GetComponent<LevelData>();
            _scoreData = GetComponent<ScoreData>();
            _saveManager = GetComponent<SaveManager>();
            _levelLoader = GetComponent<DynamicLevelLoader>();

            SetDifficultyLevel();
            _saveManager.Init();
            _scoreData.LevelName = SceneManager.GetActiveScene().name;
            _saveManager.LoadHiScore(_scoreData);
            _uiController.SetPauseMessage(_confirmNextLevelKeyCode.ToString(), _pauseKeyCode.ToString(), _restartKeyCode.ToString());
            GenerateGameplayRules();
            InjectControllerInShapes();
            UpdateAllShapes(_levelData.Player.ShapeType);

            SaveLevelState();
            StartNewGame();
        }

        private void SetDifficultyLevel()
        {
            if (PlayerPrefs.HasKey(typeof(GameDifficultyLevel).ToString()))
            {
                _difficultyLevel = (GameDifficultyLevel)PlayerPrefs.GetInt(typeof(GameDifficultyLevel).ToString());
            }
            else
            {
                _difficultyLevel = GameDifficultyLevel.Normal;
            }
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
            switch (_gameState)
            {
                case GameState.GamePlaying:
                    _scoreData.LevelTime += Time.deltaTime;
                    _uiController.TimeText = _scoreData.LevelTime.ToString(_uiController.TimeDisplayFormat);
                    if (Input.GetKey(_pauseKeyCode))
                    {
                        EnablePause(true);
                    }
                    break;
                case GameState.GameOver:
                    if (Input.GetKey(_restartKeyCode))
                    {
                        StartNewGame();
                    }
                    break;
                case GameState.LevelCompleted:
                    if (Input.GetKey(_confirmNextLevelKeyCode))
                    {
                        if(_levelLoader != null)
                        {
                            _levelLoader.ExitLevel();
                            return;
                        }
                        GoToNextLevel();
                    }
                    break;
                case GameState.Pause:
                    if (Input.GetKey(_restartKeyCode))
                    {
                        StartNewGame();
                        EnablePause(false);
                    }
                    if (Input.GetKey(_confirmNextLevelKeyCode))
                    {
                        EnablePause(false);
                    }
                    if (Input.GetKeyDown(_pauseKeyCode))
                    {
                        ExitLevel();
                    }

                    break;
            }

        }

        private void ExitLevel()
        {
            Time.timeScale = _defaultGameSpeed;
            if (_levelLoader != null)
            {
                _levelLoader.ExitLevel();
                return;
            }
            SceneManager.LoadScene(_mainMenuScene);
        }

        private void EnablePause(bool enable)
        {
            _uiController.EnablePausePanel(enable);
            if (enable)
            {
                _defaultGameSpeed = Time.timeScale;
                Time.timeScale = 0;
                _gameState = GameState.Pause;
            }
            else
            {
                Time.timeScale = _defaultGameSpeed;
                _gameState = GameState.GamePlaying;
            }
        }

        private void GoToNextLevel()
        {
            if (_levelLoader != null)
            {
                _levelLoader.ExitLevel();
                return;
            }
            if (!string.IsNullOrEmpty(_levelData.NextLevelName))
            {
                SceneManager.LoadScene(_levelData.NextLevelName);
            }
        }

        private void StartNewGame()
        {
            if(_gameState == GameState.GamePlaying)
            {
                return;
            }
            _uiController.EnableParTexts(false);
            _uiController.SetEndLevelText(false);
            _gameState = GameState.GamePlaying;
            _levelData.Player.gameObject.SetActive(true);
            _scoreData.LevelTime = 0;
            UpdateShapeShiftsText(0);
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

            foreach (var item in _levelData.TargetShapes)
            {
                item.SuccesfullCollison = false;
            }

            foreach (var shape in _levelData.BlockerShapes)
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

        private void UpdateAllShapes(ShapeType playerShapeType)
        {
            foreach(var shape in _levelData.Shapes)
            {
                shape.CollisionResult = _gameplayRules[playerShapeType][shape.ShapeType];
            }
        }

        public void SetGameOver()
        {
            _gameState = GameState.GameOver;
            _uiController.SetEndLevelText(true, false, _restartKeyCode.ToString());
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
            _uiController.SetEndLevelText(true, true, _confirmNextLevelKeyCode.ToString());
            foreach (var shape in _levelData.Shapes)
            {
                shape.gameObject.SetActive(false);
            }
            _levelData.Player.gameObject.SetActive(false);
            HandleScores();
        }

        private void HandleScores()
        {
            _uiController.EnableParTexts(true);
            if (_scoreData.ShapeShiftsRecord)
            {
                _scoreData.LowestShapeShifts = _scoreData.ShapeShifts;
                _uiController.SetShapeShiftsPar(_scoreData.ShapeShifts, true);
            }
            else
            {
                _uiController.SetShapeShiftsPar(_scoreData.LowestShapeShifts, false);
            }

            if (_scoreData.IsTimeRecord)
            {
                _scoreData.BestTime = _scoreData.LevelTime;
                _uiController.SetTimePar(_scoreData.LevelTime, true);
            }
            else
            {
                _uiController.SetTimePar(_scoreData.BestTime, false);
            }
            if(_scoreData.IsTimeRecord || _scoreData.ShapeShiftsRecord)
            {
                _saveManager.SaveHiScore(_scoreData);
            }

        }

        public void ShapeShift(ShapeType playerShapeType)
        {
            UpdateShapeShiftsText(++_scoreData.ShapeShifts);
            UpdateAllShapes(playerShapeType);
        }

        private void UpdateShapeShiftsText(int newCount)
        {
            _scoreData.ShapeShifts = newCount;
            _uiController.ShapeShiftsText = newCount.ToString();
        }

    }
}
