using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.AI.FollowStrategy
{
    class AIFollowInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallAI();
            InstallComponents();
        }

        private void InstallComponents()
        {

        }

        private void InstallAI()
        {
            Container.BindInterfacesAndSelfTo<AIFollowInput>().AsSingle();
            Container.BindInterfacesAndSelfTo<TargetClosestPlayer>().AsSingle();

        }
    }
}
