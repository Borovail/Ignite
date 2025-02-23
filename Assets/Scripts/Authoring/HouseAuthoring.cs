using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class HouseAuthoring : MonoBehaviour
    {
        public int startHP = 100;
        public int startFireLevel = 0;

        public class Baker : Baker<HouseAuthoring>
        {
            public override void Bake(HouseAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                // Додаємо компоненти
                AddComponent(entity, new Health { Value = authoring.startHP });
                AddComponent(entity, new FireLevel { Value = authoring.startFireLevel });
                AddComponent<HouseTag>(entity);
            }
        }
    }
}
