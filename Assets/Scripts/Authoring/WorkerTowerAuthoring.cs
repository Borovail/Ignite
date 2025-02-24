using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts
{
    public class WorkerTowerAuthoring : MonoBehaviour
    {
        public int SpawnPerOnce;
        public float SpawnCooldown;
        public GameObject WorkerPrefab;
        public Transform SpawnPoint;

        public class Baker : Baker<WorkerTowerAuthoring>
        {
            public override void Bake(WorkerTowerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new WorkerTower
                {
                    SpawnCooldown = authoring.SpawnCooldown,
                    SpawnPerOnce = authoring.SpawnPerOnce,
                    SpawnTimer = authoring.SpawnCooldown,

                    SpawnPoint = authoring.SpawnPoint.position,
                    WorkerPrefab = GetEntity(authoring.WorkerPrefab,TransformUsageFlags.Dynamic)
                });
            }
        }
    }

    public struct WorkerTower : IComponentData
    {
        public int SpawnPerOnce;
        public float SpawnCooldown;
        public float SpawnTimer;

        public float3 SpawnPoint;
        public Entity WorkerPrefab;
    }
}