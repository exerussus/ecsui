using Exerussus._1EasyEcs.Scripts.Core;
using Leopotam.EcsLite;
using UnityEngine;

namespace Plugins.Exerussus.EcsUI.Systems
{
    public class SizeSystem : EasySystem<PoolerUI>
    {
        private EcsFilter _scaleFilter;
        
        protected override void Initialize()
        {
            _scaleFilter = World.Filter<EcsUIData.View>().Inc<EcsUIData.ScaleTime>().Inc<EcsUIData.TargetViewScale>().Inc<EcsUIData.StandardViewScale>().Exc<EcsUIData.DestroyProcess>().End();
        }

        protected override void Update()
        {
            _scaleFilter.Foreach(OnSizeUpdate);
        }

        private void OnSizeUpdate(int entity)
        {
            ref var timeData = ref Pooler.ScaleTime.Get(entity);
            ref var targetData = ref Pooler.TargetViewScale.Get(entity);
            ref var viewData = ref Pooler.View.Get(entity);

            if (viewData.Value == null || timeData.TimeRemaining <= 0f) return;

            var rectTransform = viewData.Value;
            var target = targetData.Value;

            var deltaTime = DeltaTime;
            var totalDuration = timeData.Value;
            var remainingTime = timeData.TimeRemaining;

            var progress = 1f - (remainingTime / totalDuration);
            progress = Mathf.Clamp01(progress + deltaTime / totalDuration);

            var current = rectTransform.localScale;
            rectTransform.localScale = Vector3.Lerp(current, target, progress);

            timeData.TimeRemaining -= deltaTime;

            if (timeData.TimeRemaining <= 0f)
            {
                rectTransform.localScale = target;

                if (Pooler.ScaleReplaceableCallback.Has(entity))
                {
                    ref var replaceableCallbackData = ref Pooler.ScaleReplaceableCallback.Get(entity);
                    replaceableCallbackData.Value?.Invoke();
                    replaceableCallbackData.Value = null;
                }
                
                if (Pooler.ScaleGuaranteedCallback.Has(entity))
                {
                    ref var gCallbackData = ref Pooler.ScaleGuaranteedCallback.Get(entity);
                    gCallbackData.Value?.Invoke();
                    gCallbackData.Value = null;
                }

                if (Pooler.ScalePostProcessCallback.Has(entity))
                {
                    ref var ppCallbackData = ref Pooler.ScalePostProcessCallback.Get(entity);
                    foreach (var action in ppCallbackData.Value) action?.Invoke();
                    ppCallbackData.Value.Clear();
                }

                if (targetData.LastTick != Time.frameCount)
                {
                    Pooler.TargetViewScale.Del(entity);
                    Pooler.ScaleTime.Del(entity);
                }
            }
        }
    }
}