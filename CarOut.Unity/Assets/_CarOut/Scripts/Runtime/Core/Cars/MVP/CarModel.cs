using CarOut.Cars.Attributes;

namespace CarOut.Cars.MVP
{
	public class CarModel : Model
	{
		protected CarConfig CarData;
		protected ICarController _carController;
		
		public CarModel(CarVisual view, CarConfig carData, ICarController carController) : base(view)
		{
			_carController = carController;
			_view = view;
			CarData = carData;
		}
	}
}
