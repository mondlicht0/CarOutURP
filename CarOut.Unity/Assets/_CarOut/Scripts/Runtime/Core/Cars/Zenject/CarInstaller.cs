using CarOut.Cars;
using UnityEngine;
using Zenject;

public class CarInstaller : MonoInstaller
{
	[SerializeField] private Car car;
	
	public override void InstallBindings()
	{
		InstallPlayer();
	}
	
	private void InstallPlayer() 
	{
		Container.Bind<Car>().FromInstance(car).AsSingle();
	}
}
