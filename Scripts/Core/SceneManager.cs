using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Sluggity.GameObjects;
using System.Windows.Controls;
using Sluggity.GameObjects.Bonuses;
using Sluggity.GameObjects.Enemies;
using Sluggity.Pages;
using System.Windows;

namespace Sluggity.Core
{
    public class SceneData
    {
        public string SceneName { get; set; }
        public List<GameObjectData> GameObjectsData { get; set; } = [];
    }

    public class Scene
    {
        public string SceneName { get; set; }
        public List<GameObject> GameObjects { get; set; } = [];
    }

    public class GameObjectData
    {
        public string ObjectType { get; set; }
        public int[] Position { get; set; }
        public int[] Size { get; set; }
        public byte[] Color { get; set; }
    }

    public static class SceneManager
    {
        private const string ScenesDirectory = "Scenes";
        private static Canvas _gameCanvas;
        private static readonly string[] SceneLoadOrderByName = ["Scene_0", "Scene_1"];
        private static Scene _currentScene;

        public static List<GameObject> SceneGameObjects { get; private set; } = [];
        public static void Construct(Canvas gameCanvas)
        {
            _gameCanvas = gameCanvas;
        }

        public static void ChangeScene(string sceneName)
        {
            _gameCanvas.Children.Clear();
            var jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ScenesDirectory, $"{sceneName}.json");
            Console.WriteLine( jsonFilePath );

            if (File.Exists(jsonFilePath))
            {
                var jsonContent = File.ReadAllText(jsonFilePath);
                var sceneData = JsonConvert.DeserializeObject<SceneData>(jsonContent);
                var scene = new Scene { SceneName = sceneData.SceneName };

                foreach (var gameObjectData in sceneData.GameObjectsData)
                {
                    GameObject gameObject = gameObjectData.ObjectType switch
                    {
                        "Player" => new Player
                        {
                            SelfCollider = { },
                            X = gameObjectData.Position[0],
                            Y = gameObjectData.Position[1],
                            Width = gameObjectData.Size[0],
                            Height = gameObjectData.Size[1],
                            ColorData = gameObjectData.Color
                        },
                        "Crub" => new Crub((gameObjectData.Position[0], gameObjectData.Position[1]))
                        {
                            SelfCollider = { },
                            X = gameObjectData.Position[0],
                            Y = gameObjectData.Position[1],
                            Width = gameObjectData.Size[0],
                            Height = gameObjectData.Size[1],
                            ColorData = gameObjectData.Color
                        },
                        "Octopus" => new Octopus((gameObjectData.Position[0], gameObjectData.Position[1]))
                        {
                            SelfCollider = { },
                            X = gameObjectData.Position[0],
                            Y = gameObjectData.Position[1],
                            Width = gameObjectData.Size[0],
                            Height = gameObjectData.Size[1],
                            ColorData = gameObjectData.Color
                        },
                        "SceneExit" => new SceneExit((gameObjectData.Position[0], gameObjectData.Position[1]))
                        {
                            SelfCollider = { },
                            X = gameObjectData.Position[0],
                            Y = gameObjectData.Position[1],
                            Width = gameObjectData.Size[0],
                            Height = gameObjectData.Size[1],
                            ColorData = gameObjectData.Color
                        },
                        "Obstacle" => new Obstacle((gameObjectData.Position[0], gameObjectData.Position[1]))
                        {
                            SelfCollider = { },
                            X = gameObjectData.Position[0],
                            Y = gameObjectData.Position[1],
                            Width = gameObjectData.Size[0],
                            Height = gameObjectData.Size[1],
                            ColorData = gameObjectData.Color
                        },
                        "Decoration" => new Decoration
                        {
                            X = gameObjectData.Position[0],
                            Y = gameObjectData.Position[1],
                            Width = gameObjectData.Size[0],
                            Height = gameObjectData.Size[1],
                            ColorData = gameObjectData.Color
                        },
                        "Pearl" => new Pearl()
                        {
                            X = gameObjectData.Position[0],
                            Y = gameObjectData.Position[1],
                            Width = gameObjectData.Size[0],
                            Height = gameObjectData.Size[1],
                            ColorData = gameObjectData.Color
                        },
                        _ => null
                    };

                    if (gameObject != null)
                        scene.GameObjects.Add(gameObject);
                }

                _currentScene = scene;
            }
            else
            {
                Console.Write("JSON file not found: " + jsonFilePath);
            }

            SceneGameObjects = _currentScene.GameObjects;
            Console.Write(SceneGameObjects.Count);
        }

        public static void LoadNextScene()
        {
            var sceneIndex = _currentScene != null ? Array.IndexOf(SceneLoadOrderByName, _currentScene.SceneName) : -1;
            sceneIndex = (sceneIndex + 1) % SceneLoadOrderByName.Length;
            Console.WriteLine("Scene Index " + sceneIndex);
            ChangeScene(SceneLoadOrderByName[sceneIndex]);
        }

        public static void ReloadCurrentScene()
        {
            ChangeScene(_currentScene.SceneName);
        }

        public static void LoadLevelSelection()
        {
            Frame mainFrame = Application.Current.MainWindow?.FindName("MainFrame") as Frame;
            if (mainFrame != null)
            {
                _gameCanvas.Children.Clear();
                mainFrame.Navigate(new LevelSelectionPage());
            }
        }
    }
}
