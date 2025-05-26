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
        public EcsPackedEntity PackedEntity;
        public EcsWorld World;
        public PoolerUI PoolerUI;
        public bool IsRotationActive = true;
        public bool IsScaleActive = true;
        public bool IsDragActive = true;
        [FoldoutGroup("ECS UI")] public bool isPointActive = true;
        [FoldoutGroup("ECS UI")] public RectTransform viewRectTransform;
        [FoldoutGroup("ECS UI")] public List<string> tags = new();

        [Button, FoldoutGroup("Debug")]
        public void AddTag(string tagName)
        {
            tags.Add(tagName);
            if (World != null &&PackedEntity.Unpack(World, out var entity)) PoolerUI.UpdateTags(entity);
        }
        
        [Button, FoldoutGroup("Debug")]
        public void RemoveTag(string tagName)
        {
            tags.Remove(tagName);
            if (World != null &&PackedEntity.Unpack(World, out var entity)) PoolerUI.UpdateTags(entity);
        }
        
        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            PoolerUI = EcsUIStarter.PoolerUI;
            World = PoolerUI.World;
            PackedEntity = World.NewPackedEntity();
            
            ref var entityUIData = ref PoolerUI.EntityUI.Add(this);
            entityUIData.Value = this;
            ref var viewData = ref PoolerUI.View.Add(this);
            viewData.Value = viewRectTransform;
            ref var positionData = ref PoolerUI.StandardViewPosition.Add(this);
            positionData.Value = viewRectTransform.localPosition;
            ref var rotationData = ref PoolerUI.StandardViewRotation.Add(this);
            rotationData.Value = transform.rotation.eulerAngles;
            ref var scaleData = ref PoolerUI.StandardViewScale.Add(this);
            scaleData.Value = transform.localScale;
            if (tags.Count > 0) PoolerUI.UpdateTags(this);
        }

        protected virtual void OnDisable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (World != null && PackedEntity.Unpack(World, out var entity))
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

        public static implicit operator EcsPackedEntity(EntityUIComponent entityUI) => entityUI.PackedEntity;
        public static implicit operator int(EntityUIComponent entityUI) => entityUI.PackedEntity.Id;
        public static implicit operator PoolerUI(EntityUIComponent entityUI) => entityUI.PoolerUI;
        public static implicit operator EcsWorld(EntityUIComponent entityUI) => entityUI.World;
        public static implicit operator Transform(EntityUIComponent entityUI) => entityUI.transform;
    }
}