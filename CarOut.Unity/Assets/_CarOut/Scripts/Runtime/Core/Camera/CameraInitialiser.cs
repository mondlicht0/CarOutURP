using Cinemachine;
using UnityEngine;
using Zenject;
using CarOut.Cars;

namespace CarOut.Camera 
{
	public class CameraInitialiser : MonoBehaviour
	{
		private CinemachineVirtualCamera _virtualCamera;
		private Car _car;
		
		[Inject]
		private void Init(Car car) 
		{
			_car = car;
		}
		
		void Awake()
		{
			_virtualCamera = GetComponent<CinemachineVirtualCamera>();
		}
		
		private void Start()
		{
			_virtualCamera.Follow = _car.transform;
			_virtualCamera.LookAt = _car.transform;
		}
	}
}
