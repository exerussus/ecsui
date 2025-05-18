using System;
using System.Collections.Generic;
using Exerussus._1EasyEcs.Scripts.Core;
using Exerussus._1EasyEcs.Scripts.Custom;
using Leopotam.EcsLite;
using UnityEngine;

namespace Plugins.Exerussus.EcsUI
{
    public class PoolerUI : IGroupPooler
    {
        public void Initialize(EcsWorld world)
        {
            EntityUI = new(world);
            View = new(world);
            Tags = new(world);
            DestroyProcess = new(world);
            
            StandardViewRotation = new(world);
            TargetViewRotation = new(world);
            RotationTime = new(world);
            ScaleReplaceableCallback = new(world);
            ScaleGuaranteedCallback = new(world);
            ScalePostProcessCallback = new(world);
            
            StandardViewPosition = new(world);
            TargetViewPosition = new(world);
            PositionTime = new(world);
            PositionReplaceableCallback = new(world);
            PositionGuaranteedCallback = new(world);
            PositionPostProcessCallback = new(world);
            DraggableProcessMark = new(world);
            
            StandardViewScale = new(world);
            TargetViewScale = new(world);
            ScaleTime = new(world);
            RotationReplaceableCallback = new(world);
            RotationGuaranteedCallback = new(world);
            RotationPostProcessCallback = new(world);

            _tagFilter = World.Filter<EcsUIData.View>().Inc<EcsUIData.Tags>().Exc<EcsUIData.DestroyProcess>().End();
            _moveTagFilter = World.Filter<EcsUIData.View>().Inc<EcsUIData.StandardViewPosition>().Inc<EcsUIData.Tags>().Exc<EcsUIData.DestroyProcess>().End();
            _scaleTagFilter = World.Filter<EcsUIData.View>().Inc<EcsUIData.StandardViewPosition>().Inc<EcsUIData.Tags>().Exc<EcsUIData.DestroyProcess>().End();
        }
        
        [InjectSharedObject] public EcsWorld World { get; private set; }
        public PoolerModule<EcsUIData.EntityUI> EntityUI { get; private set; }
        public PoolerModule<EcsUIData.View> View { get; private set; }
        public PoolerModule<EcsUIData.DestroyProcess> DestroyProcess { get; private set; }
        
        public PoolerModule<EcsUIData.StandardViewRotation> StandardViewRotation { get; private set; }
        public PoolerModule<EcsUIData.TargetViewRotation> TargetViewRotation { get; private set; }
        public PoolerModule<EcsUIData.RotationTime> RotationTime { get; private set; }
        public PoolerModule<EcsUIData.ScaleReplaceableCallback> ScaleReplaceableCallback { get; private set; }
        public PoolerModule<EcsUIData.ScaleGuaranteedCallback> ScaleGuaranteedCallback { get; private set; }
        public PoolerModule<EcsUIData.ScalePostProcessCallback> ScalePostProcessCallback { get; private set; }
        
        public PoolerModule<EcsUIData.StandardViewPosition> StandardViewPosition { get; private set; }
        public PoolerModule<EcsUIData.TargetViewPosition> TargetViewPosition { get; private set; }
        public PoolerModule<EcsUIData.PositionTime> PositionTime { get; private set; }
        public PoolerModule<EcsUIData.PositionReplaceableCallback> PositionReplaceableCallback { get; private set; }
        public PoolerModule<EcsUIData.PositionGuaranteedCallback> PositionGuaranteedCallback { get; private set; }
        public PoolerModule<EcsUIData.PositionPostProcessCallback> PositionPostProcessCallback { get; private set; }
        public PoolerModule<EcsUIData.DraggableProcessMark> DraggableProcessMark { get; private set; }
        
        public PoolerModule<EcsUIData.StandardViewScale> StandardViewScale { get; private set; }
        public PoolerModule<EcsUIData.TargetViewScale> TargetViewScale { get; private set; }
        public PoolerModule<EcsUIData.ScaleTime> ScaleTime { get; private set; }
        public PoolerModule<EcsUIData.RotationReplaceableCallback> RotationReplaceableCallback { get; private set; }
        public PoolerModule<EcsUIData.RotationGuaranteedCallback> RotationGuaranteedCallback { get; private set; }
        public PoolerModule<EcsUIData.RotationPostProcessCallback> RotationPostProcessCallback { get; private set; }
        
        public PoolerModule<EcsUIData.Tags> Tags { get; private set; }

        private EcsFilter _tagFilter;
        private EcsFilter _moveTagFilter;
        private EcsFilter _scaleTagFilter;

        /// <summary> Обновляет тэги сущности. </summary>
        public void UpdateTags(int entity)
        {
            ref var entityUI = ref EntityUI.Get(entity);
            if (entityUI.Value.tags.Count > 0)
            {
                ref var tags = ref Tags.AddOrGet(entity);
                if (tags.Value == null) tags.Value = new();
                tags.Value.Clear();
                foreach (var tag in entityUI.Value.tags) tags.Value.Add(tag);
                entityUI.Value.tags.Clear();
                entityUI.Value.tags.AddRange(tags.Value);
            }
            else Tags.Del(entity);
        }
        
        /// <summary> Находит все сущности с указанным тэгом. </summary>
        public bool GetMass(string tag, out List<EntityUIComponent> foundComponents)
        {
            foundComponents = new List<EntityUIComponent>();

            foreach (var entity in _tagFilter)
            {
                ref var tags = ref Tags.Get(entity);
                if (!tags.Value.Contains(tag)) continue;
                ref var entityUI = ref EntityUI.Get(entity);
                foundComponents.Add(entityUI.Value);
            }
            
            return foundComponents.Count > 0;
        }
        
        /// <summary> Находит все сущности с указанным тэгом. </summary>
        public bool GetMass(string tag, List<EntityUIComponent> foundComponents)
        {
            foundComponents.Clear();

            foreach (var entity in _tagFilter)
            {
                ref var tags = ref Tags.Get(entity);
                if (!tags.Value.Contains(tag)) continue;
                ref var entityUI = ref EntityUI.Get(entity);
                foundComponents.Add(entityUI.Value);
            }
            
            return foundComponents.Count > 0;
        }
        
        /// <summary> Находит все сущности с указанным тэгом и перемещает их в позицию. </summary>
        public void MoveMassTo(string tag, Vector3 targetPosition, float time, bool isLocalPosition = false)
        {
            foreach (var entity in _moveTagFilter)
            {
                ref var tags = ref Tags.Get(entity);
                if (tags.Value.Contains(tag)) MoveTo(entity, targetPosition, time, isLocalPosition);
            }
        }
        
        public void ScaleMassTo(string tag, Vector3 targetScale, float time)
        {
            foreach (var entity in _scaleTagFilter)
            {
                ref var tags = ref Tags.Get(entity);
                if (tags.Value.Contains(tag)) ScaleTo(entity, targetScale, time);
            }
        }
        
        public void ScaleMassToTemp(string tag, Vector3 targetScale, float time)
        {
            foreach (var entity in _scaleTagFilter)
            {
                ref var tags = ref Tags.Get(entity);
                if (tags.Value.Contains(tag)) ScaleToTemp(entity, targetScale, time);
            }
        }
        
        public void ScaleMassTo(string tag, Vector3 targetScale, ProcessCallbackType processCallback, Action<EntityUIComponent> callback, float time)
        {
            foreach (var entity in _scaleTagFilter)
            {
                ref var tags = ref Tags.Get(entity);
                if (tags.Value.Contains(tag)) ScaleTo(entity, targetScale, time, processCallback, (() =>
                {
                    ref var entityUI = ref EntityUI.Get(entity);
                    callback?.Invoke(entityUI.Value);
                }));
            }
        }
        
        public void ScaleMassToTemp(string tag, Vector3 targetScale, float time, ProcessCallbackType processCallback, Action<EntityUIComponent> callback)
        {
            foreach (var entity in _scaleTagFilter)
            {
                ref var tags = ref Tags.Get(entity);
                if (tags.Value.Contains(tag)) ScaleToTemp(entity, targetScale, time, processCallback, () =>
                {
                    ref var entityUI = ref EntityUI.Get(entity);
                    callback?.Invoke(entityUI.Value);
                });
            }
        }
        
        /// <summary> Находит все сущности с указанным тэгом и перемещает их в позицию. </summary>
        public void MoveMassTo(string tag, Vector3 targetPosition, float time, ProcessCallbackType processCallback, Action<EntityUIComponent> callback, bool isLocalPosition = false)
        {
            foreach (var entity in _moveTagFilter)
            {
                ref var tags = ref Tags.Get(entity);
                if (tags.Value.Contains(tag)) MoveTo(entity, targetPosition, time, processCallback, () =>
                {
                    ref var entityUI = ref EntityUI.Get(entity);
                    callback?.Invoke(entityUI.Value);
                }, isLocalPosition);
            }
        }
        
        /// <summary> Находит все сущности с указанным тэгом и перемещает их в позицию. </summary>
        public void MoveMassToTemp(string tag, Vector3 targetPosition, float time, ProcessCallbackType processCallback, Action<EntityUIComponent> callback, bool isLocalPosition = false)
        {
            foreach (var entity in _moveTagFilter)
            {
                ref var tags = ref Tags.Get(entity);
                if (tags.Value.Contains(tag)) MoveToTemp(entity, targetPosition, time, processCallback, () =>
                {
                    ref var entityUI = ref EntityUI.Get(entity);
                    callback?.Invoke(entityUI.Value);
                }, isLocalPosition);
            }
        }

        /// <summary> Находит все сущности с указанной датой и перемещает их в позицию. </summary>
        public void MoveMassTo<T>(Vector3 targetPosition, float time, bool isLocalPosition = false) where T : struct, IEcsComponent
        {
            var filter = World.Filter<EcsUIData.View>().Inc<T>().Inc<EcsUIData.StandardViewPosition>().End();
            foreach (var entity in filter) MoveTo(entity, targetPosition, time, isLocalPosition);
        }

        /// <summary> Находит все сущности с указанным тэгом и перемещает их в стандартную позицию.  </summary>
        public void MoveMassToStandard(string tag, float time)
        {
            foreach (var entity in _moveTagFilter)
            {
                ref var tags = ref Tags.Get(entity);
                if (tags.Value.Contains(tag)) MoveToStandard(entity, time);
            }
        }

        /// <summary> Находит все сущности с указанной датой и перемещает их в стандартную позицию. </summary>
        public void MoveMassToStandard<T>(float time) where T : struct, IEcsComponent
        {
            var filter = World.Filter<EcsUIData.View>().Inc<T>().Inc<EcsUIData.StandardViewPosition>().End();
            foreach (var entity in filter) MoveToStandard(entity, time);
        }
        
        /// <summary> Двигает сущность в позицию без перезаписи стандартной позиции. </summary>
        public void MoveToTemp(int entity, Vector3 targetPosition, float time, bool isLocalPosition = false)
        {
            ref var targetPositionData = ref TargetViewPosition.AddOrGet(entity);
            targetPositionData.Value = targetPosition;
            targetPositionData.IsLocal = isLocalPosition;
            targetPositionData.LastTick = Time.frameCount;
            ref var timeData = ref PositionTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
        }
        
        /// <summary> Двигает сущность в позицию без перезаписи стандартной позиции. </summary>
        public void MoveToTemp(int entity, Vector3 targetPosition, float time, ProcessCallbackType callbackType, Action callback, bool isLocalPosition = false)
        {
            ref var targetPositionData = ref TargetViewPosition.AddOrGet(entity);
            targetPositionData.Value = targetPosition;
            targetPositionData.IsLocal = isLocalPosition;
            targetPositionData.LastTick = Time.frameCount;
            ref var timeData = ref PositionTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
            
            switch (callbackType)
            {
                case ProcessCallbackType.Replaceable:
                    ref var replaceableCallbackData = ref PositionReplaceableCallback.AddOrGet(entity);
                    replaceableCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.Guaranteed:
                    if (PositionGuaranteedCallback.Has(entity))
                    {
                        ref var gCallbackData = ref PositionGuaranteedCallback.Get(entity);
                        gCallbackData.Value?.Invoke();
                    }
                    ref var guaranteedCallbackData = ref PositionGuaranteedCallback.AddOrGet(entity);
                    guaranteedCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.PostProcess:
                    if (!PositionPostProcessCallback.Has(entity))
                    {
                        ref var ppCallbackData = ref PositionPostProcessCallback.Add(entity);
                        ppCallbackData.Value = new();
                    }
                    
                    ref var positionPostProcessCallbackData = ref PositionPostProcessCallback.Get(entity);
                    positionPostProcessCallbackData.Value.Add(callback);
                    break;
            }
        }
        
        /// <summary> Двигает сущность в позицию и устанавливает ее как стандартную. </summary>
        public void MoveTo(int entity, Vector3 targetPosition, float time, bool isLocalPosition = false)
        {
            ref var standardPositionData = ref StandardViewPosition.Get(entity);
            standardPositionData.Value = targetPosition;
            ref var targetPositionData = ref TargetViewPosition.AddOrGet(entity);
            targetPositionData.Value = targetPosition;
            targetPositionData.IsLocal = isLocalPosition;
            targetPositionData.LastTick = Time.frameCount;
            ref var timeData = ref PositionTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
        }
        
        /// <summary> Двигает сущность в позицию и устанавливает ее как стандартную. </summary>
        public void MoveTo(int entity, Vector3 targetPosition, float time, ProcessCallbackType callbackType, Action callback, bool isLocalPosition = false)
        {
            ref var standardPositionData = ref StandardViewPosition.Get(entity);
            standardPositionData.Value = targetPosition;
            ref var targetPositionData = ref TargetViewPosition.AddOrGet(entity);
            targetPositionData.Value = targetPosition;
            targetPositionData.LastTick = Time.frameCount;
            targetPositionData.IsLocal = isLocalPosition;
            ref var timeData = ref PositionTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
            
            switch (callbackType)
            {
                case ProcessCallbackType.Replaceable:
                    ref var replaceableCallbackData = ref PositionReplaceableCallback.AddOrGet(entity);
                    replaceableCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.Guaranteed:
                    if (PositionGuaranteedCallback.Has(entity))
                    {
                        ref var gCallbackData = ref PositionGuaranteedCallback.Get(entity);
                        gCallbackData.Value?.Invoke();
                    }
                    ref var guaranteedCallbackData = ref PositionGuaranteedCallback.AddOrGet(entity);
                    guaranteedCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.PostProcess:
                    if (!PositionPostProcessCallback.Has(entity))
                    {
                        ref var ppCallbackData = ref PositionPostProcessCallback.Add(entity);
                        ppCallbackData.Value = new();
                    }
                    
                    ref var positionPostProcessCallbackData = ref PositionPostProcessCallback.Get(entity);
                    positionPostProcessCallbackData.Value.Add(callback);
                    break;
            }
        }

        public void MoveToStandard(int entity, float time)
        {
            ref var standardPositionData = ref StandardViewPosition.Get(entity);
            ref var targetPositionData = ref TargetViewPosition.AddOrGet(entity);
            targetPositionData.Value = standardPositionData.Value;
            targetPositionData.LastTick = Time.frameCount;
            ref var timeData = ref PositionTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
        }

        public void MoveToStandard(int entity, float time, ProcessCallbackType callbackType, Action callback)
        {
            ref var standardPositionData = ref StandardViewPosition.Get(entity);
            ref var targetPositionData = ref TargetViewPosition.AddOrGet(entity);
            targetPositionData.Value = standardPositionData.Value;
            targetPositionData.LastTick = Time.frameCount;
            ref var timeData = ref PositionTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
            
            switch (callbackType)
            {
                case ProcessCallbackType.Replaceable:
                    ref var replaceableCallbackData = ref PositionReplaceableCallback.AddOrGet(entity);
                    replaceableCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.Guaranteed:
                    if (PositionGuaranteedCallback.Has(entity))
                    {
                        ref var gCallbackData = ref PositionGuaranteedCallback.Get(entity);
                        gCallbackData.Value?.Invoke();
                    }
                    ref var guaranteedCallbackData = ref PositionGuaranteedCallback.AddOrGet(entity);
                    guaranteedCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.PostProcess:
                    if (!PositionPostProcessCallback.Has(entity))
                    {
                        ref var ppCallbackData = ref PositionPostProcessCallback.Add(entity);
                        ppCallbackData.Value = new();
                    }
                    
                    ref var positionPostProcessCallbackData = ref PositionPostProcessCallback.Get(entity);
                    positionPostProcessCallbackData.Value.Add(callback);
                    break;
            }
        }
        
        public void RotateToTemp(int entity, Vector3 rotation, float time, bool isLocalRotation = false)
        {
            ref var targetRotationData = ref TargetViewRotation.AddOrGet(entity);
            targetRotationData.Value = rotation;
            targetRotationData.IsLocal = isLocalRotation;
            targetRotationData.LastTick = Time.frameCount;
            ref var timeData = ref RotationTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
        }
        
        public void RotateToTemp(int entity, Vector3 rotation, float time, ProcessCallbackType callbackType, Action callback, bool isLocalRotation = false)
        {
            ref var targetRotationData = ref TargetViewRotation.AddOrGet(entity);
            targetRotationData.Value = rotation;
            targetRotationData.IsLocal = isLocalRotation;
            targetRotationData.LastTick = Time.frameCount;
            ref var timeData = ref RotationTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
            
            switch (callbackType)
            {
                case ProcessCallbackType.Replaceable:
                    ref var replaceableCallbackData = ref RotationReplaceableCallback.AddOrGet(entity);
                    replaceableCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.Guaranteed:
                    if (RotationGuaranteedCallback.Has(entity))
                    {
                        ref var gCallbackData = ref RotationGuaranteedCallback.Get(entity);
                        gCallbackData.Value?.Invoke();
                    }
                    ref var guaranteedCallbackData = ref RotationGuaranteedCallback.AddOrGet(entity);
                    guaranteedCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.PostProcess:
                    if (!RotationPostProcessCallback.Has(entity))
                    {
                        ref var ppCallbackData = ref RotationPostProcessCallback.Add(entity);
                        ppCallbackData.Value = new();
                    }
                    
                    ref var postProcessCallbackData = ref RotationPostProcessCallback.Get(entity);
                    postProcessCallbackData.Value.Add(callback);
                    break;
            }
        }
        
        public void RotateTo(int entity, Vector3 rotation, float time, bool isLocalRotation = false)
        {
            ref var standardViewRotationData = ref StandardViewRotation.Get(entity);
            standardViewRotationData.Value = rotation;
            ref var targetRotationData = ref TargetViewRotation.AddOrGet(entity);
            targetRotationData.Value = rotation;
            targetRotationData.IsLocal = isLocalRotation;
            targetRotationData.LastTick = Time.frameCount;
            ref var timeData = ref RotationTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
        }
        
        public void RotateTo(int entity, Vector3 rotation, float time, ProcessCallbackType callbackType, Action callback, bool isLocalRotation = false)
        {
            ref var standardViewRotationData = ref StandardViewRotation.Get(entity);
            standardViewRotationData.Value = rotation;
            ref var targetRotationData = ref TargetViewRotation.AddOrGet(entity);
            targetRotationData.Value = rotation;
            targetRotationData.IsLocal = isLocalRotation;
            targetRotationData.LastTick = Time.frameCount;
            ref var timeData = ref RotationTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
            
            switch (callbackType)
            {
                case ProcessCallbackType.Replaceable:
                    ref var replaceableCallbackData = ref RotationReplaceableCallback.AddOrGet(entity);
                    replaceableCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.Guaranteed:
                    if (RotationGuaranteedCallback.Has(entity))
                    {
                        ref var gCallbackData = ref RotationGuaranteedCallback.Get(entity);
                        gCallbackData.Value?.Invoke();
                    }
                    ref var guaranteedCallbackData = ref RotationGuaranteedCallback.AddOrGet(entity);
                    guaranteedCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.PostProcess:
                    if (!RotationPostProcessCallback.Has(entity))
                    {
                        ref var ppCallbackData = ref RotationPostProcessCallback.Add(entity);
                        ppCallbackData.Value = new();
                    }
                    
                    ref var postProcessCallbackData = ref RotationPostProcessCallback.Get(entity);
                    postProcessCallbackData.Value.Add(callback);
                    break;
            }
        }
        
        public void RotateToStandard(int entity, float time)
        {
            ref var standardViewRotationData = ref StandardViewRotation.Get(entity);
            ref var targetPositionData = ref TargetViewRotation.AddOrGet(entity);
            targetPositionData.Value = standardViewRotationData.Value;
            targetPositionData.LastTick = Time.frameCount;
            ref var timeData = ref RotationTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
        }
        
        public void RotateToStandard(int entity, float time, ProcessCallbackType callbackType, Action callback)
        {
            ref var standardViewRotationData = ref StandardViewRotation.Get(entity);
            ref var targetPositionData = ref TargetViewRotation.AddOrGet(entity);
            targetPositionData.Value = standardViewRotationData.Value;
            targetPositionData.LastTick = Time.frameCount;
            ref var timeData = ref RotationTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
            
            switch (callbackType)
            {
                case ProcessCallbackType.Replaceable:
                    ref var replaceableCallbackData = ref RotationReplaceableCallback.AddOrGet(entity);
                    replaceableCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.Guaranteed:
                    if (RotationGuaranteedCallback.Has(entity))
                    {
                        ref var gCallbackData = ref RotationGuaranteedCallback.Get(entity);
                        gCallbackData.Value?.Invoke();
                    }
                    ref var guaranteedCallbackData = ref RotationGuaranteedCallback.AddOrGet(entity);
                    guaranteedCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.PostProcess:
                    if (!RotationPostProcessCallback.Has(entity))
                    {
                        ref var ppCallbackData = ref RotationPostProcessCallback.Add(entity);
                        ppCallbackData.Value = new();
                    }
                    
                    ref var postProcessCallbackData = ref RotationPostProcessCallback.Get(entity);
                    postProcessCallbackData.Value.Add(callback);
                    break;
            }
        }
        
        public void ScaleToTemp(int entity, Vector3 scale, float time)
        {
            ref var targetScaleData = ref TargetViewScale.AddOrGet(entity);
            targetScaleData.Value = scale;
            targetScaleData.LastTick = Time.frameCount;
            ref var timeData = ref ScaleTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
        }
        
        public void ScaleToTemp(int entity, Vector3 scale, float time, ProcessCallbackType callbackType, Action callback)
        {
            ref var targetScaleData = ref TargetViewScale.AddOrGet(entity);
            targetScaleData.Value = scale;
            targetScaleData.LastTick = Time.frameCount;
            ref var timeData = ref ScaleTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
            
            switch (callbackType)
            {
                case ProcessCallbackType.Replaceable:
                    ref var replaceableCallbackData = ref ScaleReplaceableCallback.AddOrGet(entity);
                    replaceableCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.Guaranteed:
                    if (ScaleGuaranteedCallback.Has(entity))
                    {
                        ref var gCallbackData = ref ScaleGuaranteedCallback.Get(entity);
                        gCallbackData.Value?.Invoke();
                    }
                    ref var guaranteedCallbackData = ref ScaleGuaranteedCallback.AddOrGet(entity);
                    guaranteedCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.PostProcess:
                    if (!ScalePostProcessCallback.Has(entity))
                    {
                        ref var ppCallbackData = ref ScalePostProcessCallback.Add(entity);
                        ppCallbackData.Value = new();
                    }
                    
                    ref var postProcessCallbackData = ref ScalePostProcessCallback.Get(entity);
                    postProcessCallbackData.Value.Add(callback);
                    break;
            }
        }
        
        public void ScaleTo(int entity, Vector3 scale, float time)
        {
            ref var standardViewScaleData = ref StandardViewScale.Get(entity);
            standardViewScaleData.Value = scale;
            ref var targetPositionData = ref TargetViewScale.AddOrGet(entity);
            targetPositionData.Value = scale;
            targetPositionData.LastTick = Time.frameCount;
            ref var timeData = ref ScaleTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
        }
        
        public void ScaleTo(int entity, Vector3 scale, float time, ProcessCallbackType callbackType, Action callback)
        {
            ref var standardViewScaleData = ref StandardViewScale.Get(entity);
            standardViewScaleData.Value = scale;
            ref var targetPositionData = ref TargetViewScale.AddOrGet(entity);
            targetPositionData.Value = scale;
            targetPositionData.LastTick = Time.frameCount;
            ref var timeData = ref ScaleTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
            
            switch (callbackType)
            {
                case ProcessCallbackType.Replaceable:
                    ref var replaceableCallbackData = ref ScaleReplaceableCallback.AddOrGet(entity);
                    replaceableCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.Guaranteed:
                    if (ScaleGuaranteedCallback.Has(entity))
                    {
                        ref var gCallbackData = ref ScaleGuaranteedCallback.Get(entity);
                        gCallbackData.Value?.Invoke();
                    }
                    ref var guaranteedCallbackData = ref ScaleGuaranteedCallback.AddOrGet(entity);
                    guaranteedCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.PostProcess:
                    if (!ScalePostProcessCallback.Has(entity))
                    {
                        ref var ppCallbackData = ref ScalePostProcessCallback.Add(entity);
                        ppCallbackData.Value = new();
                    }
                    
                    ref var postProcessCallbackData = ref ScalePostProcessCallback.Get(entity);
                    postProcessCallbackData.Value.Add(callback);
                    break;
            }
        }
        
        public void ScaleToStandard(int entity, float time)
        {
            ref var standardViewScaleData = ref StandardViewScale.Get(entity);
            ref var targetPositionData = ref TargetViewScale.AddOrGet(entity);
            targetPositionData.Value = standardViewScaleData.Value;
            targetPositionData.LastTick = Time.frameCount;
            ref var timeData = ref ScaleTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
        }
        
        public void ScaleToStandard(int entity, float time, ProcessCallbackType callbackType, Action callback)
        {
            ref var standardViewScaleData = ref StandardViewScale.Get(entity);
            ref var targetPositionData = ref TargetViewScale.AddOrGet(entity);
            targetPositionData.Value = standardViewScaleData.Value;
            targetPositionData.LastTick = Time.frameCount;
            ref var timeData = ref ScaleTime.AddOrGet(entity);
            timeData.Value = time;
            timeData.TimeRemaining = time;
            
            switch (callbackType)
            {
                case ProcessCallbackType.Replaceable:
                    ref var replaceableCallbackData = ref ScaleReplaceableCallback.AddOrGet(entity);
                    replaceableCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.Guaranteed:
                    if (ScaleGuaranteedCallback.Has(entity))
                    {
                        ref var gCallbackData = ref ScaleGuaranteedCallback.Get(entity);
                        gCallbackData.Value?.Invoke();
                    }
                    ref var guaranteedCallbackData = ref ScaleGuaranteedCallback.AddOrGet(entity);
                    guaranteedCallbackData.Value = callback;
                    break;
                case ProcessCallbackType.PostProcess:
                    if (!ScalePostProcessCallback.Has(entity))
                    {
                        ref var ppCallbackData = ref ScalePostProcessCallback.Add(entity);
                        ppCallbackData.Value = new();
                    }
                    
                    ref var postProcessCallbackData = ref ScalePostProcessCallback.Get(entity);
                    postProcessCallbackData.Value.Add(callback);
                    break;
            }
        }
    }
}