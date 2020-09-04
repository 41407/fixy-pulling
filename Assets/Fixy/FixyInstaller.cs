using System.Collections;
using System.Collections.Generic;
using Fixy;
using UnityEngine;
using Zenject;

public class FixyInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IWheel>().FromComponentsInChildren().AsSingle();
    }
}
