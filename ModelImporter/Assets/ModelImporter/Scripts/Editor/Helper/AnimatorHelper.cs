using System.Collections.Generic;
using System.IO;
using ModelImporter.Editor.Windows;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace ModelImporter.Editor.Helper
{
	public static class AnimatorHelper
	{
		private const string PostFix = "AnimatorController";
		
		public static AnimatorController GetAnimatorController(string assetPath)
		{
			var animatorControllerPath = GetAnimatorControllerPath(assetPath);
			var animatorController = 
				AssetDatabase.LoadAssetAtPath<AnimatorController>(animatorControllerPath);
			if (animatorController == null) 
				animatorController = CreateAnimatorController(animatorControllerPath);
			return animatorController;
		}

		public static void ReplaceAnimator(AnimatorController animator, 
			List<AnimatorGeneratorDialogWindow.AnimationView> animations)
		{
			for (var index = 0; index < animations.Count; index++)
				if (animations[index].Checked) ReplaceAnimation(animator, animations[index].Animation);
			EditorUtility.SetDirty(animator);
		}
		
		private static string GetAnimatorControllerPath(string assetPath)
		{
			var fileName = Path.GetFileNameWithoutExtension(assetPath);
			var directory = Path.GetDirectoryName(assetPath);
			return string.Format("{0}/{1}{2}.controller", directory, fileName, PostFix);
		}

		private static AnimatorController CreateAnimatorController(string path)
		{
			return AnimatorController.CreateAnimatorControllerAtPath(path);
		}
		
		private static void ReplaceAnimation(AnimatorController animator, AnimationClip clip)
		{
			var states = animator.layers[0].stateMachine.states;
			for (var index = 0; index < states.Length; index++)
			{
				if (states[index].state.name == clip.name)
				{
					states[index].state.motion = clip;
					return;
				}
			}
			var state = animator.layers[0].stateMachine.AddState(clip.name);
			state.motion = clip;
		}
	}
}