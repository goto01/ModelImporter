using System.Collections.Generic;
using Editor.Windows.DialogWindows;
using ModelImporter.Editor.Helper.Animator;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace ModelImporter.Editor.Windows
{
	public class AnimatorGeneratorDialogWindow : ModelImporterBaseDialogWindow<AnimatorGeneratorDialogWindow>
	{
		public class AnimationView
		{
			public AnimationClip Animation;
			public AnimationClipSettings AnimationClipSettings;
			public bool Checked;
		}

		private const int Width = 500;
		private const int Height = 400;
		private string _path;
		private List<AnimationView> _animations;
		private Vector2 _scroll ;
		private bool _setAnimatorControllerInModel = true;

		protected override string WindowTitle { get { return System.IO.Path.GetFileName(_path); } }
		public List<AnimationView> Animations { get { return _animations; } }
		public string Path { get { return _path; } }
		public bool SetAnimatorControllerInModel { get { return _setAnimatorControllerInModel; } }

		public void Initialize(string assetPath)
		{
			_path = assetPath;
			_animations = LoadAnimations();
		}
		
		protected override void DrawContentEditor()
		{
			_size = new Vector2(Width, Height);
			base.DrawContentEditor();
			UpdatePosition();
			_centerInParentWindow = false;
			DrawEditor();
		}
		
		private List<AnimationView> LoadAnimations()
		{
			var assets = AssetDatabase.LoadAllAssetsAtPath(_path);
			var animations = new List<AnimationView>();
			for (var index = 0; index < assets.Length; index++)
			{
				var animation = assets[index] as AnimationClip;
				if (animation != null && !animation.name.Contains("__preview__"))
				{
					animations.Add(new AnimationView()
					{
						Animation = animation,
						AnimationClipSettings = AnimationUtility.GetAnimationClipSettings(animation),
						Checked = this,
					});
				}
			}
			return animations;
		}

		private void DrawEditor()
		{
			DrawAnimationsEditor();
		}
		
		private void DrawAnimationsEditor()
		{
			EditorGUILayout.Space();
			DrawAnimationGenerationMessage();
			_setAnimatorControllerInModel = EditorGUILayout.Toggle("Set animator to model", _setAnimatorControllerInModel);
			EditorGUILayout.BeginVertical(GUI.skin.box);
			EditorGUILayout.LabelField(TextResourcesHelper.ModelImportDataDialogWindow.AnimationsLabel);
			_scroll = EditorGUILayout.BeginScrollView(_scroll);
			for (var index = 0; index < _animations.Count; index++)
			{
				EditorGUILayout.BeginHorizontal(GUI.skin.box);
				DrawAnimationEditor(_animations[index], index);
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndVertical();
		}

		private void DrawAnimationEditor(AnimationView animationView, int index)
		{
			EditorGUILayout.LabelField(string.Format("{0} -", index), GUILayout.Width(20));
			animationView.Checked = EditorGUILayout.Toggle(animationView.Checked, GUILayout.Width(20));
			EditorGUILayout.LabelField(animationView.Animation.name, GUILayout.Width(160));
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Ping", GUILayout.Width(50))) EditorGUIUtility.PingObject(_animations[index].Animation);
			DrawAnimationSettingsEditor(animationView.AnimationClipSettings);
		}

		private void DrawAnimationSettingsEditor(AnimationClipSettings acs)
		{
			GUI.enabled = false;
			EditorGUILayout.LabelField("Loop time", GUILayout.Width(60));
			EditorGUILayout.Toggle(acs.loopTime, GUILayout.Width(16));
			EditorGUILayout.LabelField("Loop pose", GUILayout.Width(70));
			EditorGUILayout.Toggle(acs.loopBlend, GUILayout.Width(16));
			GUI.enabled = true;
		}
		
		private void DrawAnimationGenerationMessage()
		{
			EditorGUILayout.HelpBox(TextResourcesHelper.AnimatorGeneratorDialogWIndow.AnimatorGeneratorMessage, MessageType.Info);
		}
	}
}