using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class SelectedAuthoring : MonoBehaviour
    {
        public GameObject SelectedVisual;
        public class Baker : Baker<SelectedAuthoring>
        {
            public override void Bake(SelectedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Selected
                {
                    SelectedVisual = GetEntity(authoring.SelectedVisual, TransformUsageFlags.Dynamic)
                });
                SetComponentEnabled<Selected>(entity,false);
            }
        }
    }

    public struct Selected : IComponentData, IEnableableComponent
    {
        public Entity SelectedVisual;
    }
}