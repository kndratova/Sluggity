using System.Linq;

namespace Sluggity.Core
{
    public class LevelInfo
    {
        public LevelInfo(string id, int number, SceneData data)
        {
            Id = id;
            Number = number;
            Data = data;
            PearlCount = data.GameObjectsData.Count(o => o.ObjectType == "Pearl");
        }

        public string Id { get; }
        public int Number { get; }
        public SceneData Data { get; }
        public string Name => Data.SceneName;
        public int PearlCount { get; }
    }
}
