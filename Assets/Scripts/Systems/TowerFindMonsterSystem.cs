using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts
{
    public partial struct TowerFindMonsterSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (transform, tower) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<Tower>>())
            {
                var closestPosition = float3.zero;
                var minDistance = float.MaxValue;

                foreach (var monsterTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Monster,Mover>())
                {
                    float distance = math.distance(transform.ValueRO.Position, monsterTransform.ValueRO.Position);

                    if (!(distance < minDistance)) continue;
                    minDistance = distance;
                    closestPosition = monsterTransform.ValueRO.Position;
                }

                tower.ValueRW.ShootAt = closestPosition;
            }
        }
    }
}