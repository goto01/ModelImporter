using System.Collections.Generic;
using System.IO;
using ModelImporter.Editor.Windows;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace ModelImporter.Editor.Helper.Animator
{
	public static class AnimatorHelper
	{
		private const string PostFix = "AnimatorController";
		
		public static AnimatorDescription GetAnimatorController(string assetPath)
		{
			var animatorControllerPath = GetAnimatorControllerPath(assetPath);
			var animatorController = AnimatorDescription.LoadAnimatorDescription(animatorControllerPath);
			if (animatorController == null) 
				animatorController = AnimatorDescription.CreateAnimatorDescription(animatorControllerPath);
			return animatorController;
		}

		public static void ReplaceAnimator(AnimatorDescription animator, 
			List<AnimatorGeneratorDialogWindow.AnimationView> animations)
		{
			for (var index = 0; index < animations.Count; index++)
			{
				if (!animations[index].Checked) continue;
				animator.ReplaceAnimation(animations[index].Animation, animations[index].Default);
			}
			animator.SetDirty();
		}
		
		private static string GetAnimatorControllerPath(string assetPath)
		{
			var fileName = Path.GetFileNameWithoutExtension(assetPath);
			var directory = Path.GetDirectoryName(assetPath);
			return string.Format("{0}/{1}{2}", directory, fileName, PostFix);
		}
	}
}