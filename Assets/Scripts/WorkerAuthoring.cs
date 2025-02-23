using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class WorkerAuthoring : MonoBehaviour
    {
        public class Baker : Baker<WorkerAuthoring>
        {
            public override void Bake(WorkerAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Worker());
            }
        }
    }

    public struct Worker : IComponentData
    {

    }
}
