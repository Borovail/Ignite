using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct FireDamageSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (health, fire) in SystemAPI.Query<RefRW<Health>, RefRW<FireLevel>>().WithAll<Burning>())
        {
            if (fire.ValueRO.Value > 0)
            {
                health.ValueRW.Value -= 1;
                fire.ValueRW.Value += 1; 

                if (fire.ValueRO.Value >= 100)
                {
                    Debug.Log("🔥 Будинок у максимальному вогні!");
                }

                if (health.ValueRO.Value <= 0)
                {
                    Debug.Log("💥 Будинок згорів!");
                }
            }
        }
    }
}
