using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProductionObjectFactory : PlaceholderFactory<BallInHole>
{
    private readonly DiContainer _container;
    private readonly IProductionObjectFactoryProvider _productionObjectFactoryProvider;

    public ProductionObjectFactory(DiContainer container, IProductionObjectFactoryProvider productionObjectFactoryProvider)
    {
        _container = container;
        _productionObjectFactoryProvider = productionObjectFactoryProvider;
    }

    public MiniGameInitializer Create()
    {
        Debug.Log("Started process creation of BallInHole... without injection");
        MiniGameInitializer miniGameInitializer = CreateObjectsWithoutInjection();
        
        Debug.Log("Finished process creation of BallInHole... without injection");
        return miniGameInitializer;
    }

    private MiniGameInitializer CreateObjectsWithoutInjection()
    {
        MiniGameInitializer miniGameInitPrefab = _productionObjectFactoryProvider.MiniGameInitializer;
        MiniGameInitializer miniGameInitializer = _container.InstantiatePrefabForComponent<MiniGameInitializer>(miniGameInitPrefab);

        return miniGameInitializer;
    }
}
