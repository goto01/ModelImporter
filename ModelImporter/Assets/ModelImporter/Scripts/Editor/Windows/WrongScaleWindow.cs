using System.Collections.Generic;
using ModelImporter.Editor.Helper.Animator;
using UnityEditor;
using UnityEngine;

namespace ModelImporter.Editor.Windows
{
	public class WrongScaleWindow : ModelImporterBaseDialogWindow<WrongScaleWindow>
	{
		private struct GameObjectDescription
		{
			public string Name;
			public Vector3 Scale;
		}
		
		private const int Width = 500;
		private const int Height = 400;

		private List<GameObjectDescription> _gameObjectsWithWrongScale;
		private Vector2 _scrollRect;
		
		protected override string WindowTitle { get { return "Wrong scale window"; } }

		protected override void DrawContentEditor()
		{
			_size = new Vector2(Width, Height);
			base.DrawContentEditor();
			UpdatePosition();
			_centerInParentWindow = false;
			DrawEditor();
		}

		public void Initialize(List<GameObject> gameObjectsWithWrongScale, string assetPath)
		{
			_gameObjectsWithWrongScale = new List<GameObjectDescription>();
			for (var index = 0; index < gameObjectsWithWrongScale.Count; index++)
				_gameObjectsWithWrongScale.Add(new GameObjectDescription()
				{
					Name = gameObjectsWithWrongScale[index].name,
					Scale = gameObjectsWithWrongScale[index].transform.lossyScale,
				});
		}

		private void DrawEditor()
		{
			EditorGUILayout.HelpBox(TextResourcesHelper.WrongScaleWindow.HelpBoxMessage, MessageType.Error);
			EditorGUILayout.Space();
			DrawGameObjectsWithWrongScale();
		}

		private void DrawGameObjectsWithWrongScale()
		{
			_scrollRect = EditorGUILayout.BeginScrollView(_scrollRect);
			for (var index = 0; index < _gameObjectsWithWrongScale.Count; index++)
			{
				EditorGUILayout.BeginHorizontal(GUI.skin.box);
				EditorGUILayout.LabelField(_gameObjectsWithWrongScale[index].Name);
				GUI.enabled = false;
				EditorGUILayout.Vector3Field(string.Empty, _gameObjectsWithWrongScale[index].Scale);
				GUI.enabled = true;
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();
		}
	}
}