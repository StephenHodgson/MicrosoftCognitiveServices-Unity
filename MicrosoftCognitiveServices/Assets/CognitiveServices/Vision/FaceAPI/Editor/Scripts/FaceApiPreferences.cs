using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Cognitive.DataStructures.Person;
using Microsoft.Cognitive.DataStructures.PersonGroup;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Microsoft.Cognitive.Vision.Face.Editor
{
    public class FaceApiPreferences : EditorWindow
    {
        private static FaceApiPreferences window;
        private static FaceApiPreferences Window
        {
            get
            {
                return window ?? (window = ShowFaceServicesWindow());
            }
            set
            {
                window = value;
            }
        }

        [SerializeField]
        private PersonGroupInfo[] currentGroups;

        private static int groupPopupId;
        private static bool isTraining = false;
        private static bool uploadingImages = false;
        private static string newPersonName = string.Empty;
        private static string newPersonGroupName = string.Empty;
        private static Vector2 scrollPosition = Vector2.zero;

        [SerializeField]
        private Texture2D statusIcon;
        private readonly GUILayoutOption[] iconLayoutOptions21 = { GUILayout.Height(21), GUILayout.Width(21) };
        private Color defaultGUIColor;

        [MenuItem("Window/Microsoft Cognitive Services")]
        public static FaceApiPreferences ShowFaceServicesWindow()
        {
            Window = GetWindow<FaceApiPreferences>(Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll"));
            Window.titleContent = new GUIContent("Cognitive");
            Window.minSize = new Vector2(356, 256);
            return Window;
        }

        private void OnEnable()
        {
            if (currentGroups == null)
            {
                GetPersonGroupsAsync();
            }

            if (statusIcon == null)
            {
                statusIcon = LoadImageResource("circle");
            }
        }

        private void Update()
        {
            Repaint();
        }

        private void OnGUI()
        {
            defaultGUIColor = GUI.color;
            GUILayout.Space(8f);
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("Microsoft Cognitive Services", new GUIStyle("label")
                {
                    fontSize = 24,
                    alignment = TextAnchor.MiddleCenter
                }, GUILayout.Height(32));
                EditorGUILayout.LabelField("Face API Client Settings", new GUIStyle("label") { fontStyle = FontStyle.Bold });
                EditorGUI.indentLevel++;
                {
                    FaceApiClient.ApiKey = EditorGUILayout.TextField("Face API Key", FaceApiClient.ApiKey);
                    FaceApiClient.ResourceRegion = (Region)EditorGUILayout.EnumPopup("Region", FaceApiClient.ResourceRegion);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("Face API Portal", new GUIStyle("label") { fontStyle = FontStyle.Bold });

                PersonGroupGUI();

                GUILayout.Space(8f);

                NewPersonGroupGUI();

                GUILayout.Space(8f);

                NewPersonGUI();
            }
            EditorGUILayout.EndVertical();
        }

        private void PersonGroupGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh Person Groups"))
            {
                GetPersonGroupsAsync();
            }

            GUILayout.EndHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Height(128));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < currentGroups?.Length; i++)
            {
                if (i != 0)
                {
                    GUILayout.Space(8f);
                }

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();

                Color currentColor;
                switch (currentGroups[i].TrainingStatus.StatusType)
                {
                    case StatusType.succeeded:
                        currentColor = Color.green;
                        break;
                    case StatusType.failed:
                        currentColor = Color.red;
                        break;
                    case StatusType.running:
                        currentColor = Color.yellow;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                RenderIcon(new GUIContent(statusIcon, currentGroups[i].TrainingStatus.StatusType.ToString()), currentColor);
                EditorGUILayout.LabelField(currentGroups[i].name, GUILayout.Width(128));
                GUILayout.FlexibleSpace();

                GUI.enabled = !uploadingImages && !isTraining;
                if (GUILayout.Button("Train Group"))
                {
                    TrainPersonGroupAsync(currentGroups[i]);
                }

                if (GUILayout.Button(new GUIContent("X", "Delete Group"), GUILayout.Width(18)))
                {
                    DeletePersonGroupAsync(currentGroups[i]);
                }
                GUI.enabled = true;

                EditorGUILayout.EndHorizontal();
                PersonGUI(currentGroups[i]);
                EditorGUILayout.EndVertical();
            }

            GUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void PersonGUI(PersonGroupInfo group)
        {
            if (group.People == null || group.People.Length == 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(32f);
                EditorGUILayout.LabelField("No People in group");
                EditorGUILayout.EndHorizontal();
            }

            if (group.People != null)
            {
                for (int i = 0; i < group.People.Length; i++)
                {
                    var person = group.People[i];
                    string faceDirectory = Directory.GetParent(Application.dataPath) + "/FaceAPI/" + person.name;
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(32f);
                    EditorGUILayout.LabelField(group.People[i].name, GUILayout.Width(128));
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Photos Folder"))
                    {
                        Process.Start(faceDirectory);
                    }

                    GUI.enabled = !uploadingImages && !isTraining;
                    if (GUILayout.Button("Upload"))
                    {
                        UploadPhotosAsync(person, group, faceDirectory);
                    }

                    if (GUILayout.Button(new GUIContent("X", "Delete Person"), GUILayout.Width(18)))
                    {
                        DeletePersonAsync(group.personGroupId, person.personId);
                    }
                    GUI.enabled = true;

                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        private void RenderIcon(GUIContent icon, Color color)
        {
            GUI.color = color;
            EditorGUILayout.LabelField(icon, iconLayoutOptions21);
            GUI.color = defaultGUIColor;
        }

        private void NewPersonGroupGUI()
        {
            newPersonGroupName = EditorGUILayout.TextField("New Person Group", newPersonGroupName);

            GUI.enabled = !string.IsNullOrEmpty(newPersonGroupName);
            if (GUILayout.Button("Create New Person Group"))
            {
                CreatePersonGroupAsync(newPersonGroupName, currentGroups);
            }
            GUI.enabled = true;
        }

        private void NewPersonGUI()
        {
            newPersonName = EditorGUILayout.TextField("New Person", newPersonName);
            GUI.enabled = !string.IsNullOrEmpty(newPersonName) && currentGroups != null;

            string[] popupGroups;
            if (currentGroups != null)
            {
                popupGroups = new string[currentGroups.Length];
                for (int i = 0; i < currentGroups.Length; i++)
                {
                    popupGroups[i] = currentGroups[i].name;
                }
            }
            else
            {
                popupGroups = new[] { "None" };
            }

            groupPopupId = EditorGUILayout.Popup("Person Group", groupPopupId, popupGroups);

            if (GUILayout.Button("Create New Person Group"))
            {
                Debug.Assert(currentGroups != null);
                CreatePersonAsync(newPersonName, currentGroups[groupPopupId]);
            }

            GUI.enabled = true;
        }

        private static async void GetPersonGroupsAsync()
        {
            Window.currentGroups = null;
            Window.currentGroups = await PersonGroup.GetGroupListAsync();

            if (Window.currentGroups == null) { return; }

            for (int i = 0; i < Window.currentGroups.Length; i++)
            {
                if (Window.currentGroups[i].People != null)
                {
                    var status = await PersonGroup.GetTrainingStatusAsync(Window.currentGroups[i].personGroupId);
                    Window.currentGroups[i].TrainingStatus = status.Value;
                }
            }

            GetPeopleInGroupAsync();
        }

        private static async void CreatePersonGroupAsync(string personGroupName, PersonGroupInfo[] groups)
        {
            newPersonGroupName = string.Empty;

            for (var i = 0; i < groups.Length; i++)
            {
                if (groups[i].name == personGroupName)
                {
                    Debug.LogFormat("{0} already exists", personGroupName);
                    return;
                }
            }

            await PersonGroup.CreateGroupAsync(personGroupName);
            GetPersonGroupsAsync();
        }

        private static async void TrainPersonGroupAsync(PersonGroupInfo group)
        {
            await PersonGroup.TrainGroupAsync(group.personGroupId);
            isTraining = true;

            while (isTraining)
            {
                var status = await PersonGroup.GetTrainingStatusAsync(group.personGroupId);
                group.TrainingStatus = status.Value;

                for (int i = 0; i < Window.currentGroups.Length; i++)
                {
                    if (Window.currentGroups[i].personGroupId == group.personGroupId)
                    {
                        Window.currentGroups[i].TrainingStatus = group.TrainingStatus;
                        break;
                    }
                }

                switch (group.TrainingStatus.StatusType)
                {
                    case StatusType.succeeded:
                        isTraining = false;
                        break;
                    case StatusType.failed:
                        isTraining = false;
                        break;
                    case StatusType.running:
                        await Task.Delay(1000);
                        break;
                }
            }
        }

        private static async void DeletePersonGroupAsync(PersonGroupInfo personGroup)
        {
            await PersonGroup.DeleteGroupAsync(personGroup.personGroupId);
            GetPersonGroupsAsync();
        }

        private static async void GetPeopleInGroupAsync()
        {
            for (int i = 0; i < Window.currentGroups.Length; i++)
            {
                Window.currentGroups[i].People = await Person.GetPersonListAsync(Window.currentGroups[i].personGroupId);
            }
        }

        private static async void CreatePersonAsync(string personName, PersonGroupInfo group)
        {
            newPersonName = string.Empty;

            for (int i = 0; i < group.People?.Length; i++)
            {
                if (group.People[i].name == personName)
                {
                    Debug.LogFormat("{0} already exists", personName);
                    return;
                }
            }

            await Person.CreatePersonAsync(personName, group.personGroupId);
            GetPeopleInGroupAsync();
            Directory.CreateDirectory(Directory.GetParent(Application.dataPath) + "/FaceAPI/" + personName);
        }

        private static async void UploadPhotosAsync(PersonInfo person, PersonGroupInfo group, string faceDirectory)
        {
            uploadingImages = true;

            for (int i = 0; i < Window.currentGroups.Length; i++)
            {
                if (Window.currentGroups[i].personGroupId == group.personGroupId)
                {
                    Window.currentGroups[i].TrainingStatus = new TrainingStatus();
                    break;
                }
            }

            var images = Directory.GetFiles(faceDirectory);

            for (int i = 0; i < images.Length; i++)
            {
                await Person.CreateFaceAsync(person.personId, group.personGroupId, File.ReadAllBytes(images[i]));
                await Task.Delay(1000);
            }

            uploadingImages = false;
        }

        private static async void DeletePersonAsync(string groupPersonGroupId, string personId)
        {
            await Person.DeletePersonAsync(groupPersonGroupId, personId);
            GetPeopleInGroupAsync();
        }

        private static Texture2D LoadImageResource(string name)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CognitiveServices/FaceAPI/Editor/Icons/" + name + ".png");
        }
    }
}
