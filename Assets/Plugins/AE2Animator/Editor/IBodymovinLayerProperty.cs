using u.movin;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IBodymovinLayerProperty
    {
        void AddProperty(AnimationClip animationClip, BodymovinContent content,BodymovinLayer layer, float timeUnit);
    }
}