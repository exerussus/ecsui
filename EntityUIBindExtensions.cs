using LitMotion;
using LitMotion.Adapters;
using LitMotion.Extensions;
using UnityEngine;

namespace Exerussus.EcsUI
{
    public static class EntityUIBindExtensions
    {
        public static MotionHandle BindEntityToPosition(this MotionBuilder<Vector3, NoOptions, Vector3MotionAdapter> motionBuilder, EntityUIComponent entityUI)
        {
            ref var litMotionHandle = ref entityUI.PoolerUI.LitMotionHandle.AddOrGet(entityUI);
            var motionHandle = motionBuilder.BindToPosition(entityUI);
            litMotionHandle.Value.Add(motionHandle);
            return motionHandle;
        }
        
        public static MotionHandle BindEntityToLocalPosition(this MotionBuilder<Vector3, NoOptions, Vector3MotionAdapter> motionBuilder, EntityUIComponent entityUI)
        {
            ref var litMotionHandle = ref entityUI.PoolerUI.LitMotionHandle.AddOrGet(entityUI);
            var motionHandle = motionBuilder.BindToLocalPosition(entityUI);
            litMotionHandle.Value.Add(motionHandle);
            return motionHandle;
        }
        
        public static MotionHandle BindEntityToLocalScale(this MotionBuilder<Vector3, NoOptions, Vector3MotionAdapter> motionBuilder, EntityUIComponent entityUI)
        {
            ref var litMotionHandle = ref entityUI.PoolerUI.LitMotionHandle.AddOrGet(entityUI);
            var motionHandle = motionBuilder.BindToLocalScale(entityUI);
            litMotionHandle.Value.Add(motionHandle);
            return motionHandle;
        }
        
        public static MotionHandle BindEntityToLocalEulerAngles(this MotionBuilder<Vector3, NoOptions, Vector3MotionAdapter> motionBuilder, EntityUIComponent entityUI)
        {
            ref var litMotionHandle = ref entityUI.PoolerUI.LitMotionHandle.AddOrGet(entityUI);
            var motionHandle = motionBuilder.BindToLocalEulerAngles(entityUI);
            litMotionHandle.Value.Add(motionHandle);
            return motionHandle;
        }
        
        public static MotionHandle BindEntityToEulerAngles(this MotionBuilder<Vector3, NoOptions, Vector3MotionAdapter> motionBuilder, EntityUIComponent entityUI)
        {
            ref var litMotionHandle = ref entityUI.PoolerUI.LitMotionHandle.AddOrGet(entityUI);
            var motionHandle = motionBuilder.BindToEulerAngles(entityUI);
            litMotionHandle.Value.Add(motionHandle);
            return motionHandle;
        }
    }
}