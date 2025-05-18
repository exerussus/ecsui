using Exerussus._1EasyEcs.Scripts.Core;
using Exerussus._1EasyEcs.Scripts.Custom;
using Exerussus._1Extensions.SignalSystem;
using Exerussus._1Extensions.SmallFeatures;
using Leopotam.EcsLite;

namespace Exerussus.EcsUI
{
    public class EcsUIStarter : EcsStarter
    {
        public GameContext gameContext = new();
        public override Signal Signal => SignalQoL.Instance;
        protected override GameContext GetGameContext(GameShare gameShare) => gameContext;
        
        private PoolerUI _poolerUI;
        private static readonly object InstanceLock = new object();
        private static EcsUIStarter _instance;
        
        public static PoolerUI PoolerUI => Instance._poolerUI;
        public static EcsUIStarter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (InstanceLock)
                    {
                        _instance = new UnityEngine.GameObject("[EcsUIStarter]").AddComponent<EcsUIStarter>();
                        _instance.Initialize();
                        _instance._poolerUI = _instance.GameShare.GetSharedObject<PoolerUI>();
                        DontDestroyOnLoad(_instance);
                    }
                }
                return _instance;
            }
        }

        protected override void SetSharingDataOnStart(EcsWorld world, GameShare gameShare)
        {
            
        }
        
        protected override EcsGroup[] GetGroups() => new EcsGroup[] { new EcsUIGroup() };
    }
}