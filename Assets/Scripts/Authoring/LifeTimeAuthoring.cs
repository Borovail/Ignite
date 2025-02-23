using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class LifeTimeAuthoring  : MonoBehaviour
    {
        public float LifeTime;
        public class Baker : Baker<LifeTimeAuthoring>
        {
            public override void Bake(LifeTimeAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,new LifeTime
                {
                    Value = authoring.LifeTime
                });
                SetComponentEnabled<LifeTime>(entity,false);
            }
        }
    }

    public struct LifeTime : IComponentData,IEnableableComponent
    {
        public float Value;
    }
}