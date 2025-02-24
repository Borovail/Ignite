using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts
{
    [UpdateAfter(typeof(TowerFindMonsterSystem))]
    public partial struct TowerShootingSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var tower in SystemAPI.Query<RefRW<Tower>>())
            {
                tower.ValueRW.LastTimeShoot -= SystemAPI.Time.DeltaTime;
                if (tower.ValueRO.LastTimeShoot > 0 || tower.ValueRO.ShootAt.Equals(float3.zero)) continue;

                tower.ValueRW.LastTimeShoot = tower.ValueRO.Cooldown;
                var direction = math.normalize(tower.ValueRO.ShootAt - tower.ValueRO.FirePoint);

                var bullet = state.EntityManager.Instantiate(tower.ValueRO.ProjectilePrefab);
                var bulletTransform = SystemAPI.GetComponentRW<LocalTransform>(bullet);
                var velocity = SystemAPI.GetComponentRW<PhysicsVelocity>(bullet);
                var mass = SystemAPI.GetComponentRW<PhysicsMass>(bullet);
                bulletTransform.ValueRW.Position = tower.ValueRO.FirePoint;
                bulletTransform.ValueRW.Rotation = quaternion.LookRotation(direction, math.up());

                var impulse = direction * tower.ValueRO.ProjectileSpeed * mass.ValueRO.InverseMass;
                velocity.ValueRW.ApplyLinearImpulse(mass.ValueRO, impulse);
            }

        }
    }
}