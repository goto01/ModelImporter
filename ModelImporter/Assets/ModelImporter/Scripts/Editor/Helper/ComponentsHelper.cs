using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ModelImporter.Editor.Helper
{
	public static class ComponentsHelper
	{
		enum ColliderType
		{
			BoxCollider,
			SphereCollider,
			CapsuleCollider,
			None,
		}

		struct ColliderData
		{
			public bool IsTrigger;
			public PhysicMaterial PhysicMaterial;
			//Box
			public Vector3 Center;
			public Vector3 Size;
			//Sphere
			public float Radius;
			//Capsule
			public float Height;
			public int Direction;
		}
		
		private static readonly Dictionary<Type, ColliderType> _colliderTypes = new Dictionary<Type, ColliderType>()
		{
			{ typeof(BoxCollider), ColliderType.BoxCollider },
			{ typeof(SphereCollider), ColliderType.SphereCollider },
			{ typeof(CapsuleCollider), ColliderType.CapsuleCollider },
		};
		private static readonly Dictionary<ColliderType, Type> _colliders = new Dictionary<ColliderType, Type>()
		{
			{ ColliderType.BoxCollider, typeof(BoxCollider) },
			{ ColliderType.SphereCollider, typeof(SphereCollider) },
			{ ColliderType.CapsuleCollider, typeof(CapsuleCollider) },
		};
 		
		private static ColliderType _colliderType;
		private static ColliderData _colliderData;
		private static int _layer;
		private static string _tag;

		public static void MakeSnapshot(GameObject gameObject)
		{
			var collider = gameObject.GetComponent<Collider>();
			if (collider == null)
			{
				_colliderType = ColliderType.None;
				return;
			}
			_layer = gameObject.layer;
			_tag = gameObject.tag;
			_colliderType = _colliderTypes[gameObject.GetComponent<Collider>().GetType()];
			_colliderData.IsTrigger = collider.isTrigger;
			_colliderData.PhysicMaterial = collider.sharedMaterial;
			switch (_colliderType)
			{
				case ColliderType.BoxCollider:
					var boxCollider = (BoxCollider)collider;
					_colliderData.Center = boxCollider.center;
					_colliderData.Size = boxCollider.size;
					break;
				case ColliderType.SphereCollider:
					var sphereCollider = (SphereCollider) collider;
					_colliderData.Center = sphereCollider.center;
					_colliderData.Radius = sphereCollider.radius;
					break;
				case ColliderType.CapsuleCollider:
					var capsuleCollider = (CapsuleCollider) collider;
					_colliderData.Center = capsuleCollider.center;
					_colliderData.Radius = capsuleCollider.radius;
					_colliderData.Height = capsuleCollider.height;
					_colliderData.Direction = capsuleCollider.direction;
					break;
			}
		}

		public static void SetSnapshot(GameObject gameObject)
		{
			if (_colliderType == ColliderType.None) return;
			gameObject.layer = _layer;
			gameObject.tag = _tag;
			var collider = (Collider)gameObject.AddComponent(_colliders[_colliderType]);
			collider.isTrigger = _colliderData.IsTrigger;
			collider.sharedMaterial = _colliderData.PhysicMaterial;
			switch (_colliderType)
			{
				case ColliderType.BoxCollider:
					var boxCollider = (BoxCollider) collider;
					boxCollider.center = _colliderData.Center;
					boxCollider.size = _colliderData.Size;
					break;
				case ColliderType.SphereCollider:
					var sphereCollider = (SphereCollider) collider;
					sphereCollider.center = _colliderData.Center;
					sphereCollider.radius = _colliderData.Radius;
					break;
				case ColliderType.CapsuleCollider:
					var capsuleCollider = (CapsuleCollider) collider;
					capsuleCollider.center = _colliderData.Center;
					capsuleCollider.radius = _colliderData.Radius;
					capsuleCollider.height = _colliderData.Height;
					capsuleCollider.direction = _colliderData.Direction;
					break;
			}
			EditorUtility.SetDirty(gameObject);
		}
	}
}