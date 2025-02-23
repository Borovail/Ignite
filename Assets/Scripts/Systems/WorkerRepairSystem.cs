using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts
{
    public partial struct WorkerRepairSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (transform, mover, worker, hasWater, entity) in SystemAPI
                         .Query<RefRO<LocalTransform>, RefRW<Mover>, RefRW<Worker>, RefRW<HasWater>>().WithAll<Selected>().WithPresent<HasWater>().WithEntityAccess())
            {
                worker.ValueRW.RepairTimer -= SystemAPI.Time.DeltaTime;

                if (!SystemAPI.Exists(worker.ValueRO.Building)) continue;
                if (!(math.distance(mover.ValueRO.TargetPosition, transform.ValueRO.Position) < 1.5f)) continue;

                if (SystemAPI.HasComponent<WaterSource>(worker.ValueRO.Building) && !SystemAPI.IsComponentEnabled<HasWater>(entity))
                {
                    SystemAPI.GetComponentRW<LocalTransform>(hasWater.ValueRW.WaterVisual).ValueRW.Scale = 1;
                    SystemAPI.SetComponentEnabled<HasWater>(entity, true);
                }

                if (SystemAPI.IsComponentEnabled<Burning>(worker.ValueRO.Building) && SystemAPI.IsComponentEnabled<HasWater>(entity))
                {
                    SystemAPI.SetComponentEnabled<Burning>(worker.ValueRO.Building, false);
                    SystemAPI.SetComponentEnabled<HasWater>(entity, false);
                    SystemAPI.GetComponentRW<LocalTransform>(hasWater.ValueRW.WaterVisual).ValueRW.Scale = 0;

                    SystemAPI.GetComponentRW<Fire>(worker.ValueRO.Building).ValueRW.FireLevel = 0;
                    var fireBar = SystemAPI.GetComponentRW<FireBar>(worker.ValueRO.Building);
                    SystemAPI.GetComponentRW<LocalTransform>(fireBar.ValueRW.FireBarVisual).ValueRW.Scale = 0;
                    SystemAPI.GetComponentRW<LocalTransform>(fireBar.ValueRW.FireBarVisual).ValueRW.Scale = 0;
                }

                if (!(worker.ValueRO.RepairTimer <= 0)) continue;

                SystemAPI.GetComponentRW<Health>(worker.ValueRO.Building).ValueRW.Value +=
                    worker.ValueRO.RepairAmount;
                worker.ValueRW.RepairTimer = worker.ValueRO.RepairCooldown;
            }
        }
    }
}