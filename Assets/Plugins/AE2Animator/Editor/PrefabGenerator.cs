using u.movin;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace DefaultNamespace
{
    public class PrefabGenerator
    {
        public GameObject BuildPrefab(BodymovinContent content, string prefabName, string prefabPath,
            string imageFolder, string controllerPath)
        {
            var prefabRoot = CreatePrefabRoot();
            var layers = new MovinLayer[content.layers.Length];
            /* ----- CREATE LAYERS ----- */

            var layersByIndex = new MovinLayer[content.highestLayerIndex + 1];

            for (int i = 0; i < content.layers.Length; i++)
            {
                MovinLayer layer = new MovinLayer(prefabRoot.GetComponent<RectTransform>(), content.layers[i],
                    imageFolder, content.layers.Length - i);

                layers[i] = layer;
                layersByIndex[layer.content.ind] = layers[i];
                SaveMovinShape(layer, prefabPath);
            }
            AssetDatabase.SaveAssets();

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

            var animator = prefabRoot.AddComponent<Animator>();
            animator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);

            PrefabUtility.CreatePrefab(prefabPath + "/" + prefabName + ".prefab", prefabRoot);
            return prefabRoot;
        }

        private void SaveMovinShape(MovinLayer layer, string dirPath)
        {
            if (layer.shapes.Length > 0)
            {
                foreach (var layerShape in layer.shapes)
                {
                    var layerShapeMesh = layerShape.mesh;
                    if (layerShapeMesh != null)
                    {
                        var meshAssetPath = dirPath + "/" + layerShape.gameObject.name + "Mesh.asset";
                        var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshAssetPath);
                        if ( mesh == null)
                        {
                            AssetDatabase.CreateAsset(layerShapeMesh,meshAssetPath);
                        }
                        else
                        {
                            layerShape.mesh = mesh;
                            layerShape.gameObject.GetComponent<MeshFilter>().mesh = mesh;
                        }
                    }
                }
            }
        }

        private GameObject CreatePrefabRoot()
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