using UnityEngine;
using UnityEngine.EventSystems;

namespace Exerussus.EcsUI.Components
{
    [AddComponentMenu("ECS UI/Draggable")]
    [RequireComponent(typeof(EntityUIComponent))]
    public class DraggableComponent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private EntityUIComponent _entityUI;

        private void Start()
        {
            _entityUI = GetComponent<EntityUIComponent>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_entityUI == null) return;
            if (!_entityUI.isPointActive) return;
            _entityUI.PoolerUI.DraggableProcessMark.AddOrGet(_entityUI.EcsEntityUI);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_entityUI == null) return;
            if (!_entityUI.isPointActive) return;
            _entityUI.PoolerUI.DraggableProcessMark.Del(_entityUI.EcsEntityUI);
        }
    }
}