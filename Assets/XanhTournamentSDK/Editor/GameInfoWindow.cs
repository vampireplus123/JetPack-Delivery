#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace XanhTournamentSDK
{
    [InitializeOnLoad]
    public class GameInfoWindow : EditorWindow
    {
        private string gameId = "";
        private Vector2 scrollPosition;

        private bool isLoading = false;
        private bool justShowWindow = false;
        private bool justLoadGameData = false;

        private float rotationAngle = 0f;
        private DateTime lastUpdateTime;

        private GUIStyle boardStyle;
        private GUIStyle headerStyle;
        private GUIStyle centerTextStyle;
        private GUIStyle errorMessageStyle;
        private GUIStyle placeholderTextStyle;
        private bool isInitializeEditorStyles;

        private DataContainer dataContainer;

        private const float CIRCLE_RADIUS = 15f;
        private const float ROTATION_SPEED = 200f;
        private const float CIRCLE_THICKNESS = 2f;

        private async void IntializeEditorStyles()
        {
            isInitializeEditorStyles = true;

            // Initialize styles
            while (EditorStyles.textField == null)
            {
                await System.Threading.Tasks.Task.Delay(10);
            }
            centerTextStyle = new GUIStyle(EditorStyles.textField)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14
            };

            while (EditorStyles.boldLabel == null)
            {
                await System.Threading.Tasks.Task.Delay(10);
            }
            headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 18
            };

            while (EditorStyles.helpBox == null)
            {
                await System.Threading.Tasks.Task.Delay(10);
            }
            boardStyle = new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(10, 10, 10, 10)
            };

            while (EditorStyles.label == null)
            {
                await System.Threading.Tasks.Task.Delay(10);
            }
            errorMessageStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.yellow },
                fontSize = 12
            };

            while (EditorStyles.textField == null)
            {
                await System.Threading.Tasks.Task.Delay(10);
            }
            placeholderTextStyle = new GUIStyle(EditorStyles.textField)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(0.5f, 0.5f, 0.5f, 0.5f) },
                fontSize = 14
            };
        }
        private void OnEnable()
        {
            justShowWindow = true;
            EditorApplication.update += UpdateLoadingAnimation;

            LoadOrCreateGameDataContainer();
        }
        private void OnDisable()
        {
            EditorApplication.update -= UpdateLoadingAnimation;
        }
        private void OnGUI()
        {
            if (!isInitializeEditorStyles)
            {
                IntializeEditorStyles();
            }

            // Title
            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Xanh Tournament Tool", headerStyle);
            EditorGUILayout.Space(30);

            // Center area for input and button
            EditorGUILayout.BeginVertical();

            // Center the text field
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // If the field is empty, show the placeholder text in gray
            if (string.IsNullOrEmpty(gameId))
            {
                gameId = EditorGUILayout.TextField("", "Enter Game ID", placeholderTextStyle, GUILayout.Width(200));
            }
            else
            {
                gameId = EditorGUILayout.TextField(gameId, centerTextStyle, GUILayout.Width(200));
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);

            // Center the button
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (!isLoading && GUILayout.Button("Check Game Data", GUILayout.Width(200), GUILayout.Height(30)))
            {
                if (string.IsNullOrEmpty(gameId))
                {
                    EditorUtility.DisplayDialog("Error", "Please enter a Game ID", "OK");
                    return;
                }
                isLoading = true;
                lastUpdateTime = DateTime.Now;

                DataManager.Game.LoadData(gameId, (result) =>
                {
                    isLoading = false;
                    justShowWindow = false;
                    justLoadGameData = true;
                });
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(20);

            // Loading animation or content
            if (isLoading)
            {
                // Center the loading circle
                Rect loadingRect = GUILayoutUtility.GetRect(50, 50);
                DrawLoadingCircle(loadingRect);
            }
            else
            {
                if (DataManager.Game.Data != null)
                {
                    // Save data to ScriptableObject
                    if (justLoadGameData)
                    {
                        justLoadGameData = false;

                        dataContainer.GameData = DataManager.Game.Data;
                        EditorUtility.SetDirty(dataContainer);
                        AssetDatabase.SaveAssets();
                    }

                    // Game Data Display Board
                    EditorGUILayout.BeginVertical(boardStyle);

                    EditorGUILayout.LabelField("Game Data", EditorStyles.boldLabel);
                    EditorGUILayout.Space(10);

                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                    EditorGUI.BeginDisabledGroup(true); // Make the fields read-only

                    EditorGUILayout.LabelField("ID:", DataManager.Game.Data.ID);
                    EditorGUILayout.Space(5);

                    EditorGUILayout.LabelField("Name:", DataManager.Game.Data.Name);
                    EditorGUILayout.Space(5);

                    EditorGUILayout.LabelField("Supported Tournament Formats:");
                    EditorGUI.indentLevel++;
                    if (DataManager.Game.Data.SupportedTournaments != null &&
                        DataManager.Game.Data.SupportedTournaments.Length > 0)
                    {
                        foreach (var format in DataManager.Game.Data.SupportedTournaments)
                        {
                            EditorGUILayout.LabelField("• " + format);
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField("No formats available");
                    }
                    EditorGUI.indentLevel--;

                    EditorGUI.EndDisabledGroup();

                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.EndVertical();
                }
                else if (!justShowWindow)
                {
                    justLoadGameData = false;

                    // Show error message if data loading is done but no data is available
                    EditorGUILayout.LabelField("Game not found, please check again!", errorMessageStyle);
                }
            }
        }

        private void UpdateLoadingAnimation()
        {
            if (isLoading)
            {
                var currentTime = DateTime.Now;
                var deltaTime = (float)(currentTime - lastUpdateTime).TotalSeconds;
                rotationAngle += ROTATION_SPEED * deltaTime;
                lastUpdateTime = currentTime;

                Repaint();
            }
        }
        private void DrawLoadingCircle(Rect position)
        {
            var center = new Vector2(position.center.x, position.center.y);

            // Draw loading circle
            Handles.color = Color.white;
            Handles.DrawWireArc(center, Vector3.forward,
                Quaternion.Euler(0, 0, rotationAngle) * Vector3.right,
                300, // Draw only 300 degrees to create a gap
                CIRCLE_RADIUS, CIRCLE_THICKNESS);
        }
        private void LoadOrCreateGameDataContainer()
        {
            // Try to load existing container
            dataContainer = Resources.Load<DataContainer>("DataContainer");

            // If it doesn't exist, create it
            if (dataContainer == null)
            {
                // Create the directories if they don't exist
                string path = Application.dataPath;
                string directoryPath = $"{path}/XanhTournamentSDK/Resources";
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                }

                dataContainer = CreateInstance<DataContainer>();
                AssetDatabase.CreateAsset(dataContainer, $"Assets/XanhTournamentSDK/Resources/DataContainer.asset");
                AssetDatabase.SaveAssets();
            }
        }

        [MenuItem("Xanh SDK/Game Info")]
        public static void ShowWindow()
        {
            var window = GetWindow<GameInfoWindow>("Game Info");

            // Set window size and center it
            var size = new Vector2(400, 300);
            var screenCenter = new Vector2(Screen.currentResolution.width / 4, Screen.currentResolution.height / 4);

            window.position = new Rect(screenCenter.x, screenCenter.y, size.x, size.y);
        }

        static GameInfoWindow()
        {
            EditorApplication.delayCall += () =>
            {
                if (DataManager.Game.Data == null)
                {
                    ShowWindow();
                }
            };
        }
    }
}

#endif