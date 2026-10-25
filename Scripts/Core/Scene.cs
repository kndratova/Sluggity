using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Sluggity.GameObjects;
using Sluggity.GameObjects.Bonuses;
using Sluggity.GameObjects.Enemies;

namespace Sluggity.Core
{
    public enum GameState
    {
        Playing,
        Died,
        Completed,
    }

    public class Scene
    {
        private const double k_fallLimit = -300;

        private readonly Canvas m_canvas;
        private readonly List<GameObject> m_gameObjects = new List<GameObject>();
        private readonly List<Obstacle> m_obstacles = new List<Obstacle>();

        public Scene(SceneData data, Canvas canvas, GameInput input)
        {
            m_canvas = canvas;
            Input = input;
            Width = data.Size[0];
            Height = data.Size[1];

            m_canvas.Children.Clear();
            foreach (GameObjectData objectData in data.GameObjectsData)
            {
                Add(CreateGameObject(objectData));
            }

            Player = m_gameObjects.OfType<Player>().FirstOrDefault()
                     ?? throw new InvalidDataException($"No player in scene {data.SceneName}");
            Panel.SetZIndex(Player.Visual, 1);

            Draw();
        }

        public event Action<PowerUpType> PowerUpCollected;

        public GameInput Input { get; }
        public Player Player { get; }
        public double Width { get; }
        public double Height { get; }
        public List<Obstacle> Obstacles => m_obstacles;
        public int PearlsCollected { get; private set; }
        public GameState State { get; private set; } = GameState.Playing;

        public void Update(double deltaTime)
        {
            foreach (Obstacle obstacle in m_obstacles)
            {
                obstacle.Update(this, deltaTime);
            }

            foreach (GameObject gameObject in m_gameObjects)
            {
                if (gameObject is not Obstacle)
                {
                    gameObject.Update(this, deltaTime);
                }
            }

            foreach (GameObject gameObject in m_gameObjects)
            {
                if (State != GameState.Playing)
                {
                    break;
                }

                if (gameObject != Player && !gameObject.IsDestroyed && gameObject.Overlaps(Player))
                {
                    gameObject.OnPlayerTouch(Player, this);
                }
            }

            if (Player.Top < k_fallLimit)
            {
                KillPlayer();
            }

            RemoveDestroyed();
            Draw();
        }

        public bool IsSolidAt(double x, double y)
        {
            return m_obstacles.Any(o => x >= o.X && x <= o.Right && y >= o.Y && y <= o.Top);
        }

        public void CollectPearl()
        {
            PearlsCollected++;
        }

        public void CollectPowerUp(PowerUpType type)
        {
            if (type == PowerUpType.Knife)
            {
                Player.GiveKnife();
            }
            else
            {
                Player.GiveLegs();
            }

            PowerUpCollected?.Invoke(type);
        }

        public void KillPlayer()
        {
            if (State == GameState.Playing)
            {
                State = GameState.Died;
            }
        }

        public void Complete()
        {
            if (State == GameState.Playing)
            {
                State = GameState.Completed;
            }
        }

        private void Add(GameObject gameObject)
        {
            m_gameObjects.Add(gameObject);
            if (gameObject is Obstacle obstacle)
            {
                m_obstacles.Add(obstacle);
            }

            m_canvas.Children.Add(gameObject.Visual);
        }

        private void RemoveDestroyed()
        {
            foreach (GameObject gameObject in m_gameObjects.Where(o => o.IsDestroyed))
            {
                m_canvas.Children.Remove(gameObject.Visual);
            }

            m_gameObjects.RemoveAll(o => o.IsDestroyed);
        }

        private void Draw()
        {
            foreach (GameObject gameObject in m_gameObjects)
            {
                gameObject.Draw();
            }
        }

        private static GameObject CreateGameObject(GameObjectData data)
        {
            int x = data.Position[0];
            int y = data.Position[1];

            return data.ObjectType switch
            {
                "Obstacle" => new Obstacle(x, y, data.Size[0], data.Size[1]),
                "MovingObstacle" => new MovingObstacle(x, y, data.Size[0], data.Size[1], data.Move[0], data.Move[1]),
                "Spikes" => new Spikes(x, y, data.Size[0]),
                "Player" => new Player(x, y),
                "Crab" => new Crab(x, y),
                "Octopus" => new Octopus(x, y),
                "Pearl" => new Pearl(x, y),
                "Knife" => new PowerUp(x, y, PowerUpType.Knife),
                "Legs" => new PowerUp(x, y, PowerUpType.Legs),
                "SceneExit" => new SceneExit(x, y),
                _ => throw new InvalidDataException($"Unknown object type {data.ObjectType}"),
            };
        }
    }
}
