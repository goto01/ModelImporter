using System.IO;
using ModelImporter.Data;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace ModelImporter.Editor.Helper.Animator
{
	public static class ModelImportDataHelper
	{
		private const int MaxDepth = 1000;
		private const string PostFix = "MID";

#region Check for scale		
		
		public static bool CheckGameObjectForScale(GameObject gameObject)
		{
			return CheckChildTransforms(gameObject.transform, 0);
		}

		private static bool CheckChildTransforms(Transform transform, int depth)
		{
			if (depth > MaxDepth)
			{
				Debug.LogError("Infinite rec or rec depth limit exceeds");
				return false;
			}
			if (Vector3.Distance(Vector3.one, transform.localScale) > Mathf.Epsilon) return false;
			for (var index = 0; index < transform.childCount; index++)
				if (!CheckChildTransforms(transform.GetChild(index), depth + 1)) return false;
			return true;
		}
		
#endregion		
		
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

		public static void SetAnimatorControllerToModel(GameObject model, RuntimeAnimatorController animator)
		{
			model.GetComponent<UnityEngine.Animator>().runtimeAnimatorController = animator;
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