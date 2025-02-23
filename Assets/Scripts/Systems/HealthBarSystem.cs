using Unity.Entities;
using Unity.Transforms;

namespace Assets.Scripts
{
    public partial struct HealthBarSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (healthBar, health) in SystemAPI.Query<RefRW<HealthBar>, RefRO<Health>>())
            {
                if (health.ValueRO.Value == health.ValueRO.MaxValue)
                {
                    SystemAPI.GetComponentRW<LocalTransform>(healthBar.ValueRW.HealthBarParent).ValueRW.Scale = 0;
                    continue;
                }

                SystemAPI.GetComponentRW<LocalTransform>(healthBar.ValueRW.HealthBarParent).ValueRW.Scale = 1;
                var healthNormalized = health.ValueRO.Value / health.ValueRO.MaxValue;
                SystemAPI.GetComponentRW<LocalTransform>(healthBar.ValueRW.HealthBarVisual).ValueRW.Scale = healthNormalized;
            }
        }
    }
}