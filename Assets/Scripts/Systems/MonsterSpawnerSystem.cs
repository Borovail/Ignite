using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts
{
    public partial struct MonsterSpawnerSystem : ISystem
    {
        private Random _random;
        public void OnCreate(ref SystemState state)
        {
            _random = new Random(1);
        }

        public void OnUpdate(ref SystemState state)
        {
            foreach (var (spawner, localTransform) in
                     SystemAPI.Query<RefRW<MonsterSpawner>, RefRO<LocalTransform>>())
            {
                spawner.ValueRW.Timer -= SystemAPI.Time.DeltaTime;
                if (spawner.ValueRO.Timer > 0)
                    continue;

                spawner.ValueRW.Timer = spawner.ValueRO.SpawnInterval;

                for (int i = 0; i < spawner.ValueRO.MonstersPerSpawn; i++)
                {
                    var spawnOffset = new float3(
                        _random.NextFloat(-spawner.ValueRO.SpawnRadius, spawner.ValueRO.SpawnRadius),
                        0,
                        _random.NextFloat(-spawner.ValueRO.SpawnRadius, spawner.ValueRO.SpawnRadius)
                    );

                    var monsterEntity = state.EntityManager.Instantiate(spawner.ValueRO.MonsterPrefab);
                    SystemAPI.GetComponentRW<LocalTransform>(monsterEntity).ValueRW.Position =
                        localTransform.ValueRO.Position + spawnOffset;
                }
            }
        }
    }
}