using System.Collections.Generic;
using Editor.Windows.DialogWindows;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace ModelImporter.Editor
{
	public class AnimatorGeneratorDialogiWindow : BaseDialogWindow<AnimatorGeneratorDialogiWindow>
	{
		public class AnimationView
		{
			public AnimationClip Animation;
			public AnimationClipSettings AnimationClipSettings;
			public bool Checked;
		}

		private const int Width = 500;
		private const int Height = 250;
		public string Path;
		public List<AnimationView> Animations;
		private bool _inited;
		
		protected override void DrawContentEditor()
		{
			_size = new Vector2(Width, Height);
			UpdatePosition();
			_centerInParentWindow = false;
			Initialize();
			DrawEditor();
		}

		private void Initialize()
		{
			if (_inited) return;
			_inited = true;
			Animations = LoadAnimations();
		}
		
		private List<AnimationView> LoadAnimations()
		{
			var assets = AssetDatabase.LoadAllAssetsAtPath(Path);
			var animations = new List<AnimationView>();
			for (var index = 0; index < assets.Length; index++)
			{
				var animation = assets[index] as AnimationClip;
				if (animation != null && !animation.name.Contains("__preview__"))
				{
					animations.Add(new AnimationView()
					{
						Animation = animation,
						AnimationClipSettings =  AnimationUtility.GetAnimationClipSettings(animation),
						Checked = this,
					});
				}
			}
			return animations;
		}

		private void DrawEditor()
		{
			EditorGUILayout.LabelField(System.IO.Path.GetFileName(Path), EditorStyles.boldLabel);
			DrawAnimationsEditor();
		}
		
		private void DrawAnimationsEditor()
		{
			EditorGUILayout.Space();
			DrawAnimationGenerationMessage();
			EditorGUILayout.BeginVertical(GUI.skin.box);
			for (var index = 0; index < Animations.Count; index++)
			{
				EditorGUILayout.BeginHorizontal();
				DrawAnimationEditor(Animations[index], index);
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();
		}

		private void DrawAnimationEditor(AnimationView animationView, int index)
		{
			EditorGUILayout.LabelField(index.ToString(), GUILayout.Width(20));
			animationView.Checked = EditorGUILayout.Toggle(animationView.Checked, GUILayout.Width(20));
			if (GUILayout.Button(animationView.Animation.name))
				EditorGUIUtility.PingObject(Animations[index].Animation);
		}
		
		private void DrawAnimationGenerationMessage()
		{
			var animatorPath = System.IO.Path.ChangeExtension(Path, ".controller");
			string message;
			MessageType messageType;
			if (AssetDatabase.LoadAssetAtPath<AnimatorController>(animatorPath) != null)
			{
				message = "This folder contains AnimatorController. Selected animations will be overridden"; 
				messageType = MessageType.Warning;
			}
			else
			{
				message = "This folder doesn't contain AnimatorController. " +
				          "AnimatorController will be created and selected animations will be set";
				messageType = MessageType.Info;
			}

			EditorGUILayout.HelpBox(message, messageType);
		}
	}
}