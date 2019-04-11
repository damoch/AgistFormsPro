using System.Collections.Generic;
using AgistForms.Assets.Scripts.Data;
using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.IO;
using AgistForms.Assets.Scripts.Structs;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AgistForms.Assets.Scripts.Game
{
    [RequireComponent(typeof(LevelData), typeof(LevelController))]
    public class DynamicLevelLoader : MonoBehaviour
    {
        private LevelData _levelData;
        private LevelController _levelController;
        private DynamicLevelLoaderOption _dynamicLevelLoaderOption;

        [SerializeField]
        private GameObject _freeShapePrefab;

        [SerializeField]
        private GameObject _targetShapePrefab;

        [SerializeField]
        private GameObject _blockerShapePrefab;

        [SerializeField]
        private EditorIOManager _ioManager;

        [SerializeField]
        private string _levelEditorSceneName;

        [SerializeField]
        private string _mainMenuSceneName;

        private void Start()
        {
            _levelData = GetComponent<LevelData>();
            _levelController = GetComponent<LevelController>();

            _dynamicLevelLoaderOption = (DynamicLevelLoaderOption)PlayerPrefs.GetInt(typeof(DynamicLevelLoaderOption).Name);

            _ioManager.Init();
            if(_dynamicLevelLoaderOption == DynamicLevelLoaderOption.LoadTempData)
            {
                LoadLevel(_ioManager.GetTempLevelData());
            }
            else if(_dynamicLevelLoaderOption == DynamicLevelLoaderOption.LoadLevelName)
            {
                var levelName = PlayerPrefs.GetString(DynamicLevelLoaderOption.LoadLevelName.ToString());
                LoadLevel(_ioManager.GetLevelData(levelName));
                PlayerPrefs.DeleteKey(DynamicLevelLoaderOption.LoadLevelName.ToString());
            }
        }

        private void LoadLevel(string json)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<SaveFileFields, string>>(json);

            _levelData.Player.SetSavedState(JsonConvert.DeserializeObject<ObjectSaveState>(dict[SaveFileFields.PlayerShape]));

            var freeShapes = JsonConvert.DeserializeObject<List<ObjectSaveState>>(dict[SaveFileFields.FreeShapesList]);
            var targetShapes = JsonConvert.DeserializeObject<List<ObjectSaveState>>(dict[SaveFileFields.TargetShapes]);
            var blockerShapes = JsonConvert.DeserializeObject<List<ObjectSaveState>>(dict[SaveFileFields.BlockerShapes]);

            foreach (var shape in freeShapes)
            {
                var newShape = Instantiate(_freeShapePrefab).GetComponent<FreeShape>();
                newShape.SetSavedState(shape);
                _levelData.Shapes.Add(newShape);
            }

            foreach (var shape in targetShapes)
            {
                var newShape = Instantiate(_targetShapePrefab).GetComponent<TargetShape>();
                newShape.SetSavedState(shape);
                _levelData.TargetShapes.Add(newShape);
            }

            foreach (var shape in blockerShapes)
            {
                var newShape = Instantiate(_blockerShapePrefab).GetComponent<BlockerShape>();
                newShape.SetSavedState(shape);
                _levelData.BlockerShapes.Add(newShape);
            }

            _levelController.enabled = true;
        }

        public void ExitLevel()
        {
            if(_dynamicLevelLoaderOption == DynamicLevelLoaderOption.LoadTempData)
            {
                PlayerPrefs.SetInt(typeof(LevelEditorStartupOption).Name, (int)LevelEditorStartupOption.LoadTemp);
                SceneManager.LoadScene(_levelEditorSceneName);
            }
            else
            {
                SceneManager.LoadScene(_mainMenuSceneName);
            }
        }
    }
}
