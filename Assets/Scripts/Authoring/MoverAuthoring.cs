using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class MoverAuthoring : MonoBehaviour
{
    public float MaxSpeed = 5f;
    public float Acceleration = 5f;
    public float RotationSpeed;
    public float HitStunDuration = 1f;


    public class Baker : Baker<MoverAuthoring>
    {
        public override void Bake(MoverAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Mover
            {
                MaxSpeed = authoring.MaxSpeed,
                RotationSpeed = authoring.RotationSpeed,
                HitStunDuration = authoring.HitStunDuration,
                HitStunTimer = authoring.HitStunDuration,
                Acceleration = authoring.Acceleration
            });
        }
    }

}

public struct Mover : IComponentData, IEnableableComponent
{
    public float MaxSpeed;
    public float Acceleration;
    public float RotationSpeed;
    public float3 TargetPosition;

    public float HitStunDuration;
    public float HitStunTimer;
}