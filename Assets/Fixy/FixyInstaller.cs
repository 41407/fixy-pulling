using Zenject;

namespace Fixy
{
    public class FixyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IFixyController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IPlayerPosition>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IWheel>().FromComponentsInHierarchy().AsTransient();
            Container.Bind<IWheel>().FromComponentSibling().AsTransient().WhenInjectedInto<IRearWheel>();
            Container.Bind<IRearWheel>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IDrivetrain>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IFork>().FromComponentsInChildren().AsSingle();
        }
    }
}
