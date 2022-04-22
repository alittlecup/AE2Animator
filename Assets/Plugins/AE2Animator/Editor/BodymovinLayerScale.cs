
using u.movin;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BodymovinLayerScale: IBodymovinLayerProperty
    {
        public void AddProperty(AnimationClip animationClip,BodymovinContent content, BodymovinLayer layer, float timeUnit)
        {
            var sets = layer.scaleSets;
            if (sets.Length > 0)
            {
                var editorCurveBindingX = new EditorCurveBinding()
                {
                    path = layer.prefabPath(content),
                    type = typeof(RectTransform),
                    propertyName = "m_LocalScale.x",
                };
                var animationCurveX = new AnimationCurve();

                var editorCurveBindingY = new EditorCurveBinding()
                {
                    path = layer.prefabPath(content),
                    type = typeof(RectTransform),
                    propertyName = "m_LocalScale.y",
                };
                var animationCurveY = new AnimationCurve();

                var editorCurveBindingZ = new EditorCurveBinding()
                {
                    path = layer.prefabPath(content),
                    type = typeof(RectTransform),
                    propertyName = "m_LocalScale.z",
                };
                var animationCurveZ = new AnimationCurve();

                for (var i = 0; i < sets.Length; i++)
                {
                    var animatedProperties = sets[i];
                    var key = animatedProperties.t;
                    animationCurveX.AddKey(key * timeUnit, animatedProperties.s.x);
                    animationCurveY.AddKey(key * timeUnit, animatedProperties.s.y);
                    animationCurveZ.AddKey(key * timeUnit, animatedProperties.s.z);

                }
                AnimationUtility.SetEditorCurve(animationClip,editorCurveBindingX,animationCurveX);
                AnimationUtility.SetEditorCurve(animationClip,editorCurveBindingY,animationCurveY);
                AnimationUtility.SetEditorCurve(animationClip,editorCurveBindingZ,animationCurveZ);
            }
        }
    }
}