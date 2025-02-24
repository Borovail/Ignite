using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts
{
    public class TowerAuthoring : MonoBehaviour
    {
        public float Cooldown;
        public float ProjectileSpeed;
        public GameObject ProjectilePrefab;
        public Transform FirePoint;

        public class Baker : Baker<TowerAuthoring>
        {
            public override void Bake(TowerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Tower
                {
                    Cooldown = authoring.Cooldown,
                    ProjectileSpeed = authoring.ProjectileSpeed,
                    ProjectilePrefab = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic),
                    FirePoint = authoring.FirePoint.position
                });
            }
        }
    }

    public struct Tower : IComponentData
    {
        public float Cooldown;
        public float ProjectileSpeed;
        public Entity ProjectilePrefab;
        public float3 FirePoint;

        public float LastTimeShoot;
        public float3 ShootAt;
    }
}