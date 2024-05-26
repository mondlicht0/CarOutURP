using CarOut.Cars.Attributes;
using CarOut.Cars.MVP;
using UnityEngine;
using Zenject;

namespace CarOut.Cars
{
	public class Car : MonoBehaviour
	{
		[SerializeField] private CarConfig _carData;
		
		private CarVisual _carVisual;
		private CarModel _carModel;

		private Vector2 _input;
		private InputHandler _inputHandler;

		#region DI REF
		private ICarController _controller;
		#endregion

		#region DATA PROPERTIES

		private float AccelSpeed => _carData.AccelerationSpeed;
		private float WheelBase => _carData.WheelBase;
		private float TurnRadius => _carData.TurnRadius;
		private float RearTrack => _carData.RearTrack;

		#endregion

		[Inject]
		private void Construct(ICarController carController, CarVisual carVisual)
		{
			_controller = carController;
			_carVisual = carVisual;
		}
		
		private void Awake()
		{
			_inputHandler = GetComponent<InputHandler>();
		}

		private void Start()
		{
			SetupCar();
		}

		private void SetupCar()
		{
			_carModel = new CarModel(_carVisual, _carData, _controller);
		}

		private void Update()
		{
			_input = _inputHandler.MovementInput;
		}

		private void FixedUpdate()
		{
			_controller.Move(
				_input, 
				AccelSpeed, 
				WheelBase,
				TurnRadius, 
				RearTrack);
		}
	}
}
