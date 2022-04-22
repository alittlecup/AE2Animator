

using System;
using System.Collections.Generic;
using UnityEngine;

namespace u.movin
{
    public static class BodymovinExtend
    {
        public static string prefabPath(this BodymovinLayer layer,BodymovinContent content)
        {
            var name = layer.prefabName();
            var layerParent = layer.parent;
            if (layerParent <= 0)
            {
                return name;
            }

            foreach (var bodymovinLayer in content.layers)
            {
                if (bodymovinLayer.ind == layerParent)
                {
                    var rootPath=bodymovinLayer.prefabPath(content);
                    return rootPath + "/" + name;
                }
            }
            return name;
        }

        public static string prefabName(this BodymovinLayer layer)
        {
            return layer.ind + "_" + layer.nm;
        }

        public static List<BodymovinLayer> GetChildLayer(this BodymovinContent content, BodymovinLayer layer)
        {
            var childLayers = new List<BodymovinLayer>();
            var layerInd = layer.ind;
            foreach (var bodymovinLayer in content.layers)
            {
                if (bodymovinLayer.parent == layerInd)
                {
                    childLayers.Add(bodymovinLayer);
                }
            }

            return childLayers;
        }

        public static string GetShapeName(this BodymovinLayer layer,BodymovinShape shape)
        {
            return  layer.ind+"_"+shape.ty+"_"+shape.ix;
        }

        public static Keyframe? GetKeyFrame(this AnimationCurve curve, float time)
        {
            foreach (var curveKey in curve.keys)
            {
                if (Math.Abs(curveKey.time - time) < 0.016f)
                {
                    return curveKey;
                }
            }

            return null;
        }
    }
}