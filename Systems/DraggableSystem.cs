using Exerussus._1EasyEcs.Scripts.Core;
using Leopotam.EcsLite;
using UnityEngine;

namespace Exerussus.EcsUI.Systems
{
    public class DraggableSystem : EasySystem<PoolerUI>
    {
        private EcsFilter _draggableFilter;

        protected override void Initialize()
        {
            _draggableFilter = World.Filter<EcsUIData.View>().Inc<EcsUIData.DraggableProcessMark>().Exc<EcsUIData.DestroyProcess>().End();
        }

        protected override void Update()
        {
            _draggableFilter.Foreach(OnDraggableUpdate);
        }

        private void OnDraggableUpdate(int entity)
        {
            ref var entityUiData = ref Pooler.EntityUI.Get(entity);
            
            if (!entityUiData.Value.IsDragActive)
            {
                Pooler.DraggableProcessMark.Del(entity);
                return;
            }
            
            var rectTransform = entityUiData.Value.viewRectTransform;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                Input.mousePosition,
                null,
                out Vector2 localPoint
            );
            
            rectTransform.anchoredPosition = localPoint;
        }
    }
}