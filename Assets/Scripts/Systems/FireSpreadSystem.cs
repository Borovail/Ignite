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

        foreach (var (fire, entity) in SystemAPI.Query<RefRW<FireLevel>>().WithEntityAccess())
        {
            fire.ValueRW.Value += 1; // Оновлено відповідно до правильної назви поля

            if (fire.ValueRW.Value > 10)
            {
                ecb.DestroyEntity(entity); // Видаляємо сутність, якщо рівень вогню перевищує 10
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
