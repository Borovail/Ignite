using Unity.Burst;
using Unity.Entities;

namespace Game
{


    [BurstCompile]
    public partial struct FireDamageSystem : ISystem // 
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (health, fire) in SystemAPI.Query<RefRW<Health>, RefRO<FireLevel>>().WithAll<HouseTag>())
            {
                if (fire.ValueRO.Value > 0)
                {
                    health.ValueRW.Value -= fire.ValueRO.Value;
                }
            }
        }
    }
}
