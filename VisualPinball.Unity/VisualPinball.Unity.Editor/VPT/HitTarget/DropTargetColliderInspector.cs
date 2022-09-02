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

using UnityEditor;
using VisualPinball.Engine.VPT.HitTarget;

namespace VisualPinball.Unity.Editor
{
	[CustomEditor(typeof(DropTargetColliderComponent)), CanEditMultipleObjects]
	public class DropTargetColliderInspector : TargetColliderInspector<DropTargetColliderComponent>
	{

		private bool _foldoutPhysicalDimensions = true;
		private bool _foldoutBentPosition = true;
		private bool _foldoutBehaviour = true;
		private bool _foldoutPhysicsMaterialsStanding = true;
		private bool _foldoutPhysicsMaterialsBent = true;
		private bool _foldoutPhysicsMaterialsDown = true;

		private bool _foldoutMaterial = true;






		private SerializedProperty _dimensionsProperty;
		private SerializedProperty _overhangFrontProperty;
		private SerializedProperty _overhangBackProperty;
		private SerializedProperty _massProperty;

		private SerializedProperty _deflectBackProperty;
		private SerializedProperty _deflectDownProperty;
		private SerializedProperty _deflectRotationProperty;

		private SerializedProperty _isLegacyProperty;
		private SerializedProperty _thresholdProperty;
		private SerializedProperty _minimalBrickSpeedProperty;
		private SerializedProperty _minimalBrickPercentageProperty;
		private SerializedProperty _maximumBrickSpeedProperty;
		private SerializedProperty _maximumBrickPercentageProperty;

		private SerializedProperty _physicsMaterialProperty;
		private SerializedProperty _overwritePhysicsProperty;
		private SerializedProperty _elasticityProperty;
		private SerializedProperty _elasticityFalloffProperty;
		private SerializedProperty _frictionProperty;
		private SerializedProperty _scatterProperty;

		private SerializedProperty _bentPhysicsMaterialProperty;
		private SerializedProperty _bentOverwritePhysicsProperty;
		private SerializedProperty _bentElasticityProperty;
		private SerializedProperty _bentElasticityFalloffProperty;
		private SerializedProperty _bentFrictionProperty;
		private SerializedProperty _bentScatterProperty;

		private SerializedProperty _sidePhysicsMaterialProperty;
		private SerializedProperty _sideOverwritePhysicsProperty;
		private SerializedProperty _sideElasticityProperty;
		private SerializedProperty _sideElasticityFalloffProperty;
		private SerializedProperty _sideFrictionProperty;
		private SerializedProperty _sideScatterProperty;


		protected override void OnEnable()
		{
			base.OnEnable();
			_dimensionsProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.Dimensions));

			_deflectBackProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.DeflectBack));
			_deflectDownProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.DeflectDown));
			_deflectRotationProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.DeflectRotation));

			_isLegacyProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.IsLegacy));
			_thresholdProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.Threshold));
			_massProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.Mass));
			_minimalBrickSpeedProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.MinimalBrickSpeed));
			_minimalBrickPercentageProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.MinimalBrickPercentage));
			_maximumBrickSpeedProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.MaximumBrickSpeed));
			_maximumBrickPercentageProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.MaximumBrickPercentage));

			_physicsMaterialProperty = serializedObject.FindProperty(nameof(ColliderComponent<HitTargetData, TargetComponent>.PhysicsMaterial));
			_overwritePhysicsProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.OverwritePhysics));
			_elasticityProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.Elasticity));
			_elasticityFalloffProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.ElasticityFalloff));
			_frictionProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.Friction));
			_scatterProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.Scatter));

			_bentPhysicsMaterialProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.BentPhysicsMaterial));
			_bentOverwritePhysicsProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.BentOverwritePhysics));
			_bentElasticityProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.BentElasticity));
			_bentElasticityFalloffProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.BentElasticityFalloff));
			_bentFrictionProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.BentFriction));
			_bentScatterProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.BentScatter));

			_sidePhysicsMaterialProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.SidePhysicsMaterial));
			_sideOverwritePhysicsProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.SideOverwritePhysics));
			_sideElasticityProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.SideElasticity));
			_sideElasticityFalloffProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.SideElasticityFalloff));
			_sideFrictionProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.SideFriction));
			_sideScatterProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.SideScatter));

		}
		protected override void OnTargetInspectorGUI() {

			// Hier muss jetzt alles umgestellt werden.
			// Dann muss der AnimationInspector noch ein paar daten bekommen und auch umgestellt werden

			// Physical Dimensions
			if (_foldoutPhysicalDimensions = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutPhysicalDimensions, "Physical Dimensions")) {
				EditorGUI.indentLevel = 1;
				PropertyField(_dimensionsProperty, "Dimensions", updateColliders: true);
				EditorGUI.indentLevel = 0;
			}
			// Bent Position
			EditorGUILayout.EndFoldoutHeaderGroup();
			if (_foldoutBentPosition = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutBentPosition, "Bent Position")) {
				EditorGUI.indentLevel = 1;
				PropertyField(_deflectBackProperty, "Deflect Back", updateColliders: true);
				PropertyField(_deflectDownProperty, "Deflect Down", updateColliders: true);
				PropertyField(_deflectRotationProperty, "Deflect Rotation", updateColliders: true);
				EditorGUI.indentLevel = 0;
			}
			// Behaviour
			EditorGUILayout.EndFoldoutHeaderGroup();
			if (_foldoutBehaviour = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutBehaviour, "Behaviour")) {
				EditorGUI.indentLevel = 1;
				PropertyField(_isLegacyProperty, "Legacy Collider");
				PropertyField(_thresholdProperty, "Hit Threshold");
				PropertyField(_massProperty, "Mass");
				PropertyField(_minimalBrickSpeedProperty, "Min Brick Speed");
				PropertyField(_minimalBrickPercentageProperty, "Min Brick Probability");
				PropertyField(_maximumBrickSpeedProperty, "Max Brick Speed");
				PropertyField(_maximumBrickPercentageProperty, "Max Brick Probability");
				EditorGUI.indentLevel = 0;
			}
			EditorGUILayout.EndFoldoutHeaderGroup();
			// Physics Material Standing
			if (_foldoutPhysicsMaterialsStanding = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutPhysicsMaterialsStanding, "Physics Material Standing Front")) {
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginDisabledGroup(_overwritePhysicsProperty.boolValue);
				PropertyField(_physicsMaterialProperty, "Preset");
				EditorGUI.EndDisabledGroup();
				PropertyField(_overwritePhysicsProperty,"Overwrite Physics");
				EditorGUI.BeginDisabledGroup(!_overwritePhysicsProperty.boolValue);
				PropertyField(_elasticityProperty, "Elsticity");
				PropertyField(_elasticityFalloffProperty, "Elasticity Falloff");
				PropertyField(_frictionProperty, "Friction");
				PropertyField(_scatterProperty, "Scatter Angle");
				EditorGUI.EndDisabledGroup();
				EditorGUI.indentLevel = 0;
			}

			EditorGUILayout.EndFoldoutHeaderGroup();
			if (_foldoutPhysicsMaterialsBent = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutPhysicsMaterialsBent, "Physics Material Bent Front")) {
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginDisabledGroup(_bentOverwritePhysicsProperty.boolValue);
				PropertyField(_bentPhysicsMaterialProperty, "Preset");
				EditorGUI.EndDisabledGroup();
				PropertyField(_bentOverwritePhysicsProperty, "Overwrite Physics");
				EditorGUI.BeginDisabledGroup(!_bentOverwritePhysicsProperty.boolValue);
				PropertyField(_bentElasticityProperty, "Elsticity");
				PropertyField(_bentElasticityFalloffProperty, "Elasticity Falloff");
				PropertyField(_bentFrictionProperty, "Friction");
				PropertyField(_bentScatterProperty, "Scatter Angle");
				EditorGUI.EndDisabledGroup();
				EditorGUI.indentLevel = 0;
			}
			EditorGUILayout.EndFoldoutHeaderGroup();
			if (_foldoutPhysicsMaterialsDown = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutPhysicsMaterialsDown, "Physics Material Sides and Back")) {
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginDisabledGroup(_sideOverwritePhysicsProperty.boolValue);
				PropertyField(_sidePhysicsMaterialProperty, "Preset");
				EditorGUI.EndDisabledGroup();
				PropertyField(_sideOverwritePhysicsProperty, "Overwrite Physics");
				EditorGUI.BeginDisabledGroup(!_sideOverwritePhysicsProperty.boolValue);
				PropertyField(_sideElasticityProperty, "Elsticity");
				PropertyField(_sideElasticityFalloffProperty, "Elasticity Falloff");
				PropertyField(_sideFrictionProperty, "Friction");
				PropertyField(_sideScatterProperty, "Scatter Angle");
				EditorGUI.EndDisabledGroup();
				EditorGUI.indentLevel = 0;
			}
			EditorGUILayout.EndFoldoutHeaderGroup();

		



		}
	}
}
