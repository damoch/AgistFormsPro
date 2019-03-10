using System.Linq;
using System.IO;
using UnityEngine;

namespace AgistForms.Assets.Scripts.IO
{
    public class EditorIOManager : MonoBehaviour
    {
        [SerializeField]
        private string _levelsFolderName;

        [SerializeField]
        private string _levelFileExt;

        private string _levelsSaveLocation;

        public void Init()
        {
            _levelsSaveLocation = Application.dataPath + Path.DirectorySeparatorChar + _levelsFolderName;

            if (!Directory.Exists(_levelsSaveLocation))
            {
                Directory.CreateDirectory(_levelsSaveLocation);
            }
        }

        public void SaveLevelData(EditorFile file)
        {
            var fileName = _levelsSaveLocation + Path.DirectorySeparatorChar + file.LevelName + _levelFileExt;
            File.WriteAllText(fileName, file.Serialize());
        }

        public string[] GetSavedLevels()
        {
            if (!Directory.Exists(_levelsSaveLocation))
            {
                return new string[0];
            }
            var files =  Directory.GetFiles(_levelsSaveLocation, "*" + _levelFileExt);
            var result = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                result[i] = Path.GetFileNameWithoutExtension(files[i]);
            }
            return result;
        }
    }
}
