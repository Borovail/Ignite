using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;

public partial struct DestroyHouseSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);

        foreach (var health in SystemAPI.Query<RefRO<Game.Health>>().WithEntityAccess())
        {
            var entity = health.GetEntity();
            if (health.ValueRO.Value <= 0)
            {
                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}


