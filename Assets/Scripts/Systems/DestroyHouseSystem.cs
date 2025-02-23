using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct DestroyHouseSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (health, entity) in SystemAPI.Query<RefRO<Health>>().WithEntity())
        {
            if (health.ValueRO.Value <= 0)
            {
                ecb.DestroyEntity(entity);
                Debug.Log("💀 Будинок згорів і зник!");
            }
        }

        ecb.Playback(state.EntityManager);
    }
}
