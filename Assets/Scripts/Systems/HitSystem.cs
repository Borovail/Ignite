using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts
{
    [UpdateAfter(typeof(BulletSystem))]
    public partial struct HitSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (transform, velocity, collision, health, entity)
                     in SystemAPI.Query<RefRO<LocalTransform>, RefRW<PhysicsVelocity>,
                         RefRO<Hit>, RefRW<Health>>().WithEntityAccess())
            {
                SystemAPI.SetComponentEnabled<Mover>(entity, false);
                SystemAPI.SetComponentEnabled<Hit>(entity, false);

                var direction = math.normalize(transform.ValueRO.Position - collision.ValueRO.Position);

                var distance = math.length(transform.ValueRO.Position - collision.ValueRO.Position);
                var verticalMultiplier = 0.3f;
                var horizontalMultiplier = 0.5f;

                var impulse = direction * collision.ValueRO.PushForce * horizontalMultiplier;

                impulse.y += math.pow(1f / (distance + 0.5f), 1.5f) * collision.ValueRO.PushForce * verticalMultiplier;

                velocity.ValueRW.Linear += impulse;

                health.ValueRW.Value -= collision.ValueRO.Damage;

                if (health.ValueRO.Value <= 0)
                    SystemAPI.SetComponentEnabled<LifeTime>(entity, true);
            }

            foreach (var (mover, entity) in SystemAPI.Query<RefRW<Mover>>().WithDisabled<Mover, LifeTime>().WithEntityAccess())
            {
                mover.ValueRW.HitStunTimer -= SystemAPI.Time.DeltaTime;

                if (!(mover.ValueRO.HitStunTimer <= 0)) continue;

                mover.ValueRW.HitStunTimer = mover.ValueRO.HitStunDuration;
                SystemAPI.SetComponentEnabled<Mover>(entity, true);
            }
        }
    }
}