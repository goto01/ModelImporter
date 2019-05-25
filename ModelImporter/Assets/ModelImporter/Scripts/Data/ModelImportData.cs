using System;
using System.Collections.Generic;
using UnityEngine;

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
		}

		[SerializeField] private List<AnimationData> _animationsData;

		public int AnimationsNumber { get { return _animationsData.Count; } }
		public List<AnimationData> AnimationsData{get { return _animationsData; }}
		
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
	}
}