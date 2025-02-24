using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class HasWaterAuthoring : MonoBehaviour
    {
        public GameObject WaterVisual;

        public class Baker : Baker<HasWaterAuthoring>
        {
            public override void Bake(HasWaterAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new HasWater
                {
                    WaterVisual = GetEntity(authoring.WaterVisual, TransformUsageFlags.Dynamic)
                });
                SetComponentEnabled<HasWater>(entity, false);
            }
        }
    }

    public struct HasWater : IComponentData, IEnableableComponent
    {
        public Entity WaterVisual;
    }
}