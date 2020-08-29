using ModelImporter.Editor.Helper;
using ModelImporter.Editor.Helper.Animator;
using UnityEditor;
using UnityEngine;

namespace ModelImporter.Editor
{
	public static class ModelImporterPreferences
	{
		[PreferenceItem("Model Importer")]
		public static void DrawPreferences()
		{
			DrawLanguageSelector();
			DrawEnablingEditor();
			DrawScaleEpsilonEditor();
			DrawPositionMaxMagnitudeEditor();
			DrawRotationMaxMagnitudeEditor();
		}

		private static void DrawLanguageSelector()
		{
			var currentLanguage = TextResourcesHelper.CurrentLanguage;
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Toggle(currentLanguage == TextResourcesHelper.Language.English, 
				"English", EditorStyles.miniButtonLeft))
				currentLanguage = TextResourcesHelper.Language.English;
			if (GUILayout.Toggle(currentLanguage == TextResourcesHelper.Language.Russian, 
				"Русский", EditorStyles.miniButtonRight)) 
				currentLanguage = TextResourcesHelper.Language.Russian;
			TextResourcesHelper.CurrentLanguage = currentLanguage;
			EditorGUILayout.EndHorizontal();
		}

		private static void DrawEnablingEditor()
		{
			PrefsHelper.ModeImporterActive = 
				EditorGUILayout.Toggle(TextResourcesHelper.Preferences.ActiveLabel, PrefsHelper.ModeImporterActive);
		}

		private static void DrawScaleEpsilonEditor()
		{
			PrefsHelper.ModelImporterScaleDefaultEpsilon = EditorGUILayout.FloatField("Scale epsilon",
				PrefsHelper.ModelImporterScaleDefaultEpsilon);
		}

		private static void DrawPositionMaxMagnitudeEditor()
		{
			PrefsHelper.ModelImporterPositionMaxMagnitude = EditorGUILayout.FloatField("Position max magnitude",
				PrefsHelper.ModelImporterPositionMaxMagnitude);
		}
		
		private static void DrawRotationMaxMagnitudeEditor()
		{
			PrefsHelper.ModelImporterRotationMaxMagnitude = EditorGUILayout.FloatField("Rotation max magnitude",
				PrefsHelper.ModelImporterRotationMaxMagnitude);
		}
	}
}