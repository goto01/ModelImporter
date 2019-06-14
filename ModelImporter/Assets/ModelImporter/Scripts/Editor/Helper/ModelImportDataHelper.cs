using System.Collections.Generic;
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
		
		public static List<GameObject> GetGameObjectsWithWrongScale(GameObject gameObject)
		{
			var gameObjects = new List<GameObject>();  
			CheckChildTransforms(gameObject.transform, gameObjects, 0);
			return gameObjects;
		}

		private static void CheckChildTransforms(Transform transform, List<GameObject> gameObjects, int depth)
		{
			if (depth > MaxDepth)
			{
				Debug.LogError("Infinite rec or rec depth limit exceeds");
				return;
			}
			if (Vector3.Distance(Vector3.one, transform.localScale) > Mathf.Epsilon)
			{
				gameObjects.Add(transform.gameObject);
				return;
			}
			for (var index = 0; index < transform.childCount; index++)
				CheckChildTransforms(transform.GetChild(index), gameObjects, depth + 1);
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