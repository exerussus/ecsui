﻿using System;
using LitMotion;
using LitMotion.Adapters;
using LitMotion.Extensions;
using UnityEngine;

namespace Exerussus.EcsUI
{
    public static class EntityUIExtensions
    {
        public static void MoveTo(this EntityUIComponent entityUI, Vector3 targetPosition, float time, bool isLocalPosition = false)
        {
            entityUI.PoolerUI.MoveTo(entityUI.PackedEntityUI.Id, targetPosition, time, isLocalPosition);
        }
        
        public static void MoveTo(this EntityUIComponent entityUI, Vector3 targetPosition, float time, ProcessCallbackType callbackType, Action callback, bool isLocalPosition = false)
        {
            entityUI.PoolerUI.MoveTo(entityUI.PackedEntityUI.Id, targetPosition, time, callbackType, callback, isLocalPosition);
        }
        
        public static void MoveToTemp(this EntityUIComponent entityUI, Vector3 targetPosition, float time, bool isLocalPosition = false)
        {
            entityUI.PoolerUI.MoveToTemp(entityUI.PackedEntityUI.Id, targetPosition, time, isLocalPosition);
        }
        
        public static void MoveToTemp(this EntityUIComponent entityUI, Vector3 targetPosition, float time, ProcessCallbackType callbackType, Action callback, bool isLocalPosition = false)
        {
            entityUI.PoolerUI.MoveToTemp(entityUI.PackedEntityUI.Id, targetPosition, time, callbackType, callback, isLocalPosition);
        }

        public static void MoveToStandard(this EntityUIComponent entityUI, float time)
        {
            entityUI.PoolerUI.MoveToStandard(entityUI.PackedEntityUI.Id, time);
        }

        public static void MoveToStandard(this EntityUIComponent entityUI, float time, ProcessCallbackType callbackType, Action callback)
        {
            entityUI.PoolerUI.MoveToStandard(entityUI.PackedEntityUI.Id, time, callbackType, callback);
        }
        
        public static void ScaleTo(this EntityUIComponent entityUI, Vector3 targetScale, float time)
        {
            entityUI.PoolerUI.ScaleTo(entityUI.PackedEntityUI.Id, targetScale, time);
        }
        
        public static void ScaleTo(this EntityUIComponent entityUI, Vector3 targetScale, float time, ProcessCallbackType callbackType, Action callback)
        {
            entityUI.PoolerUI.ScaleTo(entityUI.PackedEntityUI.Id, targetScale, time, callbackType, callback);
        }
        
        public static void ScaleToTemp(this EntityUIComponent entityUI, Vector3 targetScale, float time)
        {
            entityUI.PoolerUI.ScaleToTemp(entityUI.PackedEntityUI.Id, targetScale, time);
        }
        
        public static void ScaleToTemp(this EntityUIComponent entityUI, Vector3 targetScale, float time, ProcessCallbackType callbackType, Action callback)
        {
            entityUI.PoolerUI.ScaleToTemp(entityUI.PackedEntityUI.Id, targetScale, time, callbackType, callback);
        }
        
        public static void ScaleToStandard(this EntityUIComponent entityUI, float time)
        {
            entityUI.PoolerUI.ScaleToStandard(entityUI.PackedEntityUI.Id, time);
        }
        
        public static void ScaleToStandard(this EntityUIComponent entityUI, float time, ProcessCallbackType callbackType, Action callback)
        {
            entityUI.PoolerUI.ScaleToStandard(entityUI.PackedEntityUI.Id, time, callbackType, callback);
        }
        
        public static void RotateToTemp(this EntityUIComponent entityUI, Vector3 targetRotation, float time, bool isLocalRotation = false)
        {
            entityUI.PoolerUI.RotateToTemp(entityUI.PackedEntityUI.Id, targetRotation, time, isLocalRotation);
        }
        
        public static void RotateToTemp(this EntityUIComponent entityUI, Vector3 targetRotation, float time, ProcessCallbackType callbackType, Action callback, bool isLocalRotation = false)
        {
            entityUI.PoolerUI.RotateToTemp(entityUI.PackedEntityUI.Id, targetRotation, time, callbackType, callback, isLocalRotation);
        }
        
        public static void RotateTo(this EntityUIComponent entityUI, Vector3 targetRotation, float time, bool isLocalRotation = false)
        {
            entityUI.PoolerUI.RotateTo(entityUI.PackedEntityUI.Id, targetRotation, time, isLocalRotation);
        }
        
        public static void RotateTo(this EntityUIComponent entityUI, Vector3 targetRotation, float time, ProcessCallbackType callbackType, Action callback, bool isLocalRotation = false)
        {
            entityUI.PoolerUI.RotateTo(entityUI.PackedEntityUI.Id, targetRotation, time, callbackType, callback, isLocalRotation);
        }
        
        public static void RotateToStandard(this EntityUIComponent entityUI, float time)
        {
            entityUI.PoolerUI.RotateToStandard(entityUI.PackedEntityUI.Id, time);
        }
        
        public static void RotateToStandard(this EntityUIComponent entityUI, float time, ProcessCallbackType callbackType, Action callback)
        {
            entityUI.PoolerUI.RotateToStandard(entityUI.PackedEntityUI.Id, time, callbackType, callback);
        }
    }
}