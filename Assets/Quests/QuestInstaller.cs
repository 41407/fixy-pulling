using Quests;
using Quests.Location;
using UnityEngine;
using Zenject;

public class QuestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ILocation>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<IQuestJournal>().To<QuestJournal>().AsSingle();
        Container.Bind<IQuestView>().FromComponentInChildren().AsTransient();
        Container.Bind<QuestLocations>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
    }
}
