using CarOut.Cars.Attributes;
using UnityEngine;

namespace CarOut.Cars.MVP
{
	public class CarModel : Model
	{
		public CarConfig CarData { get; private set; }
		public Vector2 MoveDirection { get; private set; }
		
		public CarModel(CarConfig carData)
		{
			CarData = carData;
		}

		public void UpdateCarInput(Vector2 newDirection)
		{
			MoveDirection = newDirection;
		}
	}
}
