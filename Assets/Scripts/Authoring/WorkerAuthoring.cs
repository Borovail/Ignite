using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class WorkerAuthoring : MonoBehaviour
    {
        public int RepairAmount = 1;
        public float RepairCooldown = 1;
        public bool HasWater;

        public class Baker : Baker<WorkerAuthoring>
        {
            public override void Bake(WorkerAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Worker
                {
                    RepairAmount = authoring.RepairAmount,
                    RepairCooldown = authoring.RepairCooldown,
                    RepairTimer = authoring.RepairCooldown
                });
            }
        }
    }

    public struct Worker : IComponentData
    {
        public float RepairAmount;
        public float RepairCooldown;
        public float RepairTimer;
        public Entity Building;
    }
}
