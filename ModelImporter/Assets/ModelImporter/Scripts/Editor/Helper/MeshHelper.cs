using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ModelImporter.Data;
using UnityEditor;
using UnityEngine;

namespace ModelImporter.Editor.Helper
{
	public static class MeshHelper
	{
		public static void CopyMesh(GameObject gameObject)
		{
			var assetPath = AssetDatabase.GetAssetPath(gameObject);
			InitializeDirectory(assetPath);
			var renderers = gameObject.GetComponentsInChildren<Renderer>();
			for (var index = 0; index < renderers.Length; index++)
				if (renderers[index] is MeshRenderer) HandleRenderer((MeshRenderer) renderers[index], assetPath);
				else HandleSkinnedRenderer((SkinnedMeshRenderer) renderers[index], assetPath);
		}

		private static void HandleRenderer(MeshRenderer renderer, string assetPath)
		{
			var mesh = renderer.GetComponent<MeshFilter>().sharedMesh;
			CreateMesh(mesh, assetPath);
		}
		
		private static void HandleSkinnedRenderer(SkinnedMeshRenderer renderer, string assetPath)
		{
			var mesh = renderer.sharedMesh;
			CreateMesh(mesh, assetPath);
		}

		private static void CreateMesh(Mesh mesh, string assetPath)
		{
			var path = GetMeshPath(assetPath, mesh.name);
			var newMesh = GetMesh(path);
			newMesh.Clear();
			newMesh.vertices = mesh.vertices.ToArray();
			newMesh.triangles = mesh.triangles.ToArray();
			newMesh.normals = mesh.normals.ToArray();
			newMesh.tangents = mesh.tangents.ToArray();
			newMesh.uv = mesh.uv;
			EditorUtility.SetDirty(newMesh);
		}
		
#region Bake normal to color
		
		public static void BakeNormalsToColor(GameObject gameObject)
		{
			var assetPath = AssetDatabase.GetAssetPath(gameObject);
			InitializeDirectory(assetPath);
			var renderers = gameObject.GetComponentsInChildren<Renderer>();
			for (var index = 0; index < renderers.Length; index++)
				if (renderers[index] is MeshRenderer) BakeNormalRenderer((MeshRenderer) renderers[index], assetPath);
				else BakeNormalSkinnedRenderer((SkinnedMeshRenderer) renderers[index], assetPath);
		}
		
		private static void BakeNormalRenderer(MeshRenderer renderer, string assetPath)
		{
			var mesh = renderer.GetComponent<MeshFilter>().sharedMesh;
			BakeMesh(mesh, assetPath);
		}
		
		private static void BakeNormalSkinnedRenderer(SkinnedMeshRenderer renderer, string assetPath)
		{
			var mesh = renderer.sharedMesh;
			BakeMesh(mesh, assetPath);
		}

		private static void BakeMesh(Mesh mesh, string assetPath)
		{
			var path = GetMeshPath(assetPath, mesh.name);
			var newMesh = GetMesh(path);
			newMesh.colors = mesh.normals.Select(x => new Color(x.x, x.y, x.z, 1)).ToArray();
			EditorUtility.SetDirty(newMesh);
		}
		
#endregion
	

		private static Mesh GetMesh(string path)
		{
			var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
			Debug.Log(mesh);
			if (mesh != null) return mesh;
			mesh = new Mesh();
			AssetDatabase.CreateAsset(mesh, path);
			return mesh;
		}

		private static void InitializeDirectory(string assetPath)
		{
			var directory = Path.GetDirectoryName(assetPath);
			var folder = string.Format("{0}/Meshes", directory);
			if (!AssetDatabase.IsValidFolder(folder)) AssetDatabase.CreateFolder(directory, "Meshes");
		}
		
		private static string GetMeshPath(string assetPath, string name)
		{
			var directory = Path.GetDirectoryName(assetPath);
			return string.Format("{0}/Meshes/{1}.mesh", directory, name);
		}
	}
}