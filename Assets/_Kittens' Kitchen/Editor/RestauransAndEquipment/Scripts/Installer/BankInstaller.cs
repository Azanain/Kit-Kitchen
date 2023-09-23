using UnityEngine;
using Zenject;

public class BankInstaller : MonoInstaller
{
    [SerializeField] private Bank _bank;
    public override void InstallBindings()
    {
        Container.Bind<Bank>().FromInstance(_bank).AsSingle();
    }
}
