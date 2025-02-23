using System;
using System.Threading;
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
                if (health.ValueRO.Value <= 0)
                {
                    SystemAPI.SetComponentEnabled<LifeTime>(entity, true);

                    var minDistance = float.MaxValue;
                    var closestBuilding = Entity.Null;

                    foreach (var (buildingTransform, buildingEntity) in SystemAPI.Query<RefRO<LocalTransform>>()
                                 .WithAll<Building>().WithDisabled<LifeTime, Burning>().WithEntityAccess())
                    {
                        float distance = math.distance(transform.ValueRO.Position, buildingTransform.ValueRO.Position);

                        if (!(distance < minDistance)) continue;
                        minDistance = distance;
                        closestBuilding = buildingEntity;
                    }

                    if (SystemAPI.Exists(closestBuilding))
                    {
                        SystemAPI.SetComponentEnabled<Burning>(closestBuilding, true);
                        Debug.Log("Burn another building");
                    }
                }
                fire.ValueRW.CooldownTimer = fire.ValueRO.DamageCooldown;
            }

        }
    }
}
