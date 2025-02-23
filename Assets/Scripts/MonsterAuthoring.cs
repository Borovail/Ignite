using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class MonsterAuthoring : MonoBehaviour
    {
        public class Baker : Baker<MonsterAuthoring>
        {
            public override void Bake(MonsterAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,new Monster());
            }
        }
    }

    public struct Monster : IComponentData
    {

    }
}