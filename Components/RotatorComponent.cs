using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Exerussus.EcsUI.Components
{
    [AddComponentMenu("ECS UI/Rotator")]
    [RequireComponent(typeof(EntityUIComponent))]
    public class RotatorComponent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public RotateParameter highlight = new();
        public RotateParameter click = new();
        
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
            _entityUI.RotateToTemp(highlight.rotation, highlight.time, highlight.isLocalRotation);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!highlight.enabled) return;
            if (_entityUI == null) return;
            if (!_entityUI.isPointActive) return;
            _entityUI.RotateToStandard(highlight.backTime);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!click.enabled) return;
            if (_entityUI == null) return;
            if (!_entityUI.isPointActive) return;
            
            _entityUI.RotateToTemp(click.rotation, click.time, isLocalRotation: click.isLocalRotation, callbackType: ProcessCallbackType.Replaceable, callback: () =>
            {
                _entityUI.RotateToStandard(click.backTime);
            });
        }
    }

    [Serializable, Toggle("enabled")]
    public class RotateParameter
    {
        public bool enabled = true;
        public bool isLocalRotation = true;
        public Vector3 rotation = Vector3.one;
        public float time = 0.5f;
        public float backTime = 0.2f;
    }
}