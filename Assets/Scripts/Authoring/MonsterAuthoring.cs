using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class MonsterAuthoring : MonoBehaviour
    {
        public int Damage = 10;
        public int FirePower = 1;

        public class Baker : Baker<MonsterAuthoring>
        {
            public override void Bake(MonsterAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,new Monster
                {
                    Damage = authoring.Damage,
                    FirePower = authoring.FirePower
                });
            }
        }
    }

    public struct Monster : IComponentData
    {
        public Entity Building;
        public int Damage;
        public int FirePower;
    }
}