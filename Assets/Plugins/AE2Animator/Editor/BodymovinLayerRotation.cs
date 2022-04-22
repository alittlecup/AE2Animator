using u.movin;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    public class BodymovinLayerRotation : IBodymovinLayerProperty
    {
        public void AddProperty(AnimationClip animationClip,BodymovinContent content, BodymovinLayer layer, float timeUnit)
        {
            AddRotation(layer.rotationZSets, "localEulerAnglesRaw.z",content, layer, animationClip, timeUnit);
            AddRotation(layer.rotationXSets, "localEulerAnglesRaw.x",content, layer, animationClip, timeUnit);
            AddRotation(layer.rotationYSets, "localEulerAnglesRaw.y",content, layer, animationClip, timeUnit);
        }

        private void AddRotation(BodymovinAnimatedProperties[] sets, string propertyName,BodymovinContent content, BodymovinLayer layer,
            AnimationClip animationClip, float timeUnit)
        {
            if (sets.Length > 0)
            {
                var editorCurveBinding = new EditorCurveBinding()
                {
                    path = layer.prefabPath(content),
                    type = typeof(RectTransform),
                    propertyName = propertyName,
                };
                var animationCurve = new AnimationCurve();
                for (var i = 0; i < sets.Length; i++)
                {
                    var animatedProperties = sets[i];
                    var key = animatedProperties.t;
                    animationCurve.AddKey(key * timeUnit, animatedProperties.sf);
                }

                AnimationUtility.SetEditorCurve(animationClip, editorCurveBinding, animationCurve);
            }
        }
    }
}