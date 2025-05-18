using Exerussus._1EasyEcs.Scripts.Core;
using Leopotam.EcsLite;
using UnityEngine;

namespace Plugins.Exerussus.EcsUI.Systems
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
            ref var viewData = ref Pooler.View.Get(entity);
            var rectTransform = viewData.Value;
            
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