using AgistForms.Assets.Scripts.Enums;
using AgistForms.Assets.Scripts.LevelEditor;
using AgistForms.Assets.Scripts.Structs;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AgistForms.Assets.Scripts.IO
{
    public delegate EditorShape CreateEditorShapeDelegate(int type);
    public delegate EditorTargetShape CreateEditorTargetShapeDelegate(int type);
    public delegate EditorBlockerShape CreateEditorBlockerShapeDelegate(int type);
    public class EditorFile
    {
        public string LevelName { get; set; }

        public List<EditorShape> FreeShapes { get; set; }
        public PlayerEditorShape PlayerEditorShape { get; set; }
        public List<EditorTargetShape> TargetShapes { get; set; }
        public List<EditorBlockerShape> BlockerShapes { get; set; }
        public int EditorVersion { get; set; }

        public EditorFile(string fileName, PlayerEditorShape _shape, int version)
        {
            LevelName = fileName;
            FreeShapes = new List<EditorShape>();
            TargetShapes = new List<EditorTargetShape>();
            BlockerShapes = new List<EditorBlockerShape>();
            PlayerEditorShape = _shape;
            EditorVersion = version;
        }

        public string Serialize()
        {
            var result = new Dictionary<SaveFileFields, string>
            {
                { SaveFileFields.LevelName, LevelName },
                {SaveFileFields.EditorVersion, EditorVersion.ToString() }
            };

            var shapesSerialized = new List<ObjectSaveState>();
            var targetsSerialized = new List<ObjectSaveState>();
            var blockerSerialized = new List<ObjectSaveState>();

            foreach(var shape in FreeShapes)
            {
                shapesSerialized.Add(shape.GetSaveState());
            }

            foreach (var target in TargetShapes)
            {
                targetsSerialized.Add(target.GetSaveState());
            }

            foreach (var target in BlockerShapes)
            {
                blockerSerialized.Add(target.GetSaveState());
            }

            result.Add(SaveFileFields.FreeShapesList, 
                       JsonConvert.SerializeObject(shapesSerialized));

            result.Add(SaveFileFields.TargetShapes, 
                       JsonConvert.SerializeObject(targetsSerialized));

            result.Add(SaveFileFields.BlockerShapes,
                       JsonConvert.SerializeObject(blockerSerialized));

            result.Add(SaveFileFields.PlayerShape, JsonConvert.SerializeObject(PlayerEditorShape.GetSaveState()));

            return JsonConvert.SerializeObject(result);
        }

        public static EditorFile FromJson(string json, PlayerEditorShape playeShape, CreateEditorShapeDelegate newShapeAction, CreateEditorTargetShapeDelegate newTargetAction, 
            CreateEditorBlockerShapeDelegate newBlockerAction, int currentVersion)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<SaveFileFields, string>>(json);
            var fileVersion = int.Parse(dict[SaveFileFields.EditorVersion]);
            //TODO: uncomment, when new version is released, and compatibility fix is needed
            //if (fileVersion < currentVersion)
            //{
                
            //}
            var result = new EditorFile(dict[SaveFileFields.LevelName], playeShape, fileVersion);
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
            var blockerSerializedList = JsonConvert.DeserializeObject<List<ObjectSaveState>>(dict[SaveFileFields.BlockerShapes]);
            foreach (var blockerSerialized in blockerSerializedList)
            {
                var newShape = newBlockerAction((int)blockerSerialized.StartingShapeType);
                newShape.SetSavedState(blockerSerialized);
                result.BlockerShapes.Add(newShape);
            }

            return result;
        }
    }
}
