using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireMatchingQueriesForUpdate]
    public partial struct MoverSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (transform, mover, velocity, mass)
                     in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Mover>, RefRW<PhysicsVelocity>, RefRO<PhysicsMass>>())
            {
                var direction = mover.ValueRO.TargetPosition.Equals(float3.zero) ? float3.zero : math.normalize(mover.ValueRO.TargetPosition - transform.ValueRO.Position);

                var desiredVelocity = direction * mover.ValueRO.MaxSpeed;
                var velocityDiff = desiredVelocity - velocity.ValueRO.Linear;
                var accelarationForce = velocityDiff * mover.ValueRO.Acceleration;

                var linear = (accelarationForce * mass.ValueRO.InverseMass) * SystemAPI.Time.DeltaTime;
                linear.y = 0;
                velocity.ValueRW.Linear += linear;

                // Rotation
                velocity.ValueRW.Angular = float3.zero;
                transform.ValueRW.Rotation = math.slerp(transform.ValueRO.Rotation,
                    !direction.Equals(float3.zero) ? quaternion.LookRotation(direction, math.up()) : quaternion.LookRotation(math.forward(), math.up()),
                    mover.ValueRO.RotationSpeed * SystemAPI.Time.DeltaTime);
            }

            //foreach (var (transform, mover, velocity)
            //         in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Mover>, RefRW<PhysicsVelocity>>().WithAll<Bullet>())
            //{
            //    var direction = math.normalize(mover.ValueRO.TargetPosition - transform.ValueRO.Position);

            //    if (!mover.ValueRO.TargetPosition.Equals(float3.zero))
            //        velocity.ValueRW.Linear = direction * mover.ValueRO.MaxSpeed;
            //    velocity.ValueRW.Angular = float3.zero;
            //}
        }
    }
}