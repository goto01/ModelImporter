using System.IO;
using ModelImporter.Data;
using UnityEditor;
using UnityEngine;

namespace ModelImporter.Editor.Helper
{
	public static class ModelImportDataHelper
	{
		private const string PostFix = "MID";
		
		public static ModelImportData LoadModeImportData(string assetPath)
		{
			var path = GetModeImportDataPath(assetPath);
			return AssetDatabase.LoadAssetAtPath<ModelImportData>(path);
		}

		public static ModelImportData CreateModelImportData(string assetPath)
		{
			var mid = ScriptableObject.CreateInstance<ModelImportData>();
			mid.Initialize();
			var path = GetModeImportDataPath(assetPath);
			AssetDatabase.CreateAsset(mid, path);
			return mid;
		}
		
		public static bool CheckModeImportDataForFull(ModelImportData mid, UnityEditor.ModelImporter modelImporter)
		{
			var counter = 0;
			var animations = modelImporter.defaultClipAnimations;
			for (var index = 0; index < animations.Length; index++)
				if (mid.ContainsAnimation(animations[index].name)) counter++;
			return counter == animations.Length;
		}

		public static void FillModelImportDataSettings(UnityEditor.ModelImporter modelImporter, ModelImportData mid)
		{
			var animations = modelImporter.defaultClipAnimations;
			for (var index = 0; index < animations.Length; index++)
			{
				if (mid.ContainsAnimation(animations[index].name)) continue;
				mid.AddAnimationData(animations[index].name, false, false);
			}
			EditorUtility.SetDirty(mid);
		}

		public static void SetModelImporterImportSettings(UnityEditor.ModelImporter modelImporter, ModelImportData mid)
		{
			modelImporter.clipAnimations = GetImportedAnimationClips(modelImporter.defaultClipAnimations, mid);
			SetModelSettings(modelImporter, mid);
			SetMaterialsSettings(modelImporter, mid);
		}
		
		private static ModelImporterClipAnimation[] GetImportedAnimationClips(ModelImporterClipAnimation[] animations, 
			ModelImportData modelImportData)
		{
			for (var index = 0; index < animations.Length; index++)
			{
				var animationData = modelImportData.GetAnimationData(animations[index].name);
				animations[index].loopTime = animationData.LoopTime;
				animations[index].loopPose= animationData.LoopPose;
			}
			return animations;
		}

		private static string GetModeImportDataPath(string assetPath)
		{
			var fileName = Path.GetFileNameWithoutExtension(assetPath);
			var directory = Path.GetDirectoryName(assetPath);
			return string.Format("{0}/{1}{2}.asset", directory, fileName, PostFix);
		}

		private static void SetModelSettings(UnityEditor.ModelImporter modelImporter, ModelImportData mid)
		{
			modelImporter.importNormals = mid.Normals;
			modelImporter.normalCalculationMode = mid.NormalsMode;
			modelImporter.normalSmoothingAngle = mid.SmoothingAngle;
		}

		private static void SetMaterialsSettings(UnityEditor.ModelImporter modelImporter, ModelImportData mid)
		{
			modelImporter.importMaterials = mid.ImportMaterials;
			modelImporter.materialLocation = mid.MaterialLocation;
			modelImporter.materialName = mid.MaterialsNaming;
			modelImporter.materialSearch = mid.MaterialsSearch;
		}
	}
}