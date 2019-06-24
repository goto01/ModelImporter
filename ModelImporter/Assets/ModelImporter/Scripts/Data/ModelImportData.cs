using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ModelImporter.Data
{
	public class ModelImportData : ScriptableObject
	{
		[Serializable]
		public class AnimationData
		{
			public string Name;
			public bool LoopTime;
			public bool LoopPose;
			public bool Default;
		}

#if UNITY_EDITOR
		[SerializeField] private bool _generateMesh;
		[SerializeField] private ModelImporterNormals _normals = ModelImporterNormals.Import;
		[SerializeField] private ModelImporterNormalCalculationMode _normalsMode;
		[SerializeField] private float _smoothingAngle = 180;
		[SerializeField] private List<AnimationData> _animationsData;
		[SerializeField] private bool _importMaterials = true;
		[SerializeField] private ModelImporterMaterialLocation _materialsLocation = ModelImporterMaterialLocation.External;
		[SerializeField] private ModelImporterMaterialName _materialsNaming = ModelImporterMaterialName.BasedOnMaterialName;
		[SerializeField] private ModelImporterMaterialSearch _materialsSearch = ModelImporterMaterialSearch.Local;

		public int AnimationsNumber { get { return _animationsData.Count; } }
		public List<AnimationData> AnimationsData{get { return _animationsData; }}

		public bool GenerateMesh
		{
			get { return _generateMesh; }
			set { _generateMesh = value; }
		}
		public ModelImporterNormals Normals
		{
			get { return _normals; }
			set { _normals = value; }
		}
		public ModelImporterNormalCalculationMode NormalsMode
		{
			get { return _normalsMode; }
			set { _normalsMode = value; }
		}
		public float SmoothingAngle
		{
			get { return _smoothingAngle; }
			set { _smoothingAngle = value; }
		}

		public bool ImportMaterials
		{
			get { return _importMaterials; }
			set { _importMaterials = value; }
		}
		public ModelImporterMaterialLocation MaterialLocation
		{
			get { return _materialsLocation; }
			set { _materialsLocation = value; }
		}
		public ModelImporterMaterialName MaterialsNaming
		{
			get { return _materialsNaming; }
			set { _materialsNaming = value; }
		}
		public ModelImporterMaterialSearch MaterialsSearch
		{
			get { return _materialsSearch; }
			set { _materialsSearch = value; }
		}
		
		public void Initialize()
		{
			_animationsData = new List<AnimationData>();
		}

		public AnimationData GetAnimationData(string animationName)
		{
			for (var index = 0; index < _animationsData.Count; index++)
				if (_animationsData[index].Name == animationName) return _animationsData[index];
			return default(AnimationData);
		}
		
		public void AddAnimationData(string animationName, bool loopTime, bool loopPos)
		{
			_animationsData.Add(new AnimationData()
			{
				Name = animationName,
				LoopTime = loopTime,
				LoopPose = loopPos
			});
		}

		public bool ContainsAnimation(string animationName)
		{
			for (var index = 0; index < _animationsData.Count; index++)
				if (_animationsData[index].Name == animationName)
					return true;
			return false;
		}
#endif		
	}
}