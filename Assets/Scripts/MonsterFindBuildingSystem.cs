using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts
{
    public partial struct MonsterFindBuildingSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (mover, transform) in SystemAPI.Query<RefRW<Mover>, RefRO<LocalTransform>>().WithAll<Monster>())
            {
                var closestBuildingPosition = float3.zero;
                var minDistance = float.MaxValue;

                foreach (var buildingTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Building>())
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