using System;
using Unity.Entities;

namespace Assets.Scripts
{
    public partial struct DeathSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (health,entity) in SystemAPI.Query<RefRO<Health>>().WithDisabled<LifeTime>().WithEntityAccess())
            {
                if(health.ValueRO.Value<=0)
                    SystemAPI.SetComponentEnabled<LifeTime>(entity,true);
            }
        }
    }
}