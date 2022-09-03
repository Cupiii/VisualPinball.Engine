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

// ReSharper disable AssignmentInConditionalExpression

using UnityEditor;
using VisualPinball.Engine.VPT.HitTarget;

namespace VisualPinball.Unity.Editor
{
	[CustomEditor(typeof(DropTargetAnimationComponent)), CanEditMultipleObjects]
	public class DropTargetAnimationInspector : AnimationInspector<HitTargetData, DropTargetComponent, DropTargetAnimationComponent>
	{
		private SerializedProperty _dropSpeedProperty;
		private SerializedProperty _raiseSpeedProperty;
		private SerializedProperty _dropDelayProperty;
		private SerializedProperty _raiseDelayProperty;
		private SerializedProperty _overshootProperty;
		private SerializedProperty _overshootDropDelayProperty;
		private SerializedProperty _isDroppedProperty;

		protected override void OnEnable()
		{
			base.OnEnable();

			_dropSpeedProperty = serializedObject.FindProperty(nameof(DropTargetAnimationComponent.DropSpeed));
			_raiseSpeedProperty = serializedObject.FindProperty(nameof(DropTargetAnimationComponent.RaiseSpeed));
			_dropDelayProperty = serializedObject.FindProperty(nameof(DropTargetAnimationComponent.DropDelay));
			_raiseDelayProperty = serializedObject.FindProperty(nameof(DropTargetAnimationComponent.RaiseDelay));
			_overshootProperty = serializedObject.FindProperty(nameof(DropTargetAnimationComponent.Overshoot));
			_overshootDropDelayProperty = serializedObject.FindProperty(nameof(DropTargetAnimationComponent.OvershootDropDelay));
			_isDroppedProperty = serializedObject.FindProperty(nameof(DropTargetAnimationComponent.IsDropped));
		}

		public override void OnInspectorGUI()
		{
			if (HasErrors()) {
				return;
			}

			BeginEditing();

			OnPreInspectorGUI();

			PropertyField(_dropSpeedProperty);
			PropertyField(_raiseSpeedProperty);
			PropertyField(_dropDelayProperty);
			PropertyField(_raiseDelayProperty);
			PropertyField(_overshootProperty);
			PropertyField(_overshootDropDelayProperty);
			PropertyField(_isDroppedProperty, updateTransforms: true);

			base.OnInspectorGUI();

			EndEditing();
		}
	}
}
