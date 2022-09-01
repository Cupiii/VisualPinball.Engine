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

		private bool _foldoutMaterial = true;

		private SerializedProperty _thresholdProperty;
		private SerializedProperty _physicsMaterialProperty;
		private SerializedProperty _overwritePhysicsProperty;
		private SerializedProperty _elasticityProperty;
		private SerializedProperty _elasticityFalloffProperty;
		private SerializedProperty _frictionProperty;
		private SerializedProperty _scatterProperty;

		private SerializedProperty _isLegacyProperty;

		protected override void OnEnable()
		{
			base.OnEnable();
			_isLegacyProperty = serializedObject.FindProperty(nameof(DropTargetColliderComponent.IsLegacy));

			_thresholdProperty = serializedObject.FindProperty(nameof(HitTargetColliderComponent.Threshold));
			_physicsMaterialProperty = serializedObject.FindProperty(nameof(ColliderComponent<HitTargetData, TargetComponent>.PhysicsMaterial));
			_overwritePhysicsProperty = serializedObject.FindProperty(nameof(HitTargetColliderComponent.OverwritePhysics));
			_elasticityProperty = serializedObject.FindProperty(nameof(HitTargetColliderComponent.Elasticity));
			_elasticityFalloffProperty = serializedObject.FindProperty(nameof(HitTargetColliderComponent.ElasticityFalloff));
			_frictionProperty = serializedObject.FindProperty(nameof(HitTargetColliderComponent.Friction));
			_scatterProperty = serializedObject.FindProperty(nameof(HitTargetColliderComponent.Scatter));
		}
		protected override void OnTargetInspectorGUI() {

			PropertyField(_thresholdProperty, "Hit Threshold");

			PropertyField(_isLegacyProperty, "Legacy Collider", updateColliders: true);

			// physics material
			if (_foldoutMaterial = EditorGUILayout.BeginFoldoutHeaderGroup(_foldoutMaterial, "Physics Material")) {
				EditorGUI.BeginDisabledGroup(_overwritePhysicsProperty.boolValue);
				PropertyField(_physicsMaterialProperty, "Preset");
				EditorGUI.EndDisabledGroup();

				PropertyField(_overwritePhysicsProperty);

				EditorGUI.BeginDisabledGroup(!_overwritePhysicsProperty.boolValue);
				PropertyField(_elasticityProperty);
				PropertyField(_elasticityFalloffProperty);
				PropertyField(_frictionProperty);
				PropertyField(_scatterProperty, "Scatter Angle");
				EditorGUI.EndDisabledGroup();
			}
			EditorGUILayout.EndFoldoutHeaderGroup();


		}
	}
}
