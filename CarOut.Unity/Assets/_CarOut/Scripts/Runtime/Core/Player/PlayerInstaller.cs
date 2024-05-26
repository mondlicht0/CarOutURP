using CarOut.Cars;
using CarOut.Cars.MVP;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Car _carPrefab;
    
    public override void InstallBindings()
    {
        GameObject carGameObject = Container.InstantiatePrefab(_carPrefab);
        Car car = carGameObject.GetComponent<Car>();
        CarVisual carVisual = carGameObject.GetComponentInChildren<CarVisual>();
        
        Container.Bind<Car>().FromInstance(car).AsSingle();
        Container.Bind<CarVisual>().FromInstance(carVisual).AsSingle();
    }
}
