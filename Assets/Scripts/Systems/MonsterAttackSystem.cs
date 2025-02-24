using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts
{
    public partial struct MonsterAttackSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (transform, mover, monster, entity) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<Mover>, RefRO<Monster>>()
                         .WithNone<LifeTime>().WithEntityAccess())
            {
                if (!(math.distance(mover.ValueRO.TargetPosition, transform.ValueRO.Position) < 1.5f)) continue;
                if (!SystemAPI.Exists(monster.ValueRO.Building)) continue;

                var health = SystemAPI.GetComponentRW<Health>(monster.ValueRO.Building);
                health.ValueRW.Value -= monster.ValueRO.Damage;
                if (health.ValueRO.Value <= 0)
                    SystemAPI.SetComponentEnabled<LifeTime>(monster.ValueRO.Building, true);

                var fire = SystemAPI.GetComponentRW<Fire>(monster.ValueRO.Building);
                fire.ValueRW.FireLevel = math.min(monster.ValueRO.FirePower + fire.ValueRW.FireLevel, fire.ValueRW.ThresholdToStartBurning);
                if (fire.ValueRO.FireLevel >= fire.ValueRO.ThresholdToStartBurning)
                    SystemAPI.SetComponentEnabled<Burning>(monster.ValueRO.Building, true);

                SystemAPI.SetComponentEnabled<LifeTime>(entity, true);
            }
        }
    }
}