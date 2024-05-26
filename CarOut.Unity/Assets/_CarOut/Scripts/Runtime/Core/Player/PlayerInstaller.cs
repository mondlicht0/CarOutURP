using CarOut.Cars;
using CarOut.Cars.MVP;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Car _carPrefab;
    [SerializeField] private CarVisual _carVisual;
    
    public override void InstallBindings()
    {
        Container.InstantiatePrefab(_carPrefab);
        Container.Bind<CarVisual>().FromInstance(_carVisual);
    }
}
