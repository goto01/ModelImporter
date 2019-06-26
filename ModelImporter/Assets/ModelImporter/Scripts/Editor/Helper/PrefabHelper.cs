using System.IO;
using UnityEditor;
using UnityEngine;

namespace ModelImporter.Editor.Helper
{
	public static class PrefabHelper
	{
		public static GameObject CreateOrReplacePrefab(string modelPath)
		{
			var prefabPath = GetPrefabPath(modelPath);
			var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
			var model = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
			if (prefab != null) prefab = ReplacePrefab(model, prefab); 
			else prefab = CreatePrefab(model, prefabPath);
			return prefab;
		}
		
		public static void SetAnimatorControllerToPrefab(string modelPath, RuntimeAnimatorController animator)
		{
			var prefabPath = GetPrefabPath(modelPath);
			var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
			prefab.GetComponent<UnityEngine.Animator>().runtimeAnimatorController = animator;
			EditorUtility.SetDirty(prefab);
		}

		private static string GetPrefabPath(string modelPath)
		{
			var fileName = Path.GetFileNameWithoutExtension(modelPath);
			var directory = Path.GetDirectoryName(modelPath);
			return string.Format("{0}/{1}.prefab", directory, fileName);
		}

		private static GameObject ReplacePrefab(GameObject model, GameObject prefab)
		{
			ComponentsHelper.MakeSnapshot(prefab);
			prefab =  PrefabUtility.ReplacePrefab(model, prefab, ReplacePrefabOptions.ConnectToPrefab);
			ComponentsHelper.SetSnapshot(prefab);
			return prefab;
		}
		
		private static GameObject CreatePrefab(GameObject model, string prefabPath)
		{
			var prefab = PrefabUtility.CreatePrefab(prefabPath, model);
			EditorUtility.SetDirty(prefab);
			return prefab;
		}
	}
}