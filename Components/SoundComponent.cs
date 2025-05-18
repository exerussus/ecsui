using System;
using Exerussus._1Extensions.SmallFeatures;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Exerussus.EcsUI.Components
{
    [AddComponentMenu("ECS UI/Rotator")]
    [RequireComponent(typeof(EntityUIComponent))]
    public class SoundComponent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public SoundSettings click = new();
        public SoundSettings onStartHighlight = new();
        public SoundSettings onEndHighlight = new();

        private EntityUIComponent _entityUI;

        private void Start()
        {
            _entityUI = GetComponent<EntityUIComponent>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_entityUI == null) return;
            if (!click.enabled) return;
            if (!_entityUI.isPointActive) return;
            SignalQoL.Instance.RegistryRaise(new EcsUISignal.OnAudioClipPlay
            {
                Clip = click.clip
            });
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_entityUI == null) return;
            if (!onStartHighlight.enabled) return;
            if (!_entityUI.isPointActive) return;
            SignalQoL.Instance.RegistryRaise(new EcsUISignal.OnAudioClipPlay
            {
                Clip = onStartHighlight.clip
            });
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_entityUI == null) return;
            if (!onEndHighlight.enabled) return;
            if (!_entityUI.isPointActive) return;
            SignalQoL.Instance.RegistryRaise(new EcsUISignal.OnAudioClipPlay
            {
                Clip = onEndHighlight.clip
            });
        }
    }

    [Serializable, Toggle("enabled")]
    public class SoundSettings
    {
        public bool enabled;
        public AudioClip clip;
    }
}