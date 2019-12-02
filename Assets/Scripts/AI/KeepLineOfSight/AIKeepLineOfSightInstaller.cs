using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.AI.KeepLineOfSight
{
    class AIKeepLineOfSightInstaller : MonoInstaller
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
            Container.BindInterfacesAndSelfTo<AITargetInSight>().AsSingle();
            Container.BindInterfacesAndSelfTo<AIKeepLineOfSightInput>().AsSingle();

        }
    }
}
