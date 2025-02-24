using System.Threading;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts
{
    public partial struct WorkerRepairSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (transform, mover, worker, hasWater,workerFire, entity) in SystemAPI
                         .Query<RefRO<LocalTransform>, RefRW<Mover>, RefRW<Worker>, RefRW<HasWater>,RefRW<Fire>>().WithAll<Selected>().WithPresent<HasWater>().WithEntityAccess())
            {
                worker.ValueRW.RepairTimer -= SystemAPI.Time.DeltaTime;

                if (!SystemAPI.Exists(worker.ValueRO.Building)) continue;
                if (!(math.distance(mover.ValueRO.TargetPosition, transform.ValueRO.Position) < 1.5f)) continue;

                if (SystemAPI.IsComponentEnabled<Burning>(worker.ValueRO.Building) &&
                    !SystemAPI.IsComponentEnabled<HasWater>(entity))
                {
                    workerFire.ValueRW.FireLevel = math.min(1 + workerFire.ValueRW.FireLevel, workerFire.ValueRW.ThresholdToStartBurning);
                    if (workerFire.ValueRO.FireLevel >= workerFire.ValueRO.ThresholdToStartBurning)
                        SystemAPI.SetComponentEnabled<Burning>(entity, true);
                }

                if (SystemAPI.HasComponent<WaterSource>(worker.ValueRO.Building) && !SystemAPI.IsComponentEnabled<HasWater>(entity))
                {
                    SystemAPI.GetComponentRW<LocalTransform>(hasWater.ValueRW.WaterVisual).ValueRW.Scale = 1;
                    SystemAPI.SetComponentEnabled<HasWater>(entity, true);
                }

                if (SystemAPI.IsComponentEnabled<Burning>(worker.ValueRO.Building) && SystemAPI.IsComponentEnabled<HasWater>(entity))
                {
                    var fire = SystemAPI.GetComponentRW<Fire>(worker.ValueRO.Building);
                    fire.ValueRW.FireLevel = math.max(0, fire.ValueRW.FireLevel - 1);
                    if (fire.ValueRW.FireLevel == 0)
                        SystemAPI.SetComponentEnabled<Burning>(worker.ValueRO.Building, false);

                    SystemAPI.SetComponentEnabled<HasWater>(entity, false);
                    SystemAPI.GetComponentRW<LocalTransform>(hasWater.ValueRW.WaterVisual).ValueRW.Scale = 0;
                }
                

                if (!(worker.ValueRO.RepairTimer <= 0)) continue;

                var health = SystemAPI.GetComponentRW<Health>(worker.ValueRO.Building);
                health.ValueRW.Value = math.min(health.ValueRO.Value + worker.ValueRO.RepairAmount, health.ValueRO.MaxValue);
                worker.ValueRW.RepairTimer = worker.ValueRO.RepairCooldown;
            }
        }
    }
}