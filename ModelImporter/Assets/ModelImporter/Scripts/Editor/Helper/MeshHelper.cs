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
			var meshes = new List<Mesh>();
			for (var index = 0; index < renderers.Length; index++)
				if (renderers[index] is MeshRenderer) meshes.Add(HandleRenderer((MeshRenderer) renderers[index], assetPath));
				else meshes.Add(HandleSkinnedRenderer((SkinnedMeshRenderer) renderers[index], assetPath));
			BakeNormalsToColor(meshes);
			EditorUtility.SetDirty(gameObject);
		}

		private static Mesh HandleRenderer(MeshRenderer renderer, string assetPath)
		{
			var meshFilter = renderer.GetComponent<MeshFilter>();
			var mesh = meshFilter.sharedMesh;
			var newMesh = CreateMesh(mesh, assetPath);
			meshFilter.sharedMesh = newMesh;
			return newMesh;
		}
		
		private static Mesh HandleSkinnedRenderer(SkinnedMeshRenderer renderer, string assetPath)
		{
			var mesh = renderer.sharedMesh;
			var newMesh = CreateMesh(mesh, assetPath);
			renderer.sharedMesh = newMesh;
			return newMesh;
		}

		private static Mesh CreateMesh(Mesh mesh, string assetPath)
		{
			var path = GetMeshPath(assetPath, mesh.name);
			var newMesh = GetMesh(path);
			newMesh.Clear();
			newMesh.vertices = mesh.vertices.ToArray();
			newMesh.triangles = mesh.triangles.ToArray();
			newMesh.normals = mesh.normals.ToArray();
			newMesh.tangents = mesh.tangents.ToArray();
			newMesh.colors = mesh.colors.ToArray();
			newMesh.uv = mesh.uv;
			newMesh.boneWeights = mesh.boneWeights;
			newMesh.bindposes = mesh.bindposes;
			newMesh.bounds = mesh.bounds;
			EditorUtility.SetDirty(newMesh);
			return newMesh;
		}

		private static Mesh GetMesh(string path)
		{
			var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
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
			name = name.Replace(":", "_");
			var directory = Path.GetDirectoryName(assetPath);
			return string.Format("{0}/Meshes/{1}.mesh", directory, name);
		}

		private static void BakeNormalsToColor(List<Mesh> meshes)
		{
			for (var index = 0; index < meshes.Count; index++)
				BakeNormalsToColor(meshes[index]);
		}

		private static void BakeNormalsToColor(Mesh mesh)
		{
			var vertices = mesh.vertices;
			var normals = mesh.normals;
			var colors = new Vector3[normals.Length];
			for (var index0 = 0; index0 < normals.Length; index0++)
				for (var index1 = 0; index1 < normals.Length; index1++)
				{
					if (Vector3.Distance(vertices[index0], vertices[index1]) > Mathf.Epsilon) continue;
					var normal0 = colors[index0].magnitude > Mathf.Epsilon ? colors[index0] : 
						normals[index0];
					var normal1 = colors[index1].magnitude > Mathf.Epsilon ? colors[index1] : 
						normals[index1];
					var normal = normal0 + normal1;
					colors[index0] = colors[index1] = normal.normalized;
				}
			mesh.colors = colors.Select(x=> new Color(x.x, x.y, x.z)).ToArray();
			EditorUtility.SetDirty(mesh);
		}
	}
}