using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace ModelImporter.Editor.Helper.Animator
{
	public class AnimatorDescription
	{
		public enum AnimatorType
		{
			Default,
			Override
		}
		
		private const string AnimatorExtension = "controller";
		private const string OverrideAnimatorExtension = "overrideController";

		private readonly AnimatorController _animatorController;
		private readonly AnimatorOverrideController _animatorOverrideController;

		public AnimatorType Type { get; private set; }
		public RuntimeAnimatorController Animator
		{
			get { return Type == AnimatorType.Default ? (RuntimeAnimatorController)_animatorController : _animatorOverrideController; }
		}

		private AnimatorDescription(AnimatorController animatorController, AnimatorOverrideController animatorOverrideController,
			AnimatorType animatorType)
		{
			Type = animatorType;
			_animatorController = animatorController;
			_animatorOverrideController = animatorOverrideController;
		}

		public void ReplaceAnimation(AnimationClip animation, bool isDefault)
		{
			switch (Type)
			{
				case AnimatorType.Default:
					ReplaceDefaultAnimation(animation, isDefault);
					break;
				case AnimatorType.Override:
					ReplaceOverrideAnimation(animation);
					break;
			}
		}

		public void SetDirty()
		{
			switch (Type)
			{
				case AnimatorType.Default:
					EditorUtility.SetDirty(_animatorController);
					break;
				case AnimatorType.Override:
					EditorUtility.SetDirty(_animatorOverrideController);
					break;
			}
		}

		private void ReplaceDefaultAnimation(AnimationClip animation, bool isDefault)
		{
			var stateMachine = _animatorController.layers[0].stateMachine;
			var states = stateMachine.states;
			for (var index = 0; index < states.Length; index++)
			{
				if (states[index].state.name == animation.name)
				{
					states[index].state.motion = animation;
					if (isDefault) stateMachine.defaultState = states[index].state;
					return;
				}
			}
			var state = stateMachine.AddState(animation.name);
			state.motion = animation;
			if (isDefault) stateMachine.defaultState = state;
		}

		private void ReplaceOverrideAnimation(AnimationClip animation)
		{
			var pairs = new List<KeyValuePair<AnimationClip, AnimationClip>>();
			_animatorOverrideController.GetOverrides(pairs);
			for (var index = 0; index < pairs.Count; index++)
				if (pairs[index].Key.name == animation.name)
				{
					pairs[index] = new KeyValuePair<AnimationClip, AnimationClip>(pairs[index].Key, animation);
					_animatorOverrideController.ApplyOverrides(pairs);
					return;
				}
		}
		
		public static AnimatorDescription LoadAnimatorDescription(string path)
		{
			var defaultAnimator = AssetDatabase.LoadAssetAtPath<AnimatorController>(string.Format("{0}.{1}", path, AnimatorExtension));
			var overrideAnimator = AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(string.Format("{0}.{1}", path, OverrideAnimatorExtension));
			if (defaultAnimator == null && overrideAnimator == null) return null;
			var type = defaultAnimator != null ? AnimatorType.Default : AnimatorType.Override;
			return new AnimatorDescription(defaultAnimator, overrideAnimator, type);
		}

		public static AnimatorDescription CreateAnimatorDescription(string path)
		{
			var animatorController = AnimatorController.CreateAnimatorControllerAtPath(string.Format("{0}.{1}", path, AnimatorExtension));
			return new AnimatorDescription(animatorController, null, AnimatorType.Default);
		}
	}
}