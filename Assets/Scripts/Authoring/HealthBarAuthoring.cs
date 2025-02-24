using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class HealthBarAuthoring : MonoBehaviour
    {
        public GameObject HealthBarParent;
        public GameObject HealthBarVisual;

        public class Baker : Baker<HealthBarAuthoring>
        {
            public override void Bake(HealthBarAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new HealthBar
                {
                    HealthBarParent = GetEntity(authoring.HealthBarParent, TransformUsageFlags.Dynamic),
                    HealthBarVisual = GetEntity(authoring.HealthBarVisual, TransformUsageFlags.NonUniformScale)
                });
            }
        }
    }

    public struct HealthBar : IComponentData
    {
        public Entity HealthBarParent;
        public Entity HealthBarVisual;
    }
}