using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class ProductionInstaller : MonoInstaller
{
    [SerializeField] private ClientPool clientPool;
    [SerializeField] private ClientSystem clientSystem;
    [SerializeField] private ProductionObjectFactoryProvider productionObjectFactoryProvider;
    
    public override void InstallBindings()
    {
        Container.Bind<IProductionObjectFactoryProvider>().FromInstance(productionObjectFactoryProvider).AsSingle();
        Container.BindFactory<BallInHole, ProductionObjectFactory>().AsSingle();
        Container.Bind<ProductionObjectInitializer>().FromNewComponentOnNewGameObject().AsSingle();
        
        
        Container.Bind<ClientSystem>().FromInstance(clientSystem).AsSingle();
        Container.Bind<BallInHole>().AsSingle();
        Container.Bind<ClientFactory>().AsSingle().NonLazy();

        var _clientPool = Container.InstantiatePrefabForComponent<ClientPool>(clientPool);
        Container.Bind<ClientPool>().FromInstance(_clientPool).AsSingle();
    }
}
