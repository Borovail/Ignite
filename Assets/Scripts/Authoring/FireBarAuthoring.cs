using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class FireBarAuthoring : MonoBehaviour
    {
        public GameObject FireBarParent;
        public GameObject FireBarVisual;

        public class Baker : Baker<FireBarAuthoring>
        {
            public override void Bake(FireBarAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new FireBar
                {
                    FireBarParent = GetEntity(authoring.FireBarParent, TransformUsageFlags.Dynamic),
                    FireBarVisual = GetEntity(authoring.FireBarVisual, TransformUsageFlags.Dynamic)
                });
            }
        }
    }

    public struct FireBar : IComponentData
    {
        public Entity FireBarParent;
        public Entity FireBarVisual;
    }
}