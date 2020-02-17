// ReSharper disable UnassignedField.Global

using System;
using VisualPinball.Engine.IO;

namespace VisualPinball.Engine.VPT
{
	/// <summary>
	/// Data classes reflects what's in the VPX file.<p/>
	///
	/// Every playfield item has its own data class. They can currently
	/// only read data.
	/// </summary>
	[Serializable]
	public abstract class ItemData : BiffData
	{
		[BiffBool("LOCK", Pos = 1000)]
		public bool IsLocked;

		[BiffInt("LAYR", Pos = 1001)]
		public int EditorLayer;

		public abstract string GetName();

		protected ItemData(string storageName) : base(storageName) { }
	}

	public interface IPhysicalData {
		float Elasticity { get; set; }
		float ElasticityFalloff { get; set; }
		float Friction { get; set; }
		float Scatter { get; set; }
		bool OverwritePhysics { get; set; }
		bool IsCollidable { get; set; }
		string PhysicsMaterial { get; set; }
	}
}
