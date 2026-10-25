using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Sluggity.Core
{
    public class SceneData
    {
        private static readonly JsonSerializerOptions s_jsonOptions = new JsonSerializerOptions
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        public string SceneName { get; set; }
        public int[] Size { get; set; }
        public List<GameObjectData> GameObjectsData { get; set; } = new List<GameObjectData>();

        public static SceneData Load(string path)
        {
            return JsonSerializer.Deserialize<SceneData>(File.ReadAllText(path), s_jsonOptions);
        }
    }

    public class GameObjectData
    {
        public string ObjectType { get; set; }
        public int[] Position { get; set; }
        public int[] Size { get; set; }
        public int[] Move { get; set; }
    }
}
