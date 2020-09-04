using Fixy;
using Zenject;

public class FixyInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPlayerPosition>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IWheel>().FromComponentsInChildren().AsSingle();
    }
}
