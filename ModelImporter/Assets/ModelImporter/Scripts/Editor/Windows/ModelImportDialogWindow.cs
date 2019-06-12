using Editor.Windows.DialogWindows;
using ModelImporter.Data;
using ModelImporter.Editor.Editors.ModeImportData;
using ModelImporter.Editor.Helper.Animator;
using UnityEditor;
using UnityEngine;

namespace ModelImporter.Editor.Windows
{
	public class ModelImportDialogWindow : ModelImporterBaseDialogWindow<ModelImportDialogWindow>
	{
		private const int Width = 500;
		private const int Height = 250;
		private UnityEditor.ModelImporter _modelImporter;
		private ModelImportData _modelImportData;
		private Vector2 _scroll;
		private ModelImportDataEditor.Tab _currentTab;
		
		protected override string WindowTitle { get { return string.Format("Model Import Data(MID) - {0}", _modelImportData.name); } }
		public UnityEditor.ModelImporter ModelImporter { get { return _modelImporter; } }
		public ModelImportData ModelImportData{get { return _modelImportData; }}
		public string ModelPath { get; private set; }

		public void Initialize(UnityEditor.ModelImporter modelImporter, ModelImportData mid, string modelPath)
		{
			ModelPath = modelPath;
			_modelImporter = modelImporter;
			_modelImportData = mid;
		}

		protected override void DrawContentEditor()
		{
			_size = new Vector2(Width, Height);
			base.DrawContentEditor();
			UpdatePosition();
			_centerInParentWindow = false;
			EditorGUILayout.HelpBox(TextResourcesHelper.ModelImportDataDialogWindow.NewMidMessage, 
				MessageType.Info);
			ModelImportDataEditor.Draw(_modelImportData, ref _currentTab, ref _scroll);
		}
	}
}