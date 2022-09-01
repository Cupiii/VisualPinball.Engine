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

		public DropTargetCollider(in LineCollider lineSeg0, in LineCollider lineSeg1, ColliderInfo info) : this() {
			_header.Init(info, ColliderType.Gate);
			//LineSeg0 = lineSeg0;
			//LineSeg1 = lineSeg1;

			//Bounds = LineSeg0.Bounds;
		}

		public static void DropTargetCollide(ref BallData ball, ref NativeQueue<EventData>.ParallelWriter hitEvents,
		ref DropTargetAnimationData animationData, in float3 normal, in Entity ballEntity, in CollisionEventData collEvent,
		in Collider coll, ref Random random) {

			var dot = -math.dot(collEvent.HitNormal, ball.Velocity);
			BallCollider.Collide3DWall(ref ball, in coll.Header.Material, in collEvent, in normal, ref random);

			if (coll.FireEvents && dot >= coll.Threshold && !animationData.IsDropped) {
				animationData.HitEvent = true;
				//todo m_obj->m_currentHitThreshold = dot;
				Collider.FireHitEvent(ref ball, ref hitEvents, in ballEntity, in coll.Header);
			}
		}

		public float HitTest(ref CollisionEventData collEvent, ref DynamicBuffer<BallInsideOfBufferElement> insideOfs, in BallData ball, float dTime) {
			// todo
			// if (!this.isEnabled) {
			// 	return -1.0;
			// }
			/*
			var hitTime = LineCollider.HitTestBasic(ref collEvent, ref insideOfs, in LineSeg0, in ball, dTime, false, true, false); // any face, lateral, non-rigid
			if (hitTime >= 0) {
				// signal the Collide() function that the hit is on the front or back side
				collEvent.HitFlag = false;
				return hitTime;
			}

			hitTime = LineCollider.HitTestBasic(ref collEvent, ref insideOfs, in LineSeg1, in ball, dTime, false, true, false); // any face, lateral, non-rigid
			if (hitTime >= 0) {
				collEvent.HitFlag = true;
				return hitTime;
			}
			*/
			return -1.0f;
		}

	}
}
