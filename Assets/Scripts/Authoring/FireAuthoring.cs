﻿using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class FireAuthoring : MonoBehaviour
    {
        public int ThresholdToStartBurning;
        public int FireDamage =1;
        public int DamageCooldown = 1;

        public class Baker : Baker<FireAuthoring>
        {
            public override void Bake(FireAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,new Fire
                {
                    FireDamage = authoring.FireDamage,
                    ThresholdToStartBurning = authoring.ThresholdToStartBurning,
                    DamageCooldown = authoring.DamageCooldown,
                    CooldownTimer = authoring.DamageCooldown
                });
                AddComponent(entity,new Burning());
                SetComponentEnabled<Burning>(entity,false);
            }
        }

    }

    public struct Fire : IComponentData
    {
        public int ThresholdToStartBurning;
        public int FireDamage;
        public int FireLevel;
        public float DamageCooldown;
        public float CooldownTimer;
    }

    public struct Burning : IComponentData, IEnableableComponent
    {

    }
}