using Unity.Entities;

namespace Assets.Scripts
{
    public partial struct LifeTimeSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (spawnAction, entity) in SystemAPI.Query<RefRW<LifeTime>>().WithEntityAccess())
            {
                spawnAction.ValueRW.Value -= SystemAPI.Time.DeltaTime;

                if (!(spawnAction.ValueRO.Value <= 0)) continue;

                ecb.DestroyEntity(entity);
            }
        }
    }
}