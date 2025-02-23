using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public partial struct FireSpreadSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        
        foreach (var (fire, housePos, entity) in SystemAPI.Query<RefRO<FireLevel>, RefRO<LocalTransform>>().WithAll<Burning>().WithEntity())
        {
            if (fire.ValueRO.Value < 100) continue; 

            foreach (var (neighborFire, neighborPos, neighborEntity) in SystemAPI.Query<RefRW<FireLevel>, RefRO<LocalTransform>>().WithAll<HouseTag>().WithEntity())
            {
                if (neighborEntity == entity) continue; 

                float distance = math.distance(housePos.ValueRO.Position, neighborPos.ValueRO.Position);
                if (distance < 5f) 
                {
                    if (neighborFire.ValueRO.Value == 0) 
                    {
                        neighborFire.ValueRW.Value = 10; 
                        ecb.AddComponent<Burning>(neighborEntity);
                        Debug.Log("🔥 Вогонь поширився на сусідній будинок!");
                    }
                }
            }
        }

        ecb.Playback(state.EntityManager);
    }
}
