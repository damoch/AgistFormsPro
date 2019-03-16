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
        private string _testFileName;

        [SerializeField]
        private GameObject _freeShapePrefab;

        [SerializeField]
        private GameObject _targetShapePrefab;

        [SerializeField]
        private EditorIOManager _ioManager;

        [SerializeField]
        private string _levelEditorSceneName;

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
            else
            {
                LoadLevel(_ioManager.GetLevelData(_testFileName));
            }
        }

        private void LoadLevel(string json)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<SaveFileFields, string>>(json);

            _levelData.Player.SetSavedState(JsonConvert.DeserializeObject<ObjectSaveState>(dict[SaveFileFields.PlayerShape]));

            var freeShapes = JsonConvert.DeserializeObject<List<ObjectSaveState>>(dict[SaveFileFields.FreeShapesList]);
            var targetShapes = JsonConvert.DeserializeObject<List<ObjectSaveState>>(dict[SaveFileFields.TargetShapes]);

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

            _levelController.enabled = true;
        }

        public void GoBackToEditor()
        {
            PlayerPrefs.SetInt(typeof(LevelEditorStartupOption).Name, (int)LevelEditorStartupOption.LoadTemp);
            SceneManager.LoadScene(_levelEditorSceneName);
        }
    }
}
