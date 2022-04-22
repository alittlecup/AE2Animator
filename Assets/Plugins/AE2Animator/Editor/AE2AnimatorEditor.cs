using System.Runtime.Remoting.Contexts;
using u.movin;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace DefaultNamespace
{
    public class AE2AnimatorEditor : EditorWindow
    {
        private string PreChooseJsonLocationKey = "PreChooseJsonLocationKey";
        private string prefabName = "请输入 Prefab 名称";
        private string jsonFilePath = "";
        private string imageFolderPath = "";
        private string prefabPath = "";

        [MenuItem("Window/AE2Animator")]
        public static void ShowAE2AnimatorEditor()
        {
            var window = GetWindow<AE2AnimatorEditor>(true, "AE2AnimatorEditor", true);
            window.Show();
        }

        private void OnGUI()
        {
            prefabName = EditorGUILayout.TextField(new GUIContent("Prefab Name"), prefabName);
            string jsonPrePath = PlayerPrefs.GetString(PreChooseJsonLocationKey);
            if (GUILayout.Button("Choose Json File", GUILayout.MaxWidth(140f)))
            {
                jsonFilePath = EditorUtility.OpenFilePanel("选择 Json 文件位置", jsonPrePath, "");
            }

            if (jsonFilePath != "")
            {
                PlayerPrefs.SetString(PreChooseJsonLocationKey, jsonFilePath);
                PlayerPrefs.Save();
                if (jsonFilePath.Contains(Application.dataPath + "/Resources/"))
                {
                    jsonFilePath =
                        jsonFilePath.Replace(Application.dataPath + "/Resources/", ""); //全路径，要截断回AssetDatabase所需的相对路径
                    jsonFilePath = jsonFilePath.Split('.')[0];
                }

                GUILayout.Label(jsonFilePath);
            }

            if (GUILayout.Button("Choose Images Folder", GUILayout.MaxWidth(140f)))
            {
                imageFolderPath = EditorUtility.OpenFolderPanel("选择 Image 文件夹位置", Application.dataPath, "");
            }

            if (imageFolderPath != "")
            {
                imageFolderPath =
                    imageFolderPath.Replace(Application.dataPath + "/Resources/", ""); //全路径，要截断回AssetDatabase所需的相对路径
                GUILayout.Label(imageFolderPath);
            }

            if (GUILayout.Button("Choose Save Prefab Path", GUILayout.MaxWidth(160f)))
            {
                prefabPath = EditorUtility.OpenFolderPanel("选择 Prefab 文件位置", Application.dataPath, "");
            }

            if (prefabPath != "")
            {
                prefabPath = prefabPath.Replace(Application.dataPath, "Assets"); //全路径，要截断回AssetDatabase所需的相对路径
                GUILayout.Label(prefabPath);
            }

            if (GUILayout.Button("保存"))
            {
                if (string.IsNullOrEmpty(prefabName))
                {
                    Debug.LogError("Prefab 名称为空");
                    return;
                }

                if (string.IsNullOrEmpty(jsonFilePath))
                {
                    Debug.LogError("Json 文件路径为空");
                    return;
                }

                if (string.IsNullOrEmpty(imageFolderPath))
                {
                    Debug.LogError("image 文件夹路径为空");
                    return;
                }

                if (string.IsNullOrEmpty(prefabPath))
                {
                    Debug.LogError("prefab 保存文件夹路径为空");
                    return;
                }

                GenerateAnimator(prefabName, jsonFilePath, imageFolderPath, prefabPath);
            }
        }

        private void GenerateAnimator(string prefabName, string jsonFilePath, string imageFolder, string prefabPath)
        {
            var content = BodymovinContent.init(jsonFilePath);
            var generator = new AnimationControllerGenerator();
            var controllerPath = generator.GenerateAnimationController(content, prefabName);
            var prefabGenerator = new PrefabGenerator();
            prefabGenerator.BuildPrefab(content, prefabName, prefabPath, imageFolder, controllerPath);
        }
    }
}