using Editor.Windows.DialogWindows;
using ModelImporter.Editor.Helper.Animator;
using UnityEditor;
using UnityEngine;

namespace ModelImporter.Editor.Windows
{
	public abstract class ModelImporterBaseDialogWindow<T> : BaseDialogWindow<T> where T:BaseDialogWindow<T>
	{
		protected abstract string WindowTitle { get; }

		protected override void DrawContentEditor()
		{
			DrawHeader();
		}

		private void DrawHeader()
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(WindowTitle, EditorStyles.boldLabel, GUILayout.Width(_size.x - 140));
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