using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class BulletAuthoring : MonoBehaviour
    {
        [Header("Collisions")]
        public float Radius = 0.1f;
        public LayerMask TriggerToExplodeLayer;
        public LayerMask HitLayer;

        [Header("Bullet")]
        public float PushForce = 10;
        public float SplashRadius = 3;
        public float Damage =50f;

        public class Baker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Bullet
                {
                    Radius = authoring.Radius,
                    TriggerToExplodeLayer = authoring.TriggerToExplodeLayer,
                    HitLayer = authoring.HitLayer,
                    PushForce = authoring.PushForce,
                    SplashRadius = authoring.SplashRadius,
                    Damage = authoring.Damage
                });

            }
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }

    public struct Bullet : IComponentData
    {
        public float Radius;
        public LayerMask TriggerToExplodeLayer;
        public LayerMask HitLayer;
        public float PushForce;
        public float SplashRadius;
        public float Damage;
    }
}