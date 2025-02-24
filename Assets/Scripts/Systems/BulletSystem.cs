using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))]
    public partial struct BulletSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            var collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;

            foreach (var (transform, bullet, velocity, entity)
                     in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Bullet>, RefRO<PhysicsVelocity>>().WithEntityAccess())
            {
                if (float.IsNaN(transform.ValueRO.Position.y)) continue;

                var collisionFilter = new CollisionFilter
                {
                    BelongsTo = ~0u,
                    CollidesWith = (uint)bullet.ValueRO.TriggerToExplodeLayer.value,
                    GroupIndex = 0
                };

                var start = transform.ValueRO.Position;
                var distance = math.length(velocity.ValueRO.Linear) * SystemAPI.Time.DeltaTime;

                if (!collisionWorld.SphereCast(start, bullet.ValueRO.Radius, math.forward(transform.ValueRO.Rotation), distance, out _,
                        collisionFilter)) continue;

                var hits = new NativeList<DistanceHit>(Allocator.Temp);

                collisionFilter.CollidesWith = (uint)bullet.ValueRO.HitLayer.value;
                if (!collisionWorld.OverlapSphere(transform.ValueRO.Position, bullet.ValueRO.SplashRadius, ref hits,
                        collisionFilter))
                    continue;

                foreach (var hit in hits)
                {
                    if (!SystemAPI.HasComponent<Mover>(hit.Entity) || !SystemAPI.IsComponentEnabled<Mover>(hit.Entity))
                        continue;

                    if (!SystemAPI.HasComponent<Hit>(hit.Entity)) continue;
                    SystemAPI.SetComponentEnabled<Hit>(hit.Entity, true);
                    SystemAPI.SetComponent(hit.Entity, new Hit
                    {
                        Position = hit.Position,
                        PushForce = bullet.ValueRO.PushForce,
                        Damage = bullet.ValueRO.Damage
                    });
                }

                hits.Dispose();

                ecb.DestroyEntity(entity);
            }
        }


    }
}