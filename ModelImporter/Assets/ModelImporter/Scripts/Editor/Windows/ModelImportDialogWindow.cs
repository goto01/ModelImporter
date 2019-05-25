using Editor.Windows.DialogWindows;
using ModelImporter.Data;
using UnityEditor;
using UnityEngine;

namespace ModelImporter.Editor.Windows
{
	public class ModelImportDialogWindow : BaseDialogWindow<ModelImportDialogWindow>
	{
		private const int Width = 500;
		private const int Height = 250;
		private UnityEditor.ModelImporter _modelImporter;
		private ModelImportData _modelImportData;
		private bool _initialized;
		private Vector2 _scroll;
		
		public UnityEditor.ModelImporter ModelImporter { get { return _modelImporter; } }
		public ModelImportData ModelImportData{get { return _modelImportData; }}

		public void Initialize(UnityEditor.ModelImporter modelImporter, ModelImportData mid)
		{
			_modelImporter = modelImporter;
			_modelImportData = mid;
		}
		
		protected override void DrawContentEditor()
		{
			_size = new Vector2(Width, Height);
			UpdatePosition();
			_centerInParentWindow = false;
			DrawAnimationsEditor();
		}
		
		private void DrawAnimationsEditor()
		{
			EditorGUILayout.Space();
			EditorGUILayout.BeginVertical(GUI.skin.box);
			var animations = _modelImportData.AnimationsData;
			for (var index = 0; index < animations.Count; index++)
			{
				EditorGUILayout.BeginHorizontal();
				DrawAnimationEditor(animations[index], index);
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();
		}

		private void DrawAnimationEditor(ModelImportData.AnimationData animation, int index)
		{
			EditorGUILayout.LabelField(index.ToString(), GUILayout.Width(20));
			if (GUILayout.Button(animation.Name)) { }
			DrawAnimationSettings(animation);
		}

		private void DrawAnimationSettings(ModelImportData.AnimationData animation)
		{
			var animationData = _modelImportData.GetAnimationData(animation.Name);
			EditorGUILayout.LabelField("Loop time", GUILayout.Width(70));
			animation.LoopTime = animationData.LoopTime = EditorGUILayout.Toggle(animationData.LoopTime,
				GUILayout.Width(16));
			EditorGUILayout.LabelField("Loop blend", GUILayout.Width(70));
			animation.LoopPose = animationData.LoopPose = EditorGUILayout.Toggle(animationData.LoopPose,
				GUILayout.Width(16));
		}
	}
}