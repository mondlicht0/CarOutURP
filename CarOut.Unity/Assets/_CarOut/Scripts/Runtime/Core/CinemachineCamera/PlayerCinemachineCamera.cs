using System;
using CarOut.Cars;
using CarOut.Cars.Controller;
using CarOut.Cars.MVP;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace CarOut.Cameras
{
    public class PlayerCinemachineCamera : MonoBehaviour
    {
        private CinemachineVirtualCamera _camera;

        #region DI REF
        private Car _playerCar;
        private CarVisual _playerVisual;
        #endregion

        [Inject]
        private void Construct(Car car, CarVisual carVisual)
        {
            _playerCar = car;
            _playerVisual = carVisual;
        }

        private void Awake()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Start()
        {
            _camera.Follow = _playerCar.transform;
            _camera.LookAt = _playerVisual.transform;
        }
    }
}
