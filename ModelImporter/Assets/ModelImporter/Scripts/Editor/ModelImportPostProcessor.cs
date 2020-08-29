using System;
using Editor.Windows.DialogWindows;
using ModelImporter.Data;
using ModelImporter.Editor.Helper;
using ModelImporter.Editor.Helper.Animator;
using ModelImporter.Editor.Windows;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace ModelImporter.Editor
{
	public class ModelImportPostProcessor : AssetPostprocessor
	{
		private const int DefaultImport = 0;
		private const int SkipImportAfterReimportState = 1;
		private const int SetAnimatorState = 2;

		private static int _state;
		private static AnimatorDescription _animatorController;
		
		private UnityEditor.ModelImporter ModelImporter{get { return (UnityEditor.ModelImporter) assetImporter; }}
		
		private void OnPostprocessModel(GameObject model)
		{
			if (!PrefsHelper.ModeImporterActive) return;
			if (_state == SkipImportAfterReimportState)
			{
				_state = DefaultImport;
				return;
			}
			if (_state == SetAnimatorState)
			{
				_state = DefaultImport;
				SetAnimatorControllerToModel(assetPath, _animatorController.Animator);
				return;
			}
			var gameObjectsWithWrongScale = ModelImportDataHelper.GetGameObjectsWithWrongScale(model);
			if (gameObjectsWithWrongScale.Count > 0)
			{
				var wrongScaleWindow = Dialog.ShowDialog<WrongScaleWindow>("Wrong scale", DialogType.Yes);
				wrongScaleWindow.Initialize(gameObjectsWithWrongScale, assetPath);
				return;
			}
			if (!ModelImportDataHelper.CheckForRootPosition(model))
			{
				Dialog.ShowDialog<WrongPositionOrRotationWindow>("Wrong position", DialogType.Yes);
				return;
			}
			if (!ModelImportDataHelper.CheckForRootRotation(model))
			{
				Dialog.ShowDialog<WrongPositionOrRotationWindow>("Wrong rotation", DialogType.Yes);
				return;
			}
			var modelImportData = ModelImportDataHelper.LoadModeImportData(assetPath);
			if (modelImportData != null)
			{
				HandleModelImportData(modelImportData, assetPath);
				return;
			}
			var mid = ModelImportDataHelper.CreateModelImportData(assetPath);
			HandleModelImportData(mid, assetPath);
			
		}

		private void HandleModelImportData(ModelImportData mid, string modelPath)
		{
			if (ModelImportDataHelper.CheckModeImportDataForFull(mid, ModelImporter))
			{
				ModelImportDataHelper.SetModelImporterImportSettings(ModelImporter, mid);
			}
			else ModelImportDataHelper.FillModelImportDataSettings(ModelImporter, mid);
			var window = Dialog.ShowDialog<ModelImportDialogWindow>("Model importer", DialogType.Yes);
			window.Initialize(ModelImporter, mid, modelPath);
			window.Yes += OnModelImporteDialogWindowYes;
		}

		private void OnModelImporteDialogWindowYes(ModelImportDialogWindow sender)
		{
			EditorUtility.SetDirty(sender.ModelImportData);
			ModelImportDataHelper.SetModelImporterImportSettings(sender.ModelImporter, sender.ModelImportData);
			_state = SkipImportAfterReimportState;
			AssetDatabase.ImportAsset(sender.ModelImporter.assetPath, ImportAssetOptions.ForceUpdate);
			var prefab = PrefabHelper.CreateOrReplacePrefab(sender.ModelPath);
			if (sender.ModelImportData.GenerateMesh) MeshHelper.CopyMesh(prefab);
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
			if (sender.SetAnimatorControllerInModel)
			{
				_state = SetAnimatorState;
				_animatorController = animatorController;
				AssetDatabase.ImportAsset(sender.Path, ImportAssetOptions.ForceUpdate);
			}
		}

		private void SetAnimatorControllerToModel(string modelPath, RuntimeAnimatorController animatorController)
		{
			PrefabHelper.SetAnimatorControllerToPrefab(modelPath,
				animatorController);
		}
	}
}