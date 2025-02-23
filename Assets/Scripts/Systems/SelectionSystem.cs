using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts
{
    public partial struct SelectionSystem : ISystem
    {
        private float3 _startPosition;

        public void OnUpdate(ref SystemState state)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                var selectionArea = SelectionAreaManager.GetSelectionArea();
                const int multipleSelectionAreaSize = 40;
                if (selectionArea.height + selectionArea.width > multipleSelectionAreaSize)
                {
                    foreach (var (selected, transform, entity) in SystemAPI.Query<Selected, LocalTransform>()
                                 .WithDisabled<Selected>().WithEntityAccess())
                    {
                        var position = Camera.main.WorldToScreenPoint(transform.Position);
                        if (!selectionArea.Contains(position)) continue;

                        SystemAPI.SetComponentEnabled<Selected>(entity, true);
                        SystemAPI.GetComponentRW<LocalTransform>(selected.SelectedVisual).ValueRW.Scale = 1;
                    }
                }
                else
                {
                    var collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
                    var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    var selectableLayers = 6;

                    var raycastInput = new RaycastInput
                    {
                        Start = cameraRay.GetPoint(0),
                        End = cameraRay.GetPoint(9999),
                        Filter = new CollisionFilter
                        {
                            BelongsTo = ~0u,
                            CollidesWith = 1u << selectableLayers,
                            GroupIndex = 0
                        }
                    };

                    if (collisionWorld.CastRay(raycastInput, out var hit))
                    {
                        var selected = SystemAPI.GetComponentRW<Selected>(hit.Entity);
                        var selectedBool = SystemAPI.IsComponentEnabled<Selected>(hit.Entity);

                        SystemAPI.SetComponentEnabled<Selected>(hit.Entity, !selectedBool);
                        SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRW.SelectedVisual).ValueRW.Scale =
                            selectedBool ? 0 : 1;
                        if (selectedBool && SystemAPI.HasComponent<Mover>(hit.Entity))
                            SystemAPI.GetComponentRW<Mover>(hit.Entity).ValueRW.TargetPosition = float3.zero;
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                foreach (var (selected, mover, entity) in SystemAPI.Query<RefRW<Selected>, RefRW<Mover>>().WithEntityAccess())
                {
                    SystemAPI.SetComponentEnabled<Selected>(entity, false);
                    SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRW.SelectedVisual).ValueRW.Scale = 0;
                    mover.ValueRW.TargetPosition = float3.zero;
                }
                Debug.Log("Release");
            }
        }
    }
}