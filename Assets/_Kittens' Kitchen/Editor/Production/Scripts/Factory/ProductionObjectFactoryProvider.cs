using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionObjectFactoryProvider : MonoBehaviour, IProductionObjectFactoryProvider
{
    public MiniGameInitializer MiniGamesInitPrefab;
    public MiniGameInitializer MiniGameInitializer => MiniGamesInitPrefab;
}

public interface IProductionObjectFactoryProvider
{
    MiniGameInitializer MiniGameInitializer { get; }
}
