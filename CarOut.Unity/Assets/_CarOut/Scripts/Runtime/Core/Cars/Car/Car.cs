using CarOut.Cars.Attributes;
using CarOut.Cars.Controller;
using CarOut.Cars.MVP;
using UnityEngine;

namespace CarOut.Cars
{
	public class Car : MonoBehaviour
	{
		[SerializeField] private CarConfig _carData;

		private CarVisual _carVisual;
		private CarPresenter _carPresenter;
		private CarControllerWheelBased _controller;
		private InputHandler _inputHandler;
		private Vector2 _moveInput;
		
		private void Awake()
		{
			_inputHandler = GetComponent<InputHandler>();
			_controller = GetComponent<CarControllerWheelBased>();
			_carVisual = GetComponentInChildren<CarVisual>();
		}

		private void Start()
		{
			_carPresenter = new CarPresenter.Builder()
				.WithConfig(_carData)
				.Build(_carVisual, _controller);
		}

		private void Update()
		{
			_carPresenter.LogicUpdate(_inputHandler.MovementInput);
		}

		private void FixedUpdate()
		{
			_carPresenter.PhysicsUpdate();
		}
	}
}
