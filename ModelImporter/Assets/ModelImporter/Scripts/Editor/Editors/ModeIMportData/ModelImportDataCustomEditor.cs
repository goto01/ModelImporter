using ModelImporter.Data;
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
			ModelImportDataEditor.Draw(ModelImportData, ref _currentTab, ref _scroll);
			EditorUtility.SetDirty(ModelImportData);
		}
	}
}