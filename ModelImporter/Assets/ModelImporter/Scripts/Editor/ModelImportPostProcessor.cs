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
		private static bool _skipImportAfterReimport = false;
		private UnityEditor.ModelImporter ModelImporter{get { return (UnityEditor.ModelImporter) assetImporter; }}
		
		private void OnPostprocessModel(GameObject model)
		{
			if (_skipImportAfterReimport)
			{
				_skipImportAfterReimport = false;
				return;
			}
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
			}
			else ModelImportDataHelper.FillModelImportDataSettings(ModelImporter, mid);
			var window = Dialog.ShowDialog<ModelImportDialogWindow>("Model importer", DialogType.Yes);
			window.Initialize(ModelImporter, mid);
			window.Yes += OnModelImporteDialogWindowYes;
		}

		private void OnModelImporteDialogWindowYes(ModelImportDialogWindow sender)
		{
			EditorUtility.SetDirty(sender.ModelImportData);
			ModelImportDataHelper.SetModelImporterImportSettings(sender.ModelImporter, sender.ModelImportData);
			_skipImportAfterReimport = true;
			AssetDatabase.ImportAsset(sender.ModelImporter.assetPath, ImportAssetOptions.ForceUpdate);
			GenerateAnimatorIfRequired(sender.ModelImporter, sender.ModelImportData);
		}

		private void GenerateAnimatorIfRequired(UnityEditor.ModelImporter modelImporter, ModelImportData mid)
		{
			if (mid.AnimationsNumber == 0) return;
			var animatorGeneratorWindow = Dialog.ShowDialog<AnimatorGeneratorDialogWindow>("Animator generator", DialogType.YesNo);
			animatorGeneratorWindow.Initialize(modelImporter.assetPath);
			animatorGeneratorWindow.Yes += AnimatorGeneratorWindowOnYes;
		}

		private void AnimatorGeneratorWindowOnYes(AnimatorGeneratorDialogWindow sender)
		{
			var animatorController = AnimatorHelper.GetAnimatorController(sender.Path);
			AnimatorHelper.ReplaceAnimator(animatorController, sender.Animations);
		}
	}
}