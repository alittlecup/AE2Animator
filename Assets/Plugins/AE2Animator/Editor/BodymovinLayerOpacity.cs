using u.movin;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BodymovinLayerOpacity : IBodymovinLayerProperty
    {
        public void AddProperty(AnimationClip animationClip, BodymovinContent content, BodymovinLayer layer,
            float timeUnit)
        {
            var sets = layer.opacitySets;
            if (sets.Length > 0)
            {
                if (layer.type == 2)
                {
                    var editorCurveBinding = new EditorCurveBinding()
                    {
                        path = layer.prefabPath(content),
                        type = typeof(Image),
                        propertyName = "m_Color.a",
                    };
                    var animationCurve = new AnimationCurve();

                    for (var i = 0; i < sets.Length; i++)
                    {
                        var animatedProperties = sets[i];
                        var key = animatedProperties.t;
                        animationCurve.AddKey(key * timeUnit, animatedProperties.sf / 100f);
                    }

                    AnimationUtility.SetEditorCurve(animationClip, editorCurveBinding, animationCurve);
                }
                else if (layer.type == 0)
                {
                    var bodymovinLayers = content.GetChildLayer(layer);
                    foreach (var childLayer in bodymovinLayers)
                    {
                        ArrayUtility.AddRange(ref childLayer.opacitySets, sets);
                        AddProperty(animationClip, content, childLayer, timeUnit);
                    }
                }
                else if (layer.type == 4)
                {
                    var bodymovinShapes = layer.shapes;
                    foreach (var shape in bodymovinShapes)
                    {
                        var shapeName = layer.GetShapeName(shape);
                        var editorCurveBinding = new EditorCurveBinding()
                        {
                            path = layer.prefabPath(content) + "/" + shapeName,
                            type = typeof(MeshRenderer),
                            propertyName = "material._Color.a",
                        };
                        var animationCurve = new AnimationCurve();

                        for (var i = 0; i < sets.Length; i++)
                        {
                            var animatedProperties = sets[i];
                            var key = animatedProperties.t;
                            animationCurve.AddKey(key * timeUnit, animatedProperties.sf / 100f);
                        }

                        AnimationUtility.SetEditorCurve(animationClip, editorCurveBinding, animationCurve);
                    }
                }
            }
        }
    }
}