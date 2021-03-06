using System.Collections.Generic;
using UnityEditor;

namespace ModelImporter.Editor.Helper.Animator
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
					{Language.English, "You're importing model check properties of import and press Yes"},
					{Language.Russian, "Вы импортируете модель, проверьте свойства импорта и нажмите Yes"},
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

			private static Dictionary<Language, string> _materialsMessage =
				new Dictionary<Language, string>()
				{
					{Language.English, "With default material importing settings will be created " +
					                   "folder with materials in the current folder. Modify only these materials. They will be attached to the model each time when reimport."},
					{Language.Russian, "При настройках импорта материалов по умолчанию папка будет создана с материалами в текущей папке. " +
					                   "Изменяйте только эти материалы. Они будут прикреплены к модели каждый раз при реимпорте."}
				};
			
			public static string MaterialsMessage{get { return _materialsMessage[CurrentLanguage]; }}
		}
		 
		public class AnimatorGeneratorDialogWIndow
		{
			private static Dictionary<Language, string> _animatorGeneratorMessage = 
				new Dictionary<Language, string>()
				{
					{Language.English, "Selected animations will be overriden or created in AnimatorController in current folder"},
					{Language.Russian, "Выбранные анимации будут перезаписаны или созданны в AnimatorController в текущей дериктории"},
				};
			
			public static string AnimatorGeneratorMessage { get { return _animatorGeneratorMessage[CurrentLanguage]; } }
		}

		public class Preferences
		{
			private static Dictionary<Language, string> _activeLabel = 
				new Dictionary<Language, string>()
				{
					{Language.English, "Model Importer active"},
					{Language.Russian, "Model Importer включён"},
				};
			
			public static string ActiveLabel { get { return _activeLabel[CurrentLanguage]; } }
		}

		public class WrongScaleWindow
		{
			private static Dictionary<Language, string> _helpBoxMessage =
				new Dictionary<Language, string>()
				{
					{Language.Russian, "В модели имеются объекты, которые имеют скейл отличный от (1, 1, 1). Импортирование будет отменено."},
					{Language.English, "Model contains some objects wich have scale isn't equal to (1, 1, 1). Importing will be canceled."},
				};
			
			public static string HelpBoxMessage{get { return _helpBoxMessage[CurrentLanguage]; }}
		}
	}
}