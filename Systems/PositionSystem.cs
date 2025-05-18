using Exerussus._1EasyEcs.Scripts.Core;
using Leopotam.EcsLite;
using UnityEngine;

namespace Plugins.Exerussus.EcsUI.Systems
{
    public class PositionSystem : EasySystem<PoolerUI>
    {
        private EcsFilter _positionFilter;
        
        protected override void Initialize()
        {
            _positionFilter = World.Filter<EcsUIData.View>().Inc<EcsUIData.PositionTime>().Inc<EcsUIData.TargetViewPosition>().Inc<EcsUIData.StandardViewPosition>().Exc<EcsUIData.DestroyProcess>().End();
        }

        protected override void Update()
        {
            _positionFilter.Foreach(OnPositionUpdate);
        }

        private void OnPositionUpdate(int entity)
        {
            ref var positionTimeData = ref Pooler.PositionTime.Get(entity);
            ref var targetPositionData = ref Pooler.TargetViewPosition.Get(entity);
            ref var viewData = ref Pooler.View.Get(entity);

            if (viewData.Value == null || positionTimeData.TimeRemaining <= 0f) return;

            var rectTransform = viewData.Value;
            var target = targetPositionData.Value;
            var isLocal = targetPositionData.IsLocal;

            var deltaTime = DeltaTime;
            var totalDuration = positionTimeData.Value;
            var remainingTime = positionTimeData.TimeRemaining;

            var progress = 1f - (remainingTime / totalDuration);
            progress = Mathf.Clamp01(progress + deltaTime / totalDuration);

            if (isLocal)
            {
                var current = rectTransform.localPosition;
                rectTransform.localPosition = Vector3.Lerp(current, target, progress);
            }
            else
            {
                var current = rectTransform.position;
                rectTransform.position = Vector3.Lerp(current, target, progress);
            }

            positionTimeData.TimeRemaining -= deltaTime;

            if (positionTimeData.TimeRemaining <= 0f)
            {
                if (isLocal) rectTransform.localPosition = target;
                else rectTransform.position = target;

                if (Pooler.PositionReplaceableCallback.Has(entity))
                {
                    ref var replaceableCallbackData = ref Pooler.PositionReplaceableCallback.Get(entity);
                    replaceableCallbackData.Value?.Invoke();
                    replaceableCallbackData.Value = null;
                }

                if (Pooler.PositionGuaranteedCallback.Has(entity))
                {
                    ref var guaranteedCallbackData = ref Pooler.PositionGuaranteedCallback.Get(entity);
                    guaranteedCallbackData.Value?.Invoke();
                    guaranteedCallbackData.Value = null;
                }

                if (Pooler.PositionPostProcessCallback.Has(entity))
                {
                    ref var postProcessCallbackData = ref Pooler.PositionPostProcessCallback.Get(entity);
                    foreach (var action in postProcessCallbackData.Value) action?.Invoke();
                    postProcessCallbackData.Value.Clear();
                }

                if (targetPositionData.LastTick != Time.frameCount)
                {
                    Pooler.TargetViewPosition.Del(entity);
                    Pooler.PositionTime.Del(entity);
                }
            }
        }
    }
}