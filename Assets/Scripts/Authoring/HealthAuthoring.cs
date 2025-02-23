using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class HealthAuthoring : MonoBehaviour
    {
        public float Health = 100;
        public float MaxHealth = 100;

        public class Baker : Baker<HealthAuthoring>
        {
            public override void Bake(HealthAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,new Health
                {
                    Value = authoring.Health,
                    MaxValue = authoring.MaxHealth
                });
            }
        }
    }

    public struct Health : IComponentData
    {
        public float Value;
        public float MaxValue;
    }
}