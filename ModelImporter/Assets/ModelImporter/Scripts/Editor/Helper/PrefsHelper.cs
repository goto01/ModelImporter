using UnityEditor;

namespace ModelImporter.Editor.Helper
{
	public static class PrefsHelper
	{
		private const string ModelImporterActivateName = "ModelImporterActivate";

		public static bool ModeImporterActive
		{
			get { return EditorPrefs.GetBool(ModelImporterActivateName, true); }
			set { EditorPrefs.SetBool(ModelImporterActivateName, value); }
		}
	}
}