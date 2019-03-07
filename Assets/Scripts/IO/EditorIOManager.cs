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
    }
}
