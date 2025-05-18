using Exerussus._1EasyEcs.Scripts.Custom;
using Leopotam.EcsLite;
using Plugins.Exerussus.EcsUI.Systems;

namespace Plugins.Exerussus.EcsUI
{
    public class EcsUIGroup : EcsGroup<PoolerUI>
    {
        protected override void SetInitSystems(IEcsSystems initSystems)
        {
            
        }

        protected override void SetUpdateSystems(IEcsSystems updateSystems)
        {
            updateSystems.Add(new SizeSystem      { UpdateDelay = 0f });
            updateSystems.Add(new PositionSystem  { UpdateDelay = 0f });
            updateSystems.Add(new RotationSystem  { UpdateDelay = 0f });
            updateSystems.Add(new DraggableSystem { UpdateDelay = 0f });
            updateSystems.Add(new DestroySystem   { UpdateDelay = 0f });
#if UNITY_EDITOR
            updateSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif
        }
    }
}