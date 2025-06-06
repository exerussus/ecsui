using System;
using System.Collections.Generic;
using Exerussus._1EasyEcs.Scripts.Core;
using Leopotam.EcsLite;
using LitMotion;
using LitMotion.Adapters;
using UnityEngine;

namespace Exerussus.EcsUI
{
    public static class EcsUIData
    {
        public struct EntityUI                          : IEcsComponent { public EntityUIComponent Value; public EcsPackedEntity PackedEntity; }
        public struct View                              : IEcsComponent { public RectTransform Value; }
        public struct DestroyProcess                    : IEcsComponent { public byte ReadyToDestroy; }
                  
        public struct StandardViewScale                 : IEcsComponent { public Vector3 Value; }
        public struct TargetViewScale                   : IEcsComponent { public Vector3 Value; public int LastTick; }
        public struct ScaleTime                         : IEcsComponent { public float Value; public float TimeRemaining;  }
        /// <summary> Заменяется при повторном вызове процесса. </summary>
        public struct ScaleReplaceableCallback          : IEcsComponent { public Action Value;  }
        /// <summary> Срабатывает по окончанию процесса, или при повторном вызове и прерыванию предыдущего. </summary>
        public struct ScaleGuaranteedCallback           : IEcsComponent { public Action Value;  }
        /// <summary> Срабатывает по окончанию процесса. </summary>
        public struct ScalePostProcessCallback          : IEcsComponent { public List<Action> Value;  }
        public struct DraggableProcessMark              : IEcsComponent {  }
              
        public struct StandardViewPosition              : IEcsComponent { public Vector3 Value; }
        public struct TargetViewPosition                : IEcsComponent { public Vector3 Value; public bool IsLocal; public int LastTick; }
        public struct PositionTime                      : IEcsComponent { public float Value; public float TimeRemaining; }
        /// <summary> Заменяется при повторном вызове процесса. </summary>
        public struct PositionReplaceableCallback       : IEcsComponent { public Action Value;  }
        /// <summary> Срабатывает по окончанию процесса, или при повторном вызове и прерыванию предыдущего. </summary>
        public struct PositionGuaranteedCallback        : IEcsComponent { public Action Value;  }
        /// <summary> Срабатывает по окончанию процесса. </summary>
        public struct PositionPostProcessCallback       : IEcsComponent { public List<Action> Value;  }
        
        public struct StandardViewRotation              : IEcsComponent { public Vector3 Value; }
        public struct TargetViewRotation                : IEcsComponent { public Vector3 Value; public bool IsLocal; public int LastTick; }
        public struct RotationTime                      : IEcsComponent { public float Value; public float TimeRemaining;  }
        /// <summary> Заменяется при повторном вызове процесса. </summary>
        public struct RotationReplaceableCallback       : IEcsComponent { public Action Value;  }
        /// <summary> Срабатывает по окончанию процесса, или при повторном вызове и прерыванию предыдущего. </summary>
        public struct RotationGuaranteedCallback        : IEcsComponent { public Action Value;  }
        /// <summary> Срабатывает по окончанию процесса. </summary>
        public struct RotationPostProcessCallback       : IEcsComponent { public List<Action> Value;  }
        
        public struct Tags                              : IEcsComponent { public HashSet<string> Value;  }
        public struct LitMotionBuilder                  : IEcsComponent { public MotionBuilder<Vector3, NoOptions, Vector3MotionAdapter> Value;  }
        public struct LitMotionHandle                   : IEcsComponent { public List<MotionHandle> Value;  }
    }
}