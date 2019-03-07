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
        public PlayerEditorShape PlayerEditorShape { get; set; }
        public List<EditorTargetShape> TargetShapes { get; set; }

        public EditorFile(string fileName, PlayerEditorShape _shape)
        {
            LevelName = fileName;
            FreeShapes = new List<EditorShape>();
            TargetShapes = new List<EditorTargetShape>();
            PlayerEditorShape = _shape;
        }

        public string Serialize()
        {
            var result = new Dictionary<SaveFileFields, string>
            {
                { SaveFileFields.LevelName, LevelName }
            };

            var shapesSerialized = new List<ObjectSaveState>();
            var targetsSerialized = new List<ObjectSaveState>();

            foreach(var shape in FreeShapes)
            {
                shapesSerialized.Add(shape.GetSaveState());
            }

            foreach (var target in TargetShapes)
            {
                targetsSerialized.Add(target.GetSaveState());
            }

            result.Add(SaveFileFields.FreeShapesList, 
                       JsonConvert.SerializeObject(shapesSerialized));

            result.Add(SaveFileFields.TargetShapes, 
                       JsonConvert.SerializeObject(targetsSerialized));

            result.Add(SaveFileFields.PlayerShape, JsonConvert.SerializeObject(PlayerEditorShape.GetSaveState()));

            return JsonConvert.SerializeObject(result);
        }

        public static EditorFile FromJson(string json, PlayerEditorShape playeShape)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<SaveFileFields, string>>(json);

            var result = new EditorFile(dict[SaveFileFields.LevelName], playeShape);
            return result;
        }
    }

    enum SaveFileFields
    {
        LevelName, FreeShapesList, PlayerShape, TargetShapes
    }
}
