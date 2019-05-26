using Editor.Windows.DialogWindows;
using ModelImporter.Data;
using ModelImporter.Editor.Helper;
using UnityEditor;
using UnityEngine;

namespace ModelImporter.Editor.Windows
{
	public class ModelImportDialogWindow : BaseDialogWindow<ModelImportDialogWindow>
	{
		private enum Tab
		{
			Model,
			Animations,
			Materials
		}
		
		private const int Width = 500;
		private const int Height = 250;
		private UnityEditor.ModelImporter _modelImporter;
		private ModelImportData _modelImportData;
		private bool _initialized;
		private Vector2 _scroll;
		private Tab _currentTab;
		
		public UnityEditor.ModelImporter ModelImporter { get { return _modelImporter; } }
		public ModelImportData ModelImportData{get { return _modelImportData; }}

		public void Initialize(UnityEditor.ModelImporter modelImporter, ModelImportData mid)
		{
			_modelImporter = modelImporter;
			_modelImportData = mid;
		}
		
		protected override void DrawContentEditor()
		{
			_size = new Vector2(Width, Height);
			UpdatePosition();
			_centerInParentWindow = false;
			DrawHeader();
			DrawSelectedTab();
		}

		private void DrawHeader()
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(string.Format("Model Import Data(MID) - {0}", _modelImportData.name),
				EditorStyles.boldLabel, GUILayout.Width(Width - 140));
			LanguageSelector();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.HelpBox(TextResourcesHelper.ModelImportDataDialogWindow.NewMidMessage, 
				MessageType.Info);
			DrawTabsEditor();
		}

		private void DrawTabsEditor()
		{
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Toggle(_currentTab == Tab.Model, TextResourcesHelper.ModelImportDataDialogWindow.ModelTab,
				EditorStyles.miniButtonLeft))
				_currentTab = Tab.Model;
			if (GUILayout.Toggle(_currentTab == Tab.Animations, TextResourcesHelper.ModelImportDataDialogWindow.AnimationsTab,
				EditorStyles.miniButtonMid))
				_currentTab = Tab.Animations;
			if (GUILayout.Toggle(_currentTab == Tab.Materials, TextResourcesHelper.ModelImportDataDialogWindow.MaterialsTab,
				EditorStyles.miniButtonRight))
				_currentTab = Tab.Materials;
			EditorGUILayout.EndHorizontal();
		}

		private void LanguageSelector()
		{
			var currentLanguage = TextResourcesHelper.CurrentLanguage;
			GUILayout.FlexibleSpace();
			if (GUILayout.Toggle(currentLanguage == TextResourcesHelper.Language.English, 
				"English", EditorStyles.miniButtonLeft, GUILayout.Width(60))) 
				currentLanguage = TextResourcesHelper.Language.English;
			if (GUILayout.Toggle(currentLanguage == TextResourcesHelper.Language.Russian, 
				"Русский", EditorStyles.miniButtonRight, GUILayout.Width(60))) 
				currentLanguage = TextResourcesHelper.Language.Russian;
			TextResourcesHelper.CurrentLanguage = currentLanguage;
		}

		private void DrawSelectedTab()
		{
			if (_currentTab == Tab.Model) DrawModelEditor();
			else if (_currentTab == Tab.Animations) DrawAnimationsEditor();
			else if (_currentTab == Tab.Materials) { }
		}

		private void DrawModelEditor()
		{
			EditorGUILayout.EnumPopup("Normals", _modelImporter.importNormals);
			EditorGUILayout.EnumPopup("Normals Mode", _modelImporter.normalCalculationMode);
			EditorGUILayout.Slider("Smoothing angle", _modelImporter.normalSmoothingAngle, 0, 180);
		}
		
		private void DrawAnimationsEditor()
		{
			EditorGUILayout.Space();
			EditorGUILayout.BeginVertical(GUI.skin.box);
			EditorGUILayout.LabelField(TextResourcesHelper.ModelImportDataDialogWindow.AnimationsLabel);
			_scroll = EditorGUILayout.BeginScrollView(_scroll);
			var animations = _modelImportData.AnimationsData;
			for (var index = 0; index < animations.Count; index++)
			{
				EditorGUILayout.BeginHorizontal();
				DrawAnimationEditor(animations[index], index);
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}

		private void DrawAnimationEditor(ModelImportData.AnimationData animation, int index)
		{
			EditorGUILayout.LabelField(index.ToString(), GUILayout.Width(20));
			if (GUILayout.Button(animation.Name)) { }
			DrawAnimationSettings(animation);
		}

		private void DrawAnimationSettings(ModelImportData.AnimationData animation)
		{
			var animationData = _modelImportData.GetAnimationData(animation.Name);
			EditorGUILayout.LabelField("Loop time", GUILayout.Width(70));
			animation.LoopTime = animationData.LoopTime = EditorGUILayout.Toggle(animationData.LoopTime,
				GUILayout.Width(16));
			EditorGUILayout.LabelField("Loop blend", GUILayout.Width(70));
			animation.LoopPose = animationData.LoopPose = EditorGUILayout.Toggle(animationData.LoopPose,
				GUILayout.Width(16));
		}
	}
}