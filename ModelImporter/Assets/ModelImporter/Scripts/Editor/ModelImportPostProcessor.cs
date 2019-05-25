using System;
using Editor.Windows.DialogWindows;
using ModelImporter.Data;
using ModelImporter.Editor.Helper;
using ModelImporter.Editor.Windows;
using UnityEditor;
using UnityEngine;

namespace ModelImporter.Editor
{
	public class ModelImportPostProcessor : AssetPostprocessor
	{
		private UnityEditor.ModelImporter ModelImporter{get { return (UnityEditor.ModelImporter) assetImporter; }}
		
		private void OnPostprocessModel(GameObject model)
		{
			var modelImportData = ModelImportDataHelper.LoadModeImportData(assetPath);
			if (modelImportData != null)
			{
				HandleModelImportData(modelImportData);
				return;
			}
			var mid = ModelImportDataHelper.CreateModelImportData(assetPath);
			HandleModelImportData(mid);
		}

		private void HandleModelImportData(ModelImportData mid)
		{
			if (ModelImportDataHelper.CheckModeImportDataForFull(mid, ModelImporter))
			{
				ModelImportDataHelper.SetModelImporterImportSettings(ModelImporter, mid);
				return;
			}
			ModelImportDataHelper.FillModelImportDataSettings(ModelImporter, mid);
			var window = Dialog.ShowDialog<ModelImportDialogWindow>("Model importer", DialogType.Yes);
			window.Initialize(ModelImporter, mid);
			window.Yes += OnModelImporteDialogWindowYes;
		}

		private void OnModelImporteDialogWindowYes(ModelImportDialogWindow sender)
		{
			EditorUtility.SetDirty(sender.ModelImportData);
			ModelImportDataHelper.SetModelImporterImportSettings(sender.ModelImporter, sender.ModelImportData);
			sender.ModelImporter.SaveAndReimport();
		}
	}
}