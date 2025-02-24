using System;
using System.Threading;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts
{
    public partial struct FireSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (fire, health, transform, entity) in SystemAPI.Query<RefRW<Fire>, RefRW<Health>, RefRO<LocalTransform>>().WithAll<Burning>().WithDisabled<LifeTime>().WithEntityAccess())
            {
                fire.ValueRW.CooldownTimer -= SystemAPI.Time.DeltaTime;
                if (!(fire.ValueRO.CooldownTimer <= 0)) continue;

                health.ValueRW.Value -= fire.ValueRO.FireDamage;
                if (health.ValueRO.Value <= 0 && SystemAPI.HasComponent<Building>(entity))
                {
                    foreach (var (buildingTransform, buildingFire, buildingEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<Fire>>()
                                 .WithAll<Fire>().WithDisabled<LifeTime, Burning>().WithEntityAccess())
                    {
                        float distance = math.distance(transform.ValueRO.Position, buildingTransform.ValueRO.Position);

                        if (distance > fire.ValueRO.SpreadRadius) continue;
                        buildingFire.ValueRW.FireLevel = buildingFire.ValueRO.ThresholdToStartBurning;
                        SystemAPI.SetComponentEnabled<Burning>(buildingEntity, true);
                    }
                }

                fire.ValueRW.CooldownTimer = fire.ValueRO.DamageCooldown;
            }

        }
    }
}
