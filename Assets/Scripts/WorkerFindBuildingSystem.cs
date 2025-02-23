using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts
{
    public partial struct WorkerFindBuildingSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (mover, transform) in SystemAPI.Query<RefRW<Mover>, RefRO<LocalTransform>>().WithAll<Selected, Worker>())
            {
                var closestBuildingPosition = float3.zero;
                var minDistance = float.MaxValue;

                foreach (var buildingTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Selected,Building>())
                {
                    float distance = math.distance(transform.ValueRO.Position, buildingTransform.ValueRO.Position);

                    if (!(distance < minDistance)) continue;
                    minDistance = distance;
                    closestBuildingPosition = buildingTransform.ValueRO.Position;
                }

                mover.ValueRW.TargetPosition = closestBuildingPosition;
            }
        }
    }
}