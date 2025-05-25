using Exerussus._1EasyEcs.Scripts.Core;
using Leopotam.EcsLite;
using UnityEngine;

namespace Exerussus.EcsUI.Systems
{
    public class RotationSystem : EasySystem<PoolerUI>
    {
        private EcsFilter _rotationFilter;
        
        protected override void Initialize()
        {
            _rotationFilter = World.Filter<EcsUIData.View>().Inc<EcsUIData.RotationTime>().Inc<EcsUIData.TargetViewRotation>().Inc<EcsUIData.StandardViewRotation>().Exc<EcsUIData.DestroyProcess>().End();
        }

        protected override void Update()
        {
            _rotationFilter.Foreach(OnRotationUpdate);
        }

        private void OnRotationUpdate(int entity)
        {
            ref var timeData = ref Pooler.RotationTime.Get(entity);
            ref var targetData = ref Pooler.TargetViewRotation.Get(entity);
            ref var viewData = ref Pooler.View.Get(entity);
            ref var entityUiData = ref Pooler.EntityUI.Get(entity);

            if (!entityUiData.Value.IsRotationActive) return; 

            if (viewData.Value == null || timeData.TimeRemaining <= 0f) return;

            var rectTransform = viewData.Value;
            var isLocal = targetData.IsLocal;
            var deltaTime = DeltaTime;
            var totalDuration = timeData.Value;
            var remainingTime = timeData.TimeRemaining;
            var progress = Mathf.Clamp01(1f - (remainingTime - deltaTime) / totalDuration);
            var targetRotation = Quaternion.Euler(targetData.Value);

            if (isLocal) rectTransform.localRotation = Quaternion.Lerp(rectTransform.localRotation, targetRotation, progress);
            else rectTransform.rotation = Quaternion.Lerp(rectTransform.rotation, targetRotation, progress);
            

            timeData.TimeRemaining -= deltaTime;

            if (timeData.TimeRemaining <= 0f)
            {
                if (isLocal) rectTransform.localRotation = targetRotation;
                else rectTransform.rotation = targetRotation;
                
                if (Pooler.RotationReplaceableCallback.Has(entity))
                {
                    ref var replaceableCallbackData = ref Pooler.RotationReplaceableCallback.Get(entity);
                    replaceableCallbackData.Value?.Invoke();
                    replaceableCallbackData.Value = null;
                }
                
                if (Pooler.RotationGuaranteedCallback.Has(entity))
                {
                    ref var gCallbackData = ref Pooler.RotationGuaranteedCallback.Get(entity);
                    gCallbackData.Value?.Invoke();
                    gCallbackData.Value = null;
                }
                
                if (Pooler.RotationPostProcessCallback.Has(entity))
                {
                    ref var ppCallbackData = ref Pooler.RotationPostProcessCallback.Get(entity);
                    foreach (var action in ppCallbackData.Value) action?.Invoke();
                    ppCallbackData.Value.Clear();
                }

                if (targetData.LastTick != Time.frameCount)
                {
                    Pooler.TargetViewRotation.Del(entity);
                    Pooler.RotationTime.Del(entity);
                }
            }
        }
    }
}