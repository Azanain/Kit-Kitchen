using UnityEngine;
using Zenject;

public class RestaurantsManagerInstaller : MonoInstaller
{
    [SerializeField] private RestaurantsManager _restaurantManager;
    public override void InstallBindings()
    {
        Container.Bind<RestaurantsManager>().FromInstance(_restaurantManager).AsSingle();
    }
}
