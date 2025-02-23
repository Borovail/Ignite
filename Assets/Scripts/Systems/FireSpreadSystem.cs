using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Game;

public partial struct FireSpreadSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);

        foreach (var fire in SystemAPI.Query<RefRW<FireLevel>>().WithEntityAccess())
        {
            var entity = fire.GetEntity();
            fire.ValueRW.Level += 1; // Приклад логіки поширення вогню

            if (fire.ValueRW.Level > 10)
            {
                ecb.DestroyEntity(entity); // Наприклад, якщо рівень вогню перевищує 10, видаляємо сутність
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
