using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts
{
    public partial struct MonsterAttackSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (transform, mover, entity) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<Mover>>().WithEntityAccess())
            {
                Debug.Log(math.distance(mover.ValueRO.TargetPosition, transform.ValueRO.Position));
                if (math.distance(mover.ValueRO.TargetPosition, transform.ValueRO.Position) < 1.5f)
                {
                    //TODO
                    //Do some damage to building


                    SystemAPI.SetComponentEnabled<LifeTime>(entity, true);
                }
            }
        }
    }
}