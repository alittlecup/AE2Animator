using System.Collections.Generic;
using u.movin;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Test
    {
        private static string jsonPath = "json/data";

        [MenuItem("Test/BuildAnimationController")]
        public static void BuildAnimationController()
        {
            AnimatorController animatorController =
                AnimatorController.CreateAnimatorControllerAtPath("Assets/Resources/animation.controller");
            //得到它的Layer， 默认layer为base 你可以去拓展
            AnimatorControllerLayer layer = animatorController.layers[0];
            //把动画文件保存在我们创建的AnimationController中
            AddAnimatorToLayer(jsonPath, layer);
        }

        private static void AddAnimatorToLayer(string path, AnimatorControllerLayer layer)
        {
            var bodymovinContent = BodymovinContent.init(path);
            var bodymovinContentLayers = bodymovinContent.layers;
            var bodymovinLayer = bodymovinContentLayers[0];
        }

        [MenuItem("Test/BuildAnimator")]
        public static void BuildAnimator()
        {
            var animationClip = new AnimationClip();
            AnimationUtility.SetAnimationType(animationClip, ModelImporterAnimationType.Generic);


            var content = BodymovinContent.init(jsonPath);
            var bodymovinLayers = content.layers;
            animationClip.frameRate = content.fr;
            var timeUnit = 1f / animationClip.frameRate;
            var bodymovinLayerProperties = new List<IBodymovinLayerProperty>
            {
                new BodymovinLayerPosition(),
                new BodymovinLayerScale(),
                new BodymovinLayerRotation(),
                new BodymovinLayerOpacity()
            };
            foreach (var bodymovinLayer in bodymovinLayers)
            {
                foreach (var property in bodymovinLayerProperties)
                {
                    property.AddProperty(animationClip, content, bodymovinLayer, timeUnit);
                }
            }

            AssetDatabase.CreateAsset(animationClip, "Assets/Resources/Generate1.anim");
            AssetDatabase.SaveAssets();
        }


        [MenuItem("Test/BuildPrefab")]
        public static void BuildPrefab()
        {
            var prefabRoot = CreatePrefabRoot();
            var prefabPath = "Assets/Resources/";
            var prefabName = "Generate1";
            var content = BodymovinContent.init(jsonPath);
            var aaa = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Resources/MeshAnim.anim");
            var layers = new MovinLayer[content.layers.Length];
            /* ----- CREATE LAYERS ----- */

            var layersByIndex = new MovinLayer[content.highestLayerIndex + 1];

            for (int i = 0; i < content.layers.Length; i++)
            {
                MovinLayer layer = new MovinLayer(prefabRoot.GetComponent<RectTransform>(), content.layers[i],
                    "json/image", content.layers.Length - i);

                layers[i] = layer;
                layersByIndex[layer.content.ind] = layers[i];
            }

            /* ----- SET PARENTS ----- */

            for (int i = 0; i < layers.Length; i++)
            {
                MovinLayer layer = layers[i];
                int p = layer.content.parent;
                if (p <= 0)
                {
                    continue;
                }

                layer.transform.SetParent(
                    layersByIndex[p].content.shapes.Length > 0
                        ? layersByIndex[p].transform.GetChild(0)
                        : layersByIndex[p].transform, false);
            }

            PrefabUtility.CreatePrefab(prefabPath + prefabName + ".prefab", prefabRoot);
        }

        private static GameObject CreatePrefabRoot()
        {
            var container = new GameObject();
            container.AddComponent<RectTransform>();
            var rectTransform = container.GetComponent<RectTransform>();
            rectTransform.gameObject.AddComponent<CanvasRenderer>();
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMin = new Vector2(0, 0);
            rectTransform.offsetMax = new Vector2(0, 0);
            return container;
        }
    }
}