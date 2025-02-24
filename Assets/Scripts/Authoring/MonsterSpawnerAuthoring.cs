using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class MonsterSpawnerAuthoring : MonoBehaviour
    {
        public float SpawnInterval;
        public int MonstersPerSpawn;
        public float SpawnRadius;
        public GameObject MonsterPrefab;

        public class Baker : Baker<MonsterSpawnerAuthoring>
        {
            public override void Bake(MonsterSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new MonsterSpawner
                {
                    SpawnInterval = authoring.SpawnInterval,
                    SpawnRadius = authoring.SpawnRadius,
                    MonstersPerSpawn = authoring.MonstersPerSpawn,
                    MonsterPrefab = GetEntity(authoring.MonsterPrefab, TransformUsageFlags.Dynamic)
                });

            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, SpawnRadius);
        }
    }

    public struct MonsterSpawner : IComponentData
    {
        public float Timer;
        public float SpawnInterval;
        public int MonstersPerSpawn;
        public float SpawnRadius;
        public Entity MonsterPrefab;
    }
}