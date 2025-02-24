using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts
{
    public partial struct FireBarSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var cameraForward = Camera.main == null ? Vector3.zero : Camera.main.transform.forward;

            foreach (var (fireBar, fire) in SystemAPI.Query<RefRW<FireBar>, RefRO<Fire>>())
            {
                var barParentTransform = SystemAPI.GetComponentRW<LocalTransform>(fireBar.ValueRW.FireBarParent);

                if (fire.ValueRO.FireLevel == 0)
                {
                    barParentTransform.ValueRW.Scale = 0;
                    continue;
                }
                barParentTransform.ValueRW.Scale = 1;

                SystemAPI.GetComponentRW<PostTransformMatrix>(fireBar.ValueRW.FireBarVisual).ValueRW.Value =
                     float4x4.Scale(fire.ValueRO.FireLevel / (float)fire.ValueRO.ThresholdToStartBurning, 1, 1);

            }
        }
    }
}