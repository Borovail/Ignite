using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts
{
    public class HitAuthoring : MonoBehaviour
    {
        public class Baker : Baker<HitAuthoring>
        {
            public override void Bake(HitAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Hit());
                SetComponentEnabled<Hit>(entity, false);
            }
        }
    }

    public struct Hit : IComponentData, IEnableableComponent
    {
        public float3 Position;
        public float PushForce;
        public float Damage;
    }

}