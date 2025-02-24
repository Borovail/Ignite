using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts
{
    public partial struct WorkerMonsterCollisionSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (transform, fire, entity) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<Fire>>().WithAll<Worker>().WithEntityAccess())
            {
                var fireRange = 1f;

                foreach (var monsterTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Monster, Mover>())
                {
                    float distance = math.distance(transform.ValueRO.Position, monsterTransform.ValueRO.Position);

                    if (distance > fireRange) continue;

                    fire.ValueRW.FireLevel = math.min(1 + fire.ValueRW.FireLevel, fire.ValueRW.ThresholdToStartBurning);
                    if (fire.ValueRO.FireLevel >= fire.ValueRO.ThresholdToStartBurning)
                        SystemAPI.SetComponentEnabled<Burning>(entity, true);
                }
            }

        }
    }
}