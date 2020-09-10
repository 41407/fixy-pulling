using Zenject;

namespace Fixy
{
    public class FixyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IFixyController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IPlayerPosition>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IWheel>().FromComponentsInChildren().AsSingle();
            Container.Bind<IFork>().FromComponentsInChildren().AsSingle();
        }
    }
}
