using AgistForms.Assets.Scripts.LevelEditor;
using AgistForms.Assets.Scripts.Structs;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AgistForms.Assets.Scripts.IO
{
    public delegate EditorShape CreateEditorShapeDelegate(int type);
    public delegate EditorTargetShape CreateEditorTargetShapeDelegate(int type);
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

        public static EditorFile FromJson(string json, PlayerEditorShape playeShape, CreateEditorShapeDelegate newShapeAction, CreateEditorTargetShapeDelegate newTargetAction)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<SaveFileFields, string>>(json);

            var result = new EditorFile(dict[SaveFileFields.LevelName], playeShape);
            result.PlayerEditorShape.SetSavedState(JsonConvert.DeserializeObject<ObjectSaveState>(dict[SaveFileFields.PlayerShape]));

            var freeShapesList = JsonConvert.DeserializeObject<List<ObjectSaveState>>(dict[SaveFileFields.FreeShapesList]);
            foreach (var freeSerialized in freeShapesList)
            {
                var newShape = newShapeAction((int)freeSerialized.StartingShapeType);
                newShape.SetSavedState(freeSerialized);
                result.FreeShapes.Add(newShape);
            }

            var targetSerializedList = JsonConvert.DeserializeObject<List<ObjectSaveState>>(dict[SaveFileFields.TargetShapes]);
            foreach (var targetSerialized in targetSerializedList)
            {
                var newShape = newTargetAction((int)targetSerialized.StartingShapeType);
                newShape.SetSavedState(targetSerialized);
                result.TargetShapes.Add(newShape);
            }

            return result;
        }
    }

    enum SaveFileFields
    {
        LevelName, FreeShapesList, PlayerShape, TargetShapes
    }
}
