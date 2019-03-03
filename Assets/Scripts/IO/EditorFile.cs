using AgistForms.Assets.Scripts.LevelEditor;
using AgistForms.Assets.Scripts.Structs;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AgistForms.Assets.Scripts.IO
{
    public class EditorFile
    {
        public string LevelName { get; set; }

        public List<EditorShape> FreeShapes { get; set; }

        public EditorFile(string fileName)
        {
            LevelName = fileName;
            FreeShapes = new List<EditorShape>();
        }

        public string Serialize()
        {
            var result = new Dictionary<SaveFileFields, string>
            {
                { SaveFileFields.LevelName, LevelName }
            };

            var shapesSerialized = new List<ObjectSaveState>();

            foreach(var shape in FreeShapes)
            {
                shapesSerialized.Add(shape.GetSaveState());
            }
            result.Add(SaveFileFields.FreeShapesList, 
                       JsonConvert.SerializeObject(shapesSerialized));

            return JsonConvert.SerializeObject(result);
        }
    }

    enum SaveFileFields
    {
        LevelName, FreeShapesList
    }
}
