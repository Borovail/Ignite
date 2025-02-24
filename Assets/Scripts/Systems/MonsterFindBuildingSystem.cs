using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts
{
    public partial struct MonsterFindBuildingSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (mover, transform,monster) in SystemAPI.Query<RefRW<Mover>, RefRO<LocalTransform>,RefRW<Monster>>())
            {
                var closestBuildingPosition = float3.zero;
                var minDistance = float.MaxValue;

                foreach (var (buildingTransform,entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Building>().WithNone<LifeTime>().WithEntityAccess())
                {
                    float distance = math.distance(transform.ValueRO.Position, buildingTransform.ValueRO.Position);

                    if (!(distance < minDistance)) continue;
                    minDistance = distance;
                    closestBuildingPosition = buildingTransform.ValueRO.Position;
                    monster.ValueRW.Building = entity;
                }

                mover.ValueRW.TargetPosition = closestBuildingPosition;
            }
        }
    }
}