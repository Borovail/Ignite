using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class WaterSourceAuthoring : MonoBehaviour
    {
        public class Baker : Baker<WaterSourceAuthoring>
        {
            public override void Bake(WaterSourceAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,new WaterSource());
            }
        }
    }

    public struct WaterSource : IComponentData
    {

    }
}