using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Exerussus.EcsUI.Components
{
    [AddComponentMenu("ECS UI/Scaler")]
    [RequireComponent(typeof(EntityUIComponent))]
    public class ScalerComponent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public ScaleParameter highlight = new();
        public ScaleParameter click = new();

        private EntityUIComponent _entityUI;

        private void Start()
        {
            _entityUI = GetComponent<EntityUIComponent>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!highlight.enabled) return;
            if (_entityUI == null) return;
            if (!_entityUI.isPointActive) return;
            _entityUI.ScaleToTemp(highlight.scale, highlight.time);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!highlight.enabled) return;
            if (_entityUI == null) return;
            if (!_entityUI.isPointActive) return;
            _entityUI.ScaleToStandard(highlight.backTime);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!click.enabled) return;
            if (_entityUI == null) return;
            if (!_entityUI.isPointActive) return;
            
            _entityUI.ScaleToTemp(click.scale, click.time, callbackType: ProcessCallbackType.Replaceable, callback: () =>
            {
                _entityUI.ScaleToStandard(click.backTime);
            });
        }
    }

    [Serializable, Toggle("enabled")]
    public class ScaleParameter
    {
        public bool enabled = true;
        public Vector3 scale = new Vector3(1.4f, 1.4f, 1.4f);
        public float time = 0.5f;
        public float backTime = 0.2f;
    }
}