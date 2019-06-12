using ModelImporter.Data;
using ModelImporter.Editor.Helper.Animator;
using UnityEditor;
using UnityEngine;

namespace ModelImporter.Editor.Editors.ModeImportData
{
	public static class ModelImportDataEditor
	{
		public enum Tab
		{
			Model,
			Animations,
			Materials,
			Scale,
		}
		
		public static void Draw(ModelImportData mid, ref Tab tab, ref Vector2 scroll)
		{	
			DrawHeader(ref tab);
			DrawSelectedTab(mid, ref tab, ref scroll);
		}

		private static void DrawHeader(ref Tab tab)
		{
			DrawTabsEditor(ref tab);
		}
		
		private static void DrawTabsEditor(ref Tab currentTab)
		{
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Toggle(currentTab == Tab.Model, TextResourcesHelper.ModelImportDataDialogWindow.ModelTab,
				EditorStyles.miniButtonLeft))
				currentTab = Tab.Model;
			if (GUILayout.Toggle(currentTab == Tab.Animations, TextResourcesHelper.ModelImportDataDialogWindow.AnimationsTab,
				EditorStyles.miniButtonMid))
				currentTab = Tab.Animations;
			if (GUILayout.Toggle(currentTab == Tab.Materials, TextResourcesHelper.ModelImportDataDialogWindow.MaterialsTab,
				EditorStyles.miniButtonRight))
				currentTab = Tab.Materials;
			EditorGUILayout.EndHorizontal();
		}
		

		private static void DrawSelectedTab(ModelImportData mid, ref Tab tab, ref Vector2 scroll)
		{
			if (tab == Tab.Model) DrawModelEditor(mid);
			else if (tab == Tab.Animations) DrawAnimationsEditor(mid, ref scroll);
			else if (tab == Tab.Materials) DrawMaterialEditor(mid);
		}

		private static void DrawModelEditor(ModelImportData mid)
		{
			mid.Normals = (ModelImporterNormals)EditorGUILayout.EnumPopup("Normals", 
				mid.Normals);
			GUI.enabled = mid.Normals == ModelImporterNormals.Calculate;
			mid.NormalsMode = (ModelImporterNormalCalculationMode)EditorGUILayout.EnumPopup("Normals Mode", 
				mid.NormalsMode);
			mid.SmoothingAngle = EditorGUILayout.Slider("Smoothing angle", mid.SmoothingAngle, 0, 180);
			GUI.enabled = true;
			GUILayout.FlexibleSpace();
		}
		
		private static void DrawAnimationsEditor(ModelImportData mid, ref Vector2 scroll)
		{
			EditorGUILayout.Space();
			EditorGUILayout.BeginVertical(GUI.skin.box);
			EditorGUILayout.LabelField(TextResourcesHelper.ModelImportDataDialogWindow.AnimationsLabel);
			scroll = EditorGUILayout.BeginScrollView(scroll);
			var animations = mid.AnimationsData;
			for (var index = 0; index < animations.Count; index++)
			{
				EditorGUILayout.BeginHorizontal(GUI.skin.box);
				DrawAnimationEditor(mid, animations[index], index);
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}

		private static void DrawAnimationEditor(ModelImportData mid,
			ModelImportData.AnimationData animation, int index)
		{
			EditorGUILayout.LabelField(index.ToString(), GUILayout.Width(20));
			GUILayout.Label(animation.Name, EditorStyles.label);
			DrawAnimationSettings(mid, animation);
		}

		private static void DrawAnimationSettings(ModelImportData mid,
			ModelImportData.AnimationData animation)
		{
			var animationData = mid.GetAnimationData(animation.Name);
			EditorGUILayout.LabelField("Loop time", GUILayout.Width(70));
			animation.LoopTime = animationData.LoopTime = EditorGUILayout.Toggle(animationData.LoopTime,
				GUILayout.Width(16));
			EditorGUILayout.LabelField("Loop blend", GUILayout.Width(70));
			animation.LoopPose = animationData.LoopPose = EditorGUILayout.Toggle(animationData.LoopPose,
				GUILayout.Width(16));
		}

		private static void DrawMaterialEditor(ModelImportData mid)
		{
			EditorGUILayout.HelpBox(TextResourcesHelper.ModelImportDataDialogWindow.MaterialsMessage, MessageType.Info);
			mid.ImportMaterials = EditorGUILayout.Toggle("Import Materials", mid.ImportMaterials);
			mid.MaterialLocation = (ModelImporterMaterialLocation)EditorGUILayout.EnumPopup("Location",
				mid.MaterialLocation);
			mid.MaterialsNaming = (ModelImporterMaterialName)EditorGUILayout.EnumPopup("Naming",
				mid.MaterialsNaming);
			mid.MaterialsSearch = (ModelImporterMaterialSearch)EditorGUILayout.EnumPopup("Search",
				mid.MaterialsSearch);
			GUILayout.FlexibleSpace();
		}
	}
}