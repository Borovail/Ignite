using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

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
                var direction = math.normalize(mover.ValueRO.TargetPosition - transform.ValueRO.Position);
                direction.y = 0;

                transform.ValueRW.Rotation = math.slerp(transform.ValueRO.Rotation,
                    quaternion.LookRotation(direction, math.up()),
                    mover.ValueRO.RotationSpeed * SystemAPI.Time.DeltaTime);

                if (!mover.ValueRO.TargetPosition.Equals(float3.zero))
                    velocity.ValueRW.Linear = direction * mover.ValueRO.MaxSpeed;
                velocity.ValueRW.Angular = float3.zero;
            }
        }
    }
}