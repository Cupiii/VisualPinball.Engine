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

using System.Collections.Generic;
using UnityEngine;
using VisualPinball.Engine.Game.Engines;
using System.ComponentModel;
using System;

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
	 *	
	 * 
	 */
	[AddComponentMenu("Visual Pinball/Mechs/Drop Target Bank")]
	[HelpURL("https://docs.visualpinball.org/creators-guide/manual/mechanisms/drop-target-banks.html")]
	public class DropTargetBankComponent : MonoBehaviour, ICoilDeviceComponent, ISwitchDeviceComponent
	{
		public const string ResetCoilItem = "reset_coil";

		public const string SequenceCompletedSwitchItem = "sequence_completed_switch";

		[ToolboxItem("The number of the drop targets. See documentation of a description of each type.")]
		public int BankSize = 1;

		[SerializeField]
		[Tooltip("Drop Targets")]
		public DropTargetComponent[] DropTargets = Array.Empty<DropTargetComponent>();

		public IEnumerable<GamelogicEngineCoil> AvailableCoils => new[] {
			new GamelogicEngineCoil(ResetCoilItem) {
				Description = "Reset Coil"
			}
		};

		public IEnumerable<GamelogicEngineSwitch> AvailableSwitches => new[] {
			new GamelogicEngineSwitch(SequenceCompletedSwitchItem) {
				Description = "Sequence Completed Switch"
			}
		};

		IEnumerable<GamelogicEngineCoil> IDeviceComponent<GamelogicEngineCoil>.AvailableDeviceItems => AvailableCoils;
		IEnumerable<IGamelogicEngineDeviceItem> IWireableComponent.AvailableWireDestinations => AvailableCoils;
		IEnumerable<IGamelogicEngineDeviceItem> IDeviceComponent<IGamelogicEngineDeviceItem>.AvailableDeviceItems => AvailableCoils;

		public SwitchDefault SwitchDefault => SwitchDefault.NormallyOpen;
		IEnumerable<GamelogicEngineSwitch> IDeviceComponent<GamelogicEngineSwitch>.AvailableDeviceItems => AvailableSwitches;

		public static GameObject LoadPrefab() => Resources.Load<GameObject>("Prefabs/DropTargetBank");

		#region Runtime

		private void Awake()
		{
			GetComponentInParent<Player>().RegisterDropTargetBankComponent(this);
		}

		#endregion
	}
}
