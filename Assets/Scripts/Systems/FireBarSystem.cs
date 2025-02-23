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
            foreach (var (fireBar, fire) in SystemAPI.Query<RefRW<FireBar>, RefRO<Fire>>())
            {
                if (fire.ValueRO.FireLevel == 0)
                {
                    SystemAPI.GetComponentRW<LocalTransform>(fireBar.ValueRW.FireBarParent).ValueRW.Scale = 0;
                    continue;
                }

                SystemAPI.GetComponentRW<LocalTransform>(fireBar.ValueRW.FireBarParent).ValueRW.Scale = 1;
                var fireLevelNormalized =fire.ValueRO.FireLevel / (float)fire.ValueRO.ThresholdToStartBurning;
                SystemAPI.GetComponentRW<LocalTransform>(fireBar.ValueRW.FireBarVisual).ValueRW.Scale = fireLevelNormalized;
            }
        }
    }
}