using Exerussus._1EasyEcs.Scripts.Core;
using Leopotam.EcsLite;

namespace Exerussus.EcsUI.Systems
{
    public class DestroySystem : EasySystem<PoolerUI>
    {
        private EcsFilter _destroyFilter;
        protected override void Initialize()
        {
            _destroyFilter = World.Filter<EcsUIData.DestroyProcess>().End();
        }

        protected override void Update()
        {
            foreach (var entity in _destroyFilter)
            {
                ref var destroyData = ref Pooler.DestroyProcess.Get(entity);
                if (destroyData.ReadyToDestroy) World.DelEntity(entity);
                else destroyData.ReadyToDestroy = true;
            }
        }
    }
}