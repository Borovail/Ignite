using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class BuildingAuthoring : MonoBehaviour
    {
        public class Baker : Baker<BuildingAuthoring>
        {
            public override void Bake(BuildingAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Building());
            }
        }
    }

    public struct Building : IComponentData
    {

    }
}