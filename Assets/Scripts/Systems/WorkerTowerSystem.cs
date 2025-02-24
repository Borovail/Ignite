using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts
{
    public partial struct WorkerTowerSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var tower in SystemAPI.Query<RefRW<WorkerTower>>())
            {
                tower.ValueRW.SpawnTimer -= SystemAPI.Time.DeltaTime;
                if (!(tower.ValueRO.SpawnTimer <= 0)) continue;

                tower.ValueRW.SpawnTimer = tower.ValueRO.SpawnCooldown;

                foreach (var worker in state.EntityManager.Instantiate(tower.ValueRO.WorkerPrefab, tower.ValueRO.SpawnPerOnce,
                             Allocator.Temp))
                    SystemAPI.GetComponentRW<LocalTransform>(worker).ValueRW.Position = tower.ValueRO.SpawnPoint;
            }
        }
    }
}