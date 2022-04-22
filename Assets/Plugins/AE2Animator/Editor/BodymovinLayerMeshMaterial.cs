using u.movin;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    public class BodymovinLayerMeshMaterial: IBodymovinLayerProperty
    {
        public void AddProperty(AnimationClip animationClip, BodymovinContent content, BodymovinLayer layer, float timeUnit)
        {
            var sets = layer.effectSets;
            if (sets.Length > 0)
            {
                var editorCurveBinding = new EditorCurveBinding()
                {
                    path = layer.prefabPath(content),
                    type = typeof(Material),
                    propertyName = "m_Color.a",
                };
                var animationCurve = new AnimationCurve();

                for (var i = 0; i < sets.Length; i++)
                {
                    var animatedProperties = sets[i];
                    // var key = animatedProperties.t;
                    // animationCurve.AddKey(key * timeUnit, animatedProperties.sf/100f);
                }
                AnimationUtility.SetEditorCurve(animationClip,editorCurveBinding,animationCurve);
            }
        }
    }
}