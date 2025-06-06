using System.Collections.Generic;
using Exerussus._1EasyEcs.Scripts.Extensions;
using Leopotam.EcsLite;
using LitMotion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Exerussus.EcsUI
{
    [AddComponentMenu("ECS UI/EntityUI")]
    public class EntityUIComponent : MonoBehaviour
    {
        public EcsPackedEntity PackedEntityUI;
        public PoolerUI PoolerUI;
        public bool IsRotationActive = true;
        public bool IsScaleActive = true;
        public bool IsDragActive = true;
        [FoldoutGroup("ECS UI")] public bool isPointActive = true;
        [FoldoutGroup("ECS UI")] public RectTransform viewRectTransform;
        [FoldoutGroup("ECS UI")] public List<string> tags = new();
        public EcsWorld WorldUI => PoolerUI.World;
        public int EcsEntityUI => PackedEntityUI.Id;

        [Button, FoldoutGroup("Debug")]
        public void AddTag(string tagName)
        {
            tags.Add(tagName);
            if (WorldUI != null &&PackedEntityUI.Unpack(WorldUI, out var entity)) PoolerUI.UpdateTags(entity);
        }
        
        [Button, FoldoutGroup("Debug")]
        public void RemoveTag(string tagName)
        {
            tags.Remove(tagName);
            if (WorldUI != null &&PackedEntityUI.Unpack(WorldUI, out var entity)) PoolerUI.UpdateTags(entity);
        }
        
        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            PoolerUI = EcsUIStarter.PoolerUI;
            PackedEntityUI = WorldUI.NewPackedEntity();
            
            Debug.Log($"entity UI | Init : {PackedEntityUI.Id}.");
            
            ref var entityUIData = ref PoolerUI.EntityUI.Add(EcsEntityUI);
            entityUIData.Value = this;
            entityUIData.PackedEntity = PackedEntityUI;
            ref var viewData = ref PoolerUI.View.Add(EcsEntityUI);
            viewData.Value = viewRectTransform;
            ref var positionData = ref PoolerUI.StandardViewPosition.Add(EcsEntityUI);
            positionData.Value = viewRectTransform.localPosition;
            ref var rotationData = ref PoolerUI.StandardViewRotation.Add(EcsEntityUI);
            rotationData.Value = transform.rotation.eulerAngles;
            ref var scaleData = ref PoolerUI.StandardViewScale.Add(EcsEntityUI);
            scaleData.Value = transform.localScale;
            if (tags.Count > 0) PoolerUI.UpdateTags(EcsEntityUI);
        }

        protected virtual void OnDisable()
        {
            Debug.Log($"entity UI | Deinitialize : {PackedEntityUI.Id}.");
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (WorldUI != null && PackedEntityUI.Unpack(WorldUI, out var entity))
            {
                ref var destroyData = ref PoolerUI.DestroyProcess.AddOrGet(entity);
                destroyData.ReadyToDestroy = 10;
                if (PoolerUI.LitMotionHandle.Has(entity))
                {
                    ref var handlerData = ref PoolerUI.LitMotionHandle.Get(entity);
                    foreach (var handle in handlerData.Value)
                    {
                        if (handle.IsActive()) handle.Complete();
                    }
                }
            }
        }

        public static implicit operator EcsPackedEntity(EntityUIComponent entityUI) => entityUI.PackedEntityUI;
        public static implicit operator Transform(EntityUIComponent entityUI) => entityUI.transform;
    }
}