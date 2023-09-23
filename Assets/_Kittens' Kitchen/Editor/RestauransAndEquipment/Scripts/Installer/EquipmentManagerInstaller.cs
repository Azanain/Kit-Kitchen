using UnityEngine;
using Zenject;

public class EquipmentManagerInstaller : MonoInstaller
{
    [SerializeField] private EquipmentManager _equipmentManager;
    public override void InstallBindings()
    {
        Container.Bind<EquipmentManager>().FromInstance(_equipmentManager).AsSingle();
    }
}
