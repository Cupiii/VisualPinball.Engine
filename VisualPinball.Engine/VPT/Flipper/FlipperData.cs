// Visual Pinball Engine
// Copyright (C) 2020 freezy and VPE Team
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

#region ReSharper
// ReSharper disable UnassignedField.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ConvertToConstant.Global
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using MessagePack;
using VisualPinball.Engine.Common;
using VisualPinball.Engine.IO;
using VisualPinball.Engine.Math;
using VisualPinball.Engine.VPT.Table;

namespace VisualPinball.Engine.VPT.Flipper
{
	[Serializable]
	[BiffIgnore("RWDT")]
	[BiffIgnore("RHGT")]
	[BiffIgnore("RTHK")]
	[MessagePackObject]
	public class FlipperData : ItemData
	{
		public override string GetName() => Name;
		public override void SetName(string name) { Name = name; }

		[Key(14)]
		[BiffString("NAME", IsWideString = true, Pos = 14)]
		public string Name;

		[Key(2)]
		[BiffFloat("BASR", Pos = 2)]
		public float BaseRadius = 21.5f;

		[Key(3)]
		[BiffFloat("ENDR", Pos = 3)]
		public float EndRadius = 13.0f;

		[Key(29)]
		[BiffFloat("FRMN", Pos = 29)]
		public float FlipperRadiusMin;

		[Key(4)]
		[BiffFloat("FLPR", Pos = 4)]
		public float FlipperRadiusMax = 130.0f;

		[Key(50)]
		[BiffFloat("FLPR", SkipWrite = true)]
		public float FlipperRadius = 130.0f;

		[Key(6)]
		[BiffFloat("ANGS", Pos = 6)]
		public float StartAngle = 121.0f;

		[Key(7)]
		[BiffFloat("ANGE", Pos = 7)]
		public float EndAngle = 70.0f;

		[Key(30)]
		[BiffFloat("FHGT", Pos = 30)]
		public float Height = 50.0f;

		[Key(1)]
		[BiffVertex("VCEN", Pos = 1)]
		public Vertex2D Center;

		[Key(31)]
		[TextureReference]
		[BiffString("IMAG", Pos = 31)]
		public string Image = string.Empty;

		[Key(12)]
		[BiffString("SURF", Pos = 12)]
		public string Surface = string.Empty;

		[Key(13)]
		[MaterialReference]
		[BiffString("MATR", Pos = 13)]
		public string Material = string.Empty;

		[Key(15)]
		[MaterialReference]
		[BiffString("RUMA", Pos = 15)]
		public string RubberMaterial = string.Empty;

		[Key(161)]
		[BiffFloat("RTHF", Pos = 16.1)]
		public float RubberThickness = 7.0f;

		[Key(171)]
		[BiffFloat("RHGF", Pos = 17.1)]
		public float RubberHeight = 19.0f;

		[Key(181)]
		[BiffFloat("RWDF", Pos = 18.1)]
		public float RubberWidth = 24.0f;

		[Key(9)]
		[BiffFloat("FORC", Pos = 9)]
		public float Mass = 1f;

		[Key(19)]
		[BiffFloat("STRG", Pos = 19)]
		public float Strength = 2200f;

		[Key(20)]
		[BiffFloat("ELAS", Pos = 20)]
		public float Elasticity = 0.8f;

		[Key(21)]
		[BiffFloat("ELFO", Pos = 21)]
		public float ElasticityFalloff = 0.43f;

		[Key(22)]
		[BiffFloat("FRIC", Pos = 22)]
		public float Friction = 0.6f;

		[Key(5)]
		[BiffFloat("FRTN", Pos = 5)]
		public float Return = 0.058f;

		[Key(23)]
		[BiffFloat("RPUP", Pos = 23)]
		public float RampUp = 3f;

		[Key(25)]
		[BiffFloat("TODA", Pos = 25)]
		public float TorqueDamping = 0.75f;

		[Key(26)]
		[BiffFloat("TDAA", Pos = 26)]
		public float TorqueDampingAngle = 6f;

		[Key(24)]
		[BiffFloat("SCTR", Pos = 24)]
		public float Scatter;

		[Key(8)]
		[BiffInt("OVRP", Pos = 8)]
		public int OverridePhysics;

		[Key(27)]
		[BiffBool("VSBL", Pos = 27)]
		public bool IsVisible = true;

		[Key(28)]
		[BiffBool("ENBL", Pos = 28)]
		public bool IsEnabled = true;

		[Key(32)]
		[BiffBool("REEN", Pos = 32)]
		public bool IsReflectionEnabled = true;

		[Key(10)]
		[BiffBool("TMON", Pos = 10)]
		public bool IsTimerEnabled;

		[Key(11)]
		[BiffInt("TMIN", Pos = 11)]
		public int TimerInterval;

		[IgnoreMember] public float OverrideMass;
		[IgnoreMember] public float OverrideStrength;
		[IgnoreMember] public float OverrideElasticity;
		[IgnoreMember] public float OverrideElasticityFalloff;
		[IgnoreMember] public float OverrideFriction;
		[IgnoreMember] public float OverrideReturnStrength;
		[IgnoreMember] public float OverrideCoilRampUp;
		[IgnoreMember] public float OverrideTorqueDamping;
		[IgnoreMember] public float OverrideTorqueDampingAngle;
		[IgnoreMember] public float OverrideScatterAngle;

		public float GetReturnRatio(TableData tableData) => DoOverridePhysics(tableData) ? OverrideReturnStrength : Return;
		public float GetStrength(TableData tableData) => DoOverridePhysics(tableData) ? OverrideStrength : Strength;
		public float GetTorqueDampingAngle(TableData tableData) => DoOverridePhysics(tableData) ? OverrideTorqueDampingAngle : TorqueDampingAngle;
		public float GetFlipperMass(TableData tableData) => DoOverridePhysics(tableData) ? OverrideMass : Mass;
		public float GetTorqueDamping(TableData tableData) => DoOverridePhysics(tableData) ? OverrideTorqueDamping : TorqueDamping;
		public float GetRampUpSpeed(TableData tableData) => DoOverridePhysics(tableData) ? OverrideCoilRampUp : RampUp;

		public void UpdatePhysicsSettings(Table.Table table)
		{
			if (DoOverridePhysics(table.Data)) {
				var registry = Registry.Instance;

				var idx = OverridePhysics != 0 ? OverridePhysics - 1 : table.Data.OverridePhysics - 1;

				OverrideMass = registry.Get<float>("Player", $"FlipperPhysicsMass${idx}", 1);
				if (OverrideMass < 0.0) {
					OverrideMass = Mass;
				}

				OverrideStrength = registry.Get<float>("Player", $"FlipperPhysicsStrength${idx}", 2200);
				if (OverrideStrength < 0.0) {
					OverrideStrength = Strength;
				}

				OverrideElasticity = registry.Get<float>("Player", $"FlipperPhysicsElasticity${idx}", 0.8f);
				if (OverrideElasticity < 0.0) {
					OverrideElasticity = Elasticity;
				}

				OverrideScatterAngle = registry.Get<float>("Player", $"FlipperPhysicsScatter${idx}", 0);
				if (OverrideScatterAngle < 0.0) {
					OverrideScatterAngle = Scatter;
				}

				OverrideReturnStrength = registry.Get<float>("Player", $"FlipperPhysicsReturnStrength${idx}", 0.058f);
				if (OverrideReturnStrength < 0.0) {
					OverrideReturnStrength = Return;
				}

				OverrideElasticityFalloff =
					registry.Get<float>("Player", $"FlipperPhysicsElasticityFalloff${idx}", 0.43f);
				if (OverrideElasticityFalloff < 0.0) {
					OverrideElasticityFalloff = ElasticityFalloff;
				}

				OverrideFriction = registry.Get<float>("Player", $"FlipperPhysicsFriction${idx}", 0.6f);
				if (OverrideFriction < 0.0) {
					OverrideFriction = Friction;
				}

				OverrideCoilRampUp = registry.Get<float>("Player", $"FlipperPhysicsCoilRampUp${idx}", 3.0f);
				if (OverrideCoilRampUp < 0.0) {
					OverrideCoilRampUp = RampUp;
				}

				OverrideTorqueDamping = registry.Get<float>("Player", $"FlipperPhysicsEOSTorque${idx}", 0.75f);
				if (OverrideTorqueDamping < 0.0) {
					OverrideTorqueDamping = TorqueDamping;
				}

				OverrideTorqueDampingAngle = registry.Get<float>("Player", $"FlipperPhysicsEOSTorqueAngle${idx}", 6.0f);
				if (OverrideTorqueDampingAngle < 0.0) {
					OverrideTorqueDampingAngle = TorqueDampingAngle;
				}
			}
		}

		public FlipperData(string name, float x, float y) : base(StoragePrefix.GameItem)
		{
			Name = name;
			Center = new Vertex2D(x, y);
		}

		#region BIFF

		static FlipperData()
		{
			Init(typeof(FlipperData), Attributes);
		}

		[SerializationConstructor]
		public FlipperData() : base(StoragePrefix.GameItem)
		{
		}

		public FlipperData(string storageName) : base(storageName)
		{
		}

		public FlipperData(BinaryReader reader, string storageName) : this(storageName)
		{
			Load(this, reader, Attributes);
		}

		public override void Write(BinaryWriter writer, HashWriter hashWriter)
		{
			writer.Write((int)ItemType.Flipper);
			WriteRecord(writer, Attributes, hashWriter);
			WriteEnd(writer, hashWriter);
		}

		private static readonly Dictionary<string, List<BiffAttribute>> Attributes = new Dictionary<string, List<BiffAttribute>>();

		#endregion

		public bool DoOverridePhysics(TableData tableData) => OverridePhysics != 0 || tableData.OverridePhysicsFlipper && tableData.OverridePhysics != 0;
	}
}
