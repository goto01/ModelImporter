﻿using System;
using Editor.Windows.DialogWindows;
using ModelImporter.Data;
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
				SetAnimatorControllerToModel(model, _animatorController.Animator);
				return;
			}
			if (!ModelImportDataHelper.CheckGameObjectForScale(model))
			{
				Dialog.ShowDialog<YesNoDialogWindow>("Scale error", DialogType.Yes).Message = "Scale error";
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
			_state = SkipImportAfterReimportState;
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
			if (sender.SetAnimatorControllerInModel)
			{
				_state = SetAnimatorState;
				_animatorController = animatorController;
				AssetDatabase.ImportAsset(sender.Path, ImportAssetOptions.ForceUpdate);
			}
		}

		private void SetAnimatorControllerToModel(GameObject gameObject, RuntimeAnimatorController animatorController)
		{
			ModelImportDataHelper.SetAnimatorControllerToModel(gameObject,
				animatorController);
			EditorUtility.SetDirty(gameObject);
		}
	}
}