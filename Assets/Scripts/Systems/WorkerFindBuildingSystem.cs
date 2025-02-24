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
            foreach (var (mover, transform,worker) in SystemAPI.Query<RefRW<Mover>, RefRO<LocalTransform>,RefRW<Worker>>().WithAll<Selected>())
            {
                var closestBuildingPosition = float3.zero;
                var minDistance = float.MaxValue;

                foreach (var (buildingTransform,entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Selected,Building>().WithEntityAccess())
                {
                    float distance = math.distance(transform.ValueRO.Position, buildingTransform.ValueRO.Position);

                    if (!(distance < minDistance)) continue;
                    minDistance = distance;
                    closestBuildingPosition = buildingTransform.ValueRO.Position;
                    worker.ValueRW.Building = entity;
                }

                mover.ValueRW.TargetPosition = closestBuildingPosition;
            }
        }
    }
}