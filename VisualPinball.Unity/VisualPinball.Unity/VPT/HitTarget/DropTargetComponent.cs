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
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;
using VisualPinball.Engine.VPT.HitTarget;
using VisualPinball.Engine.VPT.Table;

namespace VisualPinball.Unity
{
	/*
	 * Drop target Todo List for Cupiii (will be deleted when Pull Request is started).
	 * 
	 * Research:
	 *    What is needed - ongoing discussion: https://discord.com/channels/630126995162005544/931515470853582849
	 *    Issue: https://github.com/freezy/VisualPinball.Engine/issues/364
	 *    NFozzy / RothbauerW Physics Guide: https://github.com/freezy/VisualPinball.Engine/files/7816308/nFozzy.Physics.-.2022-01-05.pdf
	 *    
	 *    * Feature Research:
	 *		* Standard DT Properties in VP (list here) are:
	 *			* Color and formatting:
	 *				* Image
	 *				* Material
	 *				* Drop Speed
	 *				* Depth BIAS (What is this?)
	 *				* Disable Lighting (0..1)
	 *				* Disable Lightning from below (0..1)
	 *				* Visible
	 *				* Reflection enabled
	 *				* Position/Scale Vector3s
	 *				* Orientation
	 *			* Physics
	 *				* Has HitEvent
	 *				* Hit Threshold
	 *				* Material Settings Overwrite and:
	 *					* Elasticity
	 *					* Elasticity Falloff
	 *					* Friction
	 *					* Scatter Angle
	 *				* Legacy Mode (What's this?)
	 *				* Collidable (switchable in script (or via Visual scripting?))
	 *				* IsDropped
	 *			* Timer
	 *				* Timer Enabled
	 *				* Timer Interval
	 *				* User Value (What's this?)
	 *		* Additional DT Behaviour from nfozzy/rothbauerw:
	 *			* pure Animation
	 *				* Const DTDropSpeed = 110 ‘in milliseconds
	 *				* Const DTDropUpSpeed = 40 ‘in milliseconds
	 *				* Const DTDropUnits = 44 ‘VP units primitive drops so top of at or below the playfield
	 *				* Const DTDropUpUnits = 10 ‘VP units primitive raises above the up position on drops up
	 *				* Const DTMaxBend = 8 ‘max degrees primitive rotates when hit
	 *				* Const DTDropDelay = 20 ‘time in milliseconds before target drops (due to friction/impact of the ball)
	 *				* Const DTRaiseDelay = 40 ‘time in milliseconds before target drops back to normal up position after the
	 *					solenoid fires to raise the target
	 *			* Behaviour / Bricking
	 *				* Const DTBrickVel = 30 ‘velocity at which the target will brick, set to ‘0’ to disable brick
	 *				* Const DTEnableBrick = 1 ‘Set to 0 to disable bricking, 1 to enable bricking
	 *			* Physics 
	 *				* Const DTMass = 0.2 ‘Mass of the Drop Target (between 0 and 1), higher values provide more resistance
	 *			* Sound 
	 *				* Const DTHitSound = “targethit” ‘Drop Target Hit sound
	 *				* Const DTDropSound = “DTDrop” ‘Drop Target Drop sound
	 *				* Const DTResetSound = “DTReset” ‘Drop Target reset sound
	 *		* Additional behaviour according to experts:
	 *			* For flexibility two coils (Set/Reset) on the DTBank.
	 *			* Two Coils per DT (set/reset).
	 *			* A Switch "All Up"
	 *			* A Switch "All Down"
	 *			* A Switch "Down" per DT (a switch UP for DT can be obtained by VisualScripting)
	 *			* An inclination on the second wall. 
	 *				* Possibly one normal wall for normal Ball reflection
	 *				* and two inclinated walls (RothB actually uses ramps for this) and speed-thresholds, wehn they become active
	 *				* best Solution although not the fastest and possibly needs new Physics code: 
	 *					* Inclination based on Ball speed.
	 *			* A Threshold for Bricks, when ball comes in straight with too much speed (settable via threshold) 
	 *			* maybe Brick percentage for several speeds.
	 *			* A Randomness for Bricks (Ball speed is too height, only Brick in 10% of all hits.)
	 *				* Personal Research: Bricks mostly occour on bally machines, when the spring needs more tension or the whole unit is "dusty and old"
	 *				* Personal experience: I never had Bricks on my Stern 2001 Monopoly (HomeSue only) single DT (Cop-Target) I once had back in 2004,
	 *				  But i know Bricks for VERY hard shots from my Gotlieb Spirit of '76
	 *		* Thoughts from Freezys implementaion (code wise)
	 *			* Possibly another broadphase in Physics code
	 *			* Meshes / Physics Meshed should be corrected. are stragely doubled now
	 *			* possibly a rewrite / enhancement of physics code so that dynamic objects are possible
	 *		
	 *	* Code-Research:
	 *		* How are DT meshed generated, why is there some strage behaviour now?
	 *			There seems to be a difference beween 
	 *				TargetComponent.getMesh() which points to MainRenderableComponent.getDefaultMesh 
	 *					This converts the mesh that sits in the MeshFilter into a mesh and returns it 
	 *				and HitTargetMeshGenerator.getBaseMesh.
	 *					Which returns the VP "Prefabs" written in the code (which is off, when legacy colliders are "on")
	 *			Where are they called?
	 *				TargetComponent.getMesh()
	 *					Is called in DropTargetColliderGenerator.GenerateColliders(...)	
	 *				HitTargetMeshGenerator.getBaseMesh.
	 *					Is called in HittargetMeshgenerator.GetMesh(...)
	 *			STILL VERY STRANGE.
	 *		* how could under all this new thingymagingy backwards compatibility be preserved? Another Component? Added to standard Drop Targets? 
	 *			* How to Save that into the VPX?
	 *			* ANSWER (by Freezy): Should not be saved to VPX. Ignore.
	 *		* Why the Heck does the generated hitmesh cause so much trouble (hanging) with the ball...
	 *			* OK, it generates a newTime an a hit after that. but it is not siabled and tehrefor it everytime generates a enwtime
	 *			* As sthis is solved, can i use collider.header.isDisabled or isEnabled - i sure can use it, but how can i change it back an 
	 *			  forth between the different hitgenerators.
	 *		* Possibly seperate Mesh generation / Physics-Mesh Gen for every object?
	 *		* How does event system work inside VPE?
	 *		* How to toggle Physics behavoir on runtime? (on/off or better) 
	 *		* How does animation to the mesh work in VPE/Unity 
	 *		* Can the ball be affected in runtime (possibly no problem, when event system is working)
	 *		* 
	 *			
	 *	Several months have gone since the above lis was written - I got much more insight, how everything works, so letzs try to sum up:		
	 *	What File does what:
	 *	
	 *		DropTargetAnimationComponent
	 *			* Provides User Inteface for all Animation Properties, Speed, RaiseDelay, IsDropped - consider this initialisation values given by user
	 *		DropTargetAnimationData
	 *			* All internal Variables used for animation purposes. Like States "MoveDown" TimeMsec TimeStamp But also a const: DropTargetLimit... Whatever.
	 *		DropTargetAnimationSystem
	 *			* System to animate Drop Targets - Only writes to DropTarget AnimationData - Question is, if droptarget animation data is accessibly from the physics.
	 *			
	 *		DropTargetApi
	 *			* Events, 
	 *			* Switches and 
	 *			* Api for other Components (like the DTBank)
	 *			* Wiring
	 *			* (ColliderGeneration) only API call. DTColliderGenerator does the work.
	 *			* Standard Events (OnInit, OnDestroy, OnHit (via IApiHittable)
	 *		
	 *		DropTargetColliderComponent
	 *			* Povides User Interface for all Collision Properties, Elasticity, Falloff, Friction, Scatter, etc. - consider this initialisation values given by user
	 *		DropTargetColliderGenerator
	 *			* Generates all Colliders for the DT - There was a "mistake", that the Mesh was used to generate the colliders - 
	 *				that could be very complicated, but also good, for DT, that have special roll-over characteristics 
	 *				Don't know what to do here - simple self generated mesh, or use "animation" dt - possibly overkill - 
	 *				will use self generated one with overhang front/back/overhang hight
	 *		
	 *		DropTargetComponent
	 *			* Standard Component (extendsTargetComponent by inheritance) - some things I dont't understand yet. 
	 *		DropTargetStaticData
	 *			* used in the AnimationSystem - gets copied over from properties in (mostly animation-)component in DTComponent.convert(...)
	 *		DropTargetTransformationSystem	
	 *			* my best guess is that this is the job, that sets the position of the visible mesh... - yes, tests confirm - i could use this - 
	 *				i have to use and enhance this when i enhance DropTargetAnimationData to Rotation etc.
	 *		
	 *		HitTarget*
	 *			* not looked into
	 *		
	 *		* TargetCollider
	 *			* very interesting - the DT has no HitTest - so it is assumed to be static and only standard Colliders (Line3DCollider, Trinagle Collider, Point Collider).
	 *			* Do we need a hitTest? The Problem is, that I loose contact to the collider after generation - can I possibly save them?
	 *			* I have a Colliderheader but cannot get it from within the droptarget
	 *			* also the only thing that couldbe used is "onEnabled"
	 *			
	 *			
	 *			Hier bleibt auch die Frage, wann wird collide ausgelöst? nur wenn hittest einen kleinen Wert wiedergibt?
	 * 
	 */
	[AddComponentMenu("Visual Pinball/Game Item/Drop Target")]
	public class DropTargetComponent : TargetComponent, IConvertGameObjectToEntity
	{
		public override bool IsLegacy {
			get {
				var colliderComponent = GetComponent<DropTargetColliderComponent>();
				return colliderComponent && colliderComponent.IsLegacy;
			}
		}

		protected override float ZOffset {
			get {
				var animationComponent = GetComponentInChildren<DropTargetAnimationComponent>();
				return animationComponent && animationComponent.IsDropped ? -DropTargetAnimationData.DropTargetLimit : 0f;
			}
		}

		#region Conversion

		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			Convert(entity, dstManager);

			var colliderComponent = GetComponent<DropTargetColliderComponent>();
			var animationComponent = GetComponentInChildren<DropTargetAnimationComponent>();
			if (colliderComponent && animationComponent) {

				dstManager.AddComponentData(entity, new DropTargetStaticData {
					Speed = animationComponent.Speed,
					RaiseDelay = animationComponent.RaiseDelay,
					UseHitEvent = colliderComponent.UseHitEvent,
				});

				dstManager.AddComponentData(entity, new DropTargetAnimationData {
					IsDropped = animationComponent.IsDropped,
					MoveDown = !animationComponent.IsDropped,
					ZOffset = animationComponent.IsDropped ? -DropTargetAnimationData.DropTargetLimit : 0f
				});
			}

			// register
			transform.GetComponentInParent<Player>().RegisterDropTarget(this, entity);
		}

		public override IEnumerable<MonoBehaviour> SetData(HitTargetData data)
		{
			var updatedComponents = base.SetData(data).ToList();

			// drop target data
			var colliderComponent = GetComponent<DropTargetColliderComponent>();
			if (colliderComponent) {
				colliderComponent.enabled = data.IsCollidable;
				colliderComponent.UseHitEvent = data.UseHitEvent;
				colliderComponent.Threshold = data.Threshold;
				colliderComponent.IsLegacy = data.IsLegacy;

				colliderComponent.OverwritePhysics = data.OverwritePhysics;
				colliderComponent.Elasticity = data.Elasticity;
				colliderComponent.ElasticityFalloff = data.ElasticityFalloff;
				colliderComponent.Friction = data.Friction;
				colliderComponent.Scatter = data.Scatter;

				updatedComponents.Add(colliderComponent);
			}

			// animation data
			var animationComponent = GetComponent<DropTargetAnimationComponent>();
			if (animationComponent) {
				animationComponent.enabled = data.IsDropTarget;
				animationComponent.Speed = data.DropSpeed;
				animationComponent.RaiseDelay = data.RaiseDelay;
				animationComponent.IsDropped = data.IsDropped;

				updatedComponents.Add(animationComponent);
			}

			return updatedComponents;
		}

		public override IEnumerable<MonoBehaviour> SetReferencedData(HitTargetData data, Table table, IMaterialProvider materialProvider, ITextureProvider textureProvider, Dictionary<string, IMainComponent> components)
		{
			var colliderComponent = GetComponent<DropTargetColliderComponent>();
			if (colliderComponent) {
				colliderComponent.PhysicsMaterial = materialProvider.GetPhysicsMaterial(data.PhysicsMaterial);
			}

			// visibility
			SetEnabled<Renderer>(data.IsVisible);

			return Array.Empty<MonoBehaviour>();
		}

		public override HitTargetData CopyDataTo(HitTargetData data, string[] materialNames, string[] textureNames, bool forExport)
		{
			base.CopyDataTo(data, materialNames, textureNames, forExport);

			// collision data
			var colliderComponent = GetComponent<DropTargetColliderComponent>();
			if (colliderComponent) {
				data.IsCollidable = colliderComponent.enabled;
				data.Threshold = colliderComponent.Threshold;
				data.UseHitEvent = colliderComponent.UseHitEvent;
				data.PhysicsMaterial = colliderComponent.PhysicsMaterial == null ? string.Empty : colliderComponent.PhysicsMaterial.name;
				data.IsLegacy = colliderComponent.IsLegacy;

				data.OverwritePhysics = colliderComponent.OverwritePhysics;
				data.Elasticity = colliderComponent.Elasticity;
				data.ElasticityFalloff = colliderComponent.ElasticityFalloff;
				data.Friction = colliderComponent.Friction;
				data.Scatter = colliderComponent.Scatter;

			} else {
				data.IsCollidable = false;
			}

			// animation data
			var dropTargetAnimationComponent = GetComponent<DropTargetAnimationComponent>();
			if (dropTargetAnimationComponent) {
				data.DropSpeed = dropTargetAnimationComponent.Speed;
				data.RaiseDelay = dropTargetAnimationComponent.RaiseDelay;
				data.IsDropped = dropTargetAnimationComponent.IsDropped;
			}

			return data;
		}

		#endregion
	}
}
