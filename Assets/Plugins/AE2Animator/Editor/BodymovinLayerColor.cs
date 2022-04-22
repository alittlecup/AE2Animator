using u.movin;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BodymovinLayerColor : IBodymovinLayerProperty
    {
        public void AddProperty(AnimationClip animationClip, BodymovinContent content, BodymovinLayer layer,
            float timeUnit)
        {
            var sets = layer.effectSets;
            if (sets.Length > 0)
            {
                // if (layer.type == 2)
                // {
                //     var editorCurveBinding = new EditorCurveBinding()
                //     {
                //         path = layer.prefabPath(content),
                //         type = typeof(Image),
                //         propertyName = "m_Color.a",
                //     };
                //     var animationCurve = new AnimationCurve();
                //
                //     for (var i = 0; i < sets.Length; i++)
                //     {
                //         var animatedProperties = sets[i];
                //         var key = animatedProperties.t;
                //         animationCurve.AddKey(key * timeUnit, animatedProperties.sf / 100f);
                //     }
                //
                //     AnimationUtility.SetEditorCurve(animationClip, editorCurveBinding, animationCurve);
                // }
                // else
                if (layer.type == 0)
                {
                    var bodymovinLayers = content.GetChildLayer(layer);
                    foreach (var childLayer in bodymovinLayers)
                    {
                        ArrayUtility.AddRange(ref childLayer.effectSets, sets);
                        AddProperty(animationClip, content, childLayer, timeUnit);
                    }
                }
                else if (layer.type == 4)
                {
                    foreach (var layerEffectSet in layer.effectSets)
                    {
                        var fillColorSets = layerEffectSet.fillColorSets;
                        if (fillColorSets!=null&& fillColorSets.Length > 0)
                        {
                            var bodymovinShapes = layer.shapes;
                            foreach (var shape in bodymovinShapes)
                            {
                                var shapeName = layer.GetShapeName(shape);
                                var path = layer.prefabPath(content) + "/" + shapeName ;

                                for (var i = 0; i < fillColorSets.Length; i++)
                                {
                                    var animatedProperties = fillColorSets[i];
                                    var key = animatedProperties.t;

                                    var editorCurveBindingR = new EditorCurveBinding()
                                    {
                                        path = path,
                                        type = typeof(MeshRenderer),
                                        propertyName = "material._Color.r",
                                    };
                                    var animationCurveR = new AnimationCurve();
                                    animationCurveR.AddKey(key * timeUnit, animatedProperties.s[0]);
                                    AnimationUtility.SetEditorCurve(animationClip, editorCurveBindingR, animationCurveR);

                                    var editorCurveBindingG = new EditorCurveBinding()
                                    {
                                        path = path,
                                        type = typeof(MeshRenderer),
                                        propertyName = "material._Color.g",
                                    };
                                    var animationCurveG = new AnimationCurve();
                                    animationCurveG.AddKey(key * timeUnit, animatedProperties.s[1]);
                                    AnimationUtility.SetEditorCurve(animationClip, editorCurveBindingG, animationCurveG);

                                    var editorCurveBindingB = new EditorCurveBinding()
                                    {
                                        path = path,
                                        type = typeof(MeshRenderer),
                                        propertyName = "material._Color.b",
                                    };
                                    var animationCurveB = new AnimationCurve();
                                    animationCurveB.AddKey(key * timeUnit, animatedProperties.s[2]);
                                    AnimationUtility.SetEditorCurve(animationClip, editorCurveBindingB, animationCurveB);
                                }

                            }
                        }
                    }
                }
            }
        }
    }
}