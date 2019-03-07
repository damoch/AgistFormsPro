using AgistForms.Assets.Scripts.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AgistForms.Assets.Scripts.IO
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField]
        private string _hiScoresFileName;

        [SerializeField]
        private List<SavedScore> _savedScores;
        private string _hiScoresPath;

        public void Init()
        {
            _hiScoresPath = Application.dataPath + Path.DirectorySeparatorChar + _hiScoresFileName;
            LoadSavedScores();
        }

        private void LoadSavedScores()
        {
            _savedScores = new List<SavedScore>();
            if (!File.Exists(_hiScoresPath) || Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return;
            }
            var lines = File.ReadAllLines(_hiScoresPath);

            foreach (var json in lines)
            {
                _savedScores.Add(JsonUtility.FromJson<SavedScore>(json));
            }
        }

        public void LoadHiScore(ScoreData data)
        {
            var existing = _savedScores.FirstOrDefault(x => x.LevelName == data.LevelName);
            if(existing == null)
            {
                return;
            }
            data.BestTime = existing.Time;
            data.LowestShapeShifts = existing.ShiftsCount;
        }

        public void SaveHiScore(ScoreData data)
        {
            if(Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return;
            }
            var existing = _savedScores.FirstOrDefault(x => x.LevelName == data.LevelName);
            if (existing != null)
            {
                _savedScores.Remove(existing);
            }
            _savedScores.Add(new SavedScore()
            {
                LevelName = data.LevelName,
                ShiftsCount = data.LowestShapeShifts,
                Time = data.BestTime
            });
            var list = new List<string>();

            foreach (var sa in _savedScores)
            {
                list.Add(JsonUtility.ToJson(sa));
            }

            File.WriteAllLines(_hiScoresPath, list.ToArray());
        }
    }

    public class SavedScore
    {
        public float Time;
        public int ShiftsCount;
        public string LevelName;
    }
}
