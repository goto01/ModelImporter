using System.Collections.Generic;
using UnityEditor;

namespace ModelImporter.Editor.Helper
{
	public static class TextResourcesHelper
	{
		private const string TextResourcesHelperCurrentLanguageName = "TextResourcesHelperCurrentLanguage";
		
		public enum Language
		{
			English = 0,
			Russian = 1
		}

		public static Language CurrentLanguage
		{
			get { return (Language) EditorPrefs.GetInt(TextResourcesHelperCurrentLanguageName, 0); }
			set { EditorPrefs.SetInt(TextResourcesHelperCurrentLanguageName, (int)value); }
		}

		public class ModelImportDataDialogWindow
		{
			private static Dictionary<Language, string> _newMidMessage = 
				new Dictionary<Language, string>()
				{
					{Language.English, "You're importing model for first time or with new animations. " +
					                   "ModelImportData will be created or overridden"},
					{Language.Russian, "Вы импортируете модель впервые или с новой анимацией. " +
					                   "ModelImportData будет создан или переопределен"}
				};

			public static string NewMidMessage { get { return _newMidMessage[CurrentLanguage]; } }
			
			private static Dictionary<Language, string> _animationsLabel = 
				new Dictionary<Language, string>()
				{
					{Language.English, "Animations:"},
					{Language.Russian, "Анимации:"},
				};
			
			public static string AnimationsLabel { get { return _animationsLabel[CurrentLanguage]; } }

			private static Dictionary<Language, string> _modelTab =
				new Dictionary<Language, string>()
				{
					{Language.English, "Model"},
					{Language.Russian, "Модель"},
				};
			
			public static string ModelTab { get { return _modelTab[CurrentLanguage]; } }
			
			private static Dictionary<Language, string> _animationsTab =
				new Dictionary<Language, string>()
				{
					{Language.English, "Animations"},
					{Language.Russian, "Анимации"},
				};
			
			public static string AnimationsTab { get { return _animationsTab[CurrentLanguage]; } }
			
			private static Dictionary<Language, string> _materialsTab =
				new Dictionary<Language, string>()
				{
					{Language.English, "Materials"},
					{Language.Russian, "Материалы"},
				};
			
			public static string MaterialsTab { get { return _materialsTab[CurrentLanguage]; } }
		}
		 
	}
}