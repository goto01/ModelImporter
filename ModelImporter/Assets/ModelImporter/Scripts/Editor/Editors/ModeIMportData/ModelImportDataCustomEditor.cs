using ModelImporter.Data;
using ModelImporter.Editor.Helper.Animator;
using UnityEditor;
using UnityEngine;

namespace ModelImporter.Editor.Editors.ModeImportData
{
	[CustomEditor(typeof(Data.ModelImportData))]
	public class ModelImportDataCustomEditor : UnityEditor.Editor
	{
		private Vector2 _scroll;
		private ModelImportDataEditor.Tab _currentTab;
		
		private ModelImportData ModelImportData { get { return (ModelImportData) target; } }
		
		public override void OnInspectorGUI()
		{
			DrawLanguageSelector();
			ModelImportDataEditor.Draw(ModelImportData, ref _currentTab, ref _scroll);
			EditorUtility.SetDirty(ModelImportData);
		}
		
		private void DrawLanguageSelector()
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			LanguageSelector();
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
	}
}