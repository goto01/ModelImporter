using UnityEditor;

namespace ModelImporter.Editor.Helper.Animator
{
	public static class PrefsHelper
	{
		private const float ScaleDefaultEpsilon = .001f;
		private const string ModelImporterActivateName = "ModelImporterActivate";
		private const string ModelImporterScaleDefaultEpsilonName = "ModelImporterScaleDefaultEpsilon";

		public static bool ModeImporterActive
		{
			get { return EditorPrefs.GetBool(ModelImporterActivateName, true); }
			set { EditorPrefs.SetBool(ModelImporterActivateName, value); }
		}

		public static float ModelImporterScaleDefaultEpsilon
		{
			get { return EditorPrefs.GetFloat(ModelImporterScaleDefaultEpsilonName, ScaleDefaultEpsilon); }
			set { EditorPrefs.SetFloat(ModelImporterScaleDefaultEpsilonName, value); }
		}
	}
}