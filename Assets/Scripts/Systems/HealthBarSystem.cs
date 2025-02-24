using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts
{
    public partial struct HealthBarSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var cameraForward = Camera.main == null ? Vector3.zero : Camera.main.transform.forward;

            foreach (var (healthBar, health, transform) in SystemAPI.Query<RefRW<HealthBar>, RefRO<Health>, RefRW<LocalTransform>>())
            {
                var barParentTransform = SystemAPI.GetComponentRW<LocalTransform>(healthBar.ValueRW.HealthBarParent);

                if (health.ValueRO.Value == health.ValueRO.MaxValue)
                {
                    barParentTransform.ValueRW.Scale = 0;
                    continue;
                }

                barParentTransform.ValueRW.Scale = 1;

                SystemAPI.GetComponentRW<PostTransformMatrix>(healthBar.ValueRW.HealthBarVisual).ValueRW.Value
                                   = float4x4.Scale(health.ValueRO.Value / health.ValueRO.MaxValue, 1, 1);
            }
        }
    }
}