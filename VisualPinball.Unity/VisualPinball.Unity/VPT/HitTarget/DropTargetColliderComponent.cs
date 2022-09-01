// Visual Pinball Engine
// Copyright (C) 2022 freezy and VPE Team
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <https://www.gnu.org/licenses/>.

// ReSharper disable InconsistentNaming

using System;
using Unity.Entities;
using UnityEngine;
using VisualPinball.Engine.VPT.HitTarget;

namespace VisualPinball.Unity
{
	[AddComponentMenu("Visual Pinball/Collision/Drop Target Collider")]
	[RequireComponent(typeof(DropTargetComponent))]
	public class DropTargetColliderComponent : ColliderComponent<HitTargetData, TargetComponent>
	{
		#region Data

		// Physical Dimensions
		[Min (0f)]
		[Tooltip ("Dimension of the main drop target body collider")]
		public Vector3 Dimensions;

		[Min(0f)]
		[Tooltip("Overhang front")]
		public float OverhangFront;

		[Min(0f)]
		[Tooltip("Overhang Back")]
		public float OverhangBack;

		[Range(0f, 10f)]
		[Tooltip("Mass")]
		public float Mass;


		// bent position
		[Min(0f)]
		[Tooltip("Deflected Target Offset (Back)")]
		public float DelfectBack= 2.0f;

		[Min(0f)]
		[Tooltip("Deflected Target Offset (Down)")]
		public float DeflectDown = 0.1f;

		[Range(-180f, 180f)]
		[Tooltip("Deflected rotation of the target. (positive gives airballs)")]
		public float DeflectRotation = 1f;


		// behaviour
		[Tooltip("If enabled, hit events by balls hitting from behind might be triggered.")]
		public bool IsLegacy;

		[Range(0, 100f)]
		[Tooltip("Minimal impact needed in order to trigger a hit event.")]
		public float Threshold = 2.0f;

		[Range(0f, 60f)]
		[Tooltip("Velocity at which the target will start to brick (with MinimalBrickPercentage)")]
		public float MinimalBrickSpeed = 30f;

		[Range(0f, 100f)]
		[Tooltip("BrickPercentage at minimal Brick Velocity)")]
		public float MinimalBrickPercentage = 20f;

		[Range(0f, 60f)]
		[Tooltip("Velocity from which the target BrickPercentage will be MaximumBrickPercentage")]
		public float MaximumBrickSpeed = 50f;

		[Range(0f, 100f)]
		[Tooltip("BrickPercentage at maximum Brick Velocity)")]
		public float MaximumBrickPercentage = 70f;




		// Physics Materials
		// Standing
		public override PhysicsMaterialData PhysicsMaterialData => GetPhysicsMaterialData(Elasticity, ElasticityFalloff, Friction, Scatter, OverwritePhysics);
		
		[Tooltip("Ignore the assigned physics material above and use the value below.")] 
		public bool OverwritePhysics = true;

		[Min(0f)]
		[Tooltip("Bounciness, also known as coefficient of restitution. Higher is more bouncy.")]
		public float Elasticity = 0.35f;

		[Min(0f)]
		[Tooltip("How much to decrease elasticity for fast impacts.")]
		public float ElasticityFalloff = 0.5f;

		[Min(0)]
		[Tooltip("Friction of the material.")]
		public float Friction = 0.2f;

		[Range(-90f, 90f)]
		[Tooltip("When hit, add a random angle between 0 and this value to the trajectory.")]
		public float Scatter = 5f;


		// Bent
		public PhysicsMaterialData BentPhysicsMaterialData => GetPhysicsMaterialData(Elasticity, ElasticityFalloff, Friction, Scatter, OverwritePhysics);

		[Tooltip("Ignore the assigned physics material above and use the value below.")]
		public bool BentOverwritePhysics = true;

		[Min(0f)]
		[Tooltip("Bounciness, also known as coefficient of restitution. Higher is more bouncy.")]
		public float BentElasticity = 0.35f;

		[Min(0f)]
		[Tooltip("How much to decrease elasticity for fast impacts.")]
		public float BentElasticityFalloff = 0.5f;

		[Min(0)]
		[Tooltip("Friction of the material.")]
		public float BentFriction = 0.2f;

		[Range(-90f, 90f)]
		[Tooltip("When hit, add a random angle between 0 and this value to the trajectory.")]
		public float BentScatter = 5f;

		// Down
		public PhysicsMaterialData DownPhysicsMaterialData => GetPhysicsMaterialData(Elasticity, ElasticityFalloff, Friction, Scatter, OverwritePhysics);

		[Tooltip("Ignore the assigned physics material above and use the value below.")]
		public bool DownOverwritePhysics = true;

		[Min(0f)]
		[Tooltip("Bounciness, also known as coefficient of restitution. Higher is more bouncy.")]
		public float DownElasticity = 0.35f;

		[Min(0f)]
		[Tooltip("How much to decrease elasticity for fast impacts.")]
		public float DownElasticityFalloff = 0.5f;

		[Min(0)]
		[Tooltip("Friction of the material.")]
		public float DownFriction = 0.2f;

		[Range(-90f, 90f)]
		[Tooltip("When hit, add a random angle between 0 and this value to the trajectory.")]
		public float DownScatter = 5f;


		// not used
		[Tooltip("If set, send \"dropped\" and \"raised\" hit events.")]
		public bool UseHitEvent = true;

		#endregion


		protected override IApiColliderGenerator InstantiateColliderApi(Player player, Entity entity)
			=> new DropTargetApi(gameObject, entity, player);
	}
}
