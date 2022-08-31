using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;


namespace VisualPinball.Unity {
	internal struct DropTargetCollider : ICollider {

		public int Id => _header.Id;

		private ColliderHeader _header;

		public ColliderBounds Bounds { get; private set; }

		public unsafe void Allocate(BlobBuilder builder, ref BlobBuilderArray<BlobPtr<Collider>> colliders, int colliderId) {
			_header.Id = colliderId;
			var bounds = Bounds;
			bounds.ColliderId = colliderId;
			Bounds = bounds;
			ref var ptr = ref UnsafeUtility.As<BlobPtr<Collider>, BlobPtr<GateCollider>>(ref colliders[_header.Id]);
			ref var collider = ref builder.Allocate(ref ptr);
			UnsafeUtility.MemCpy(
				UnsafeUtility.AddressOf(ref collider),
				UnsafeUtility.AddressOf(ref this),
				sizeof(GateCollider)
			);
		}

	}
}
