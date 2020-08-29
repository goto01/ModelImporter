using UnityEditor;

namespace ModelImporter.Editor.Helper
{
	public static class PrefsHelper
	{
		private const float DefaultValue = .001f;
		private const string ModelImporterActivateName = "ModelImporterActivate";
		private const string ModelImporterScaleDefaultEpsilonName = "ModelImporterScaleDefaultEpsilon";
		private const string ModelImporterPositionMaxMagnitudeName = "ModelImporterPositionMaxMagnitude";
		private const string ModelImporterRotationMaxMagnitudeName = "ModelImporterRotationMaxMagnitude";

		public static bool ModeImporterActive
		{
			get { return EditorPrefs.GetBool(ModelImporterActivateName, true); }
			set { EditorPrefs.SetBool(ModelImporterActivateName, value); }
		}

		public static float ModelImporterScaleDefaultEpsilon
		{
			get { return EditorPrefs.GetFloat(ModelImporterScaleDefaultEpsilonName, DefaultValue); }
			set { EditorPrefs.SetFloat(ModelImporterScaleDefaultEpsilonName, value); }
		}

		public static float ModelImporterPositionMaxMagnitude
		{
			get { return EditorPrefs.GetFloat(ModelImporterPositionMaxMagnitudeName, DefaultValue); } 
			set { EditorPrefs.SetFloat(ModelImporterPositionMaxMagnitudeName, value); }
		}
		
		public static float ModelImporterRotationMaxMagnitude
		{
			get { return EditorPrefs.GetFloat(ModelImporterRotationMaxMagnitudeName, DefaultValue); } 
			set { EditorPrefs.SetFloat(ModelImporterRotationMaxMagnitudeName, value); }
		}
	}
}