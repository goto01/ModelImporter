using System.Collections.Generic;
using ModelImporter.Data;
using ModelImporter.Editor.Helper.Animator;
using UnityEditor;
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
			public bool Default;
		}

		private const int Width = 500;
		private const int Height = 400;
		private string _path;
		private List<AnimationView> _animations;
		private Vector2 _scroll ;
		private bool _setAnimatorControllerInModel = true;
		private ModelImportData _mid;

		protected override string WindowTitle { get { return System.IO.Path.GetFileName(_path); } }
		public List<AnimationView> Animations { get { return _animations; } }
		public string Path { get { return _path; } }
		public bool SetAnimatorControllerInModel { get { return _setAnimatorControllerInModel; } }

		public void Initialize(string assetPath)
		{
			_path = assetPath;
			_mid = ModelImportDataHelper.LoadModeImportData(assetPath);
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
						Checked = true,
						Default = _mid.GetAnimationData(animation.name).Default,
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
			DrawAnimationSettingsEditor(animationView);
		}

		private void DrawAnimationSettingsEditor(AnimationView animationView)
		{
			GUI.enabled = false;
			EditorGUILayout.LabelField("Loop", EditorStyles.miniLabel, GUILayout.Width(25));
			EditorGUILayout.Toggle(animationView.AnimationClipSettings.loopTime, GUILayout.Width(16));
			EditorGUILayout.LabelField("Loop blend", EditorStyles.miniLabel, GUILayout.Width(55));
			EditorGUILayout.Toggle(animationView.AnimationClipSettings.loopBlend, GUILayout.Width(16));
			EditorGUILayout.LabelField("Default", EditorStyles.miniLabel, GUILayout.Width(37));
			EditorGUILayout.Toggle(animationView.Default, GUILayout.Width(16));
			GUI.enabled = true;
		}
		
		private void DrawAnimationGenerationMessage()
		{
			EditorGUILayout.HelpBox(TextResourcesHelper.AnimatorGeneratorDialogWIndow.AnimatorGeneratorMessage, MessageType.Info);
		}
	}
}