using System.Collections.Generic;
using u.movin;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using AnimatorController = UnityEditor.Animations.AnimatorController;
using AnimatorControllerLayer = UnityEditor.Animations.AnimatorControllerLayer;

namespace DefaultNamespace
{
    public class AnimationControllerGenerator
    {
        private string dir = "Assets/Resources/";

        private string BuildAnimator(BodymovinContent content, string animationName)
        {
            var animationClip = new AnimationClip();
            AnimationUtility.SetAnimationType(animationClip, ModelImporterAnimationType.Generic);

            var bodymovinLayers = content.layers;
            animationClip.frameRate = content.fr;
            var timeUnit = 1f / animationClip.frameRate;
            var bodymovinLayerProperties = new List<IBodymovinLayerProperty>
            {
                new BodymovinLayerPosition(),
                new BodymovinLayerScale(),
                new BodymovinLayerRotation(),
                new BodymovinLayerOpacity(),
                new BodymovinLayerColor(),
            };
            foreach (var bodymovinLayer in bodymovinLayers)
            {
                foreach (var property in bodymovinLayerProperties)
                {
                    property.AddProperty(animationClip, content, bodymovinLayer,timeUnit);
                }
            }

            var path = dir + animationName + "Anim.anim";
            AssetDatabase.CreateAsset(animationClip, path);
            AssetDatabase.SaveAssets();
            return path;
        }

        public string GenerateAnimationController(BodymovinContent content, string animationName)
        {

            var path = dir + animationName + "Controller.controller";
            AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(path);
            //得到它的Layer， 默认layer为base 你可以去拓展
            AnimatorControllerLayer layer = animatorController.layers[0];

            var animatorPath = BuildAnimator(content, animationName);

            AnimationClip newClip = AssetDatabase.LoadAssetAtPath(animatorPath, typeof(AnimationClip)) as AnimationClip;

            var animatorStateMachine = layer.stateMachine;

            //取出动画名子 添加到state里面
            AnimatorState state = animatorStateMachine.AddState(newClip.name);
            state.motion = newClip;
            AssetDatabase.SaveAssets();
            return path;
        }
    }
}