namespace CarOut.Cars.MVP 
{
	public abstract class CarPresenter : Presenter
	{
		private CarVisual _visual;
		private CarModel _model;

		protected CarPresenter(CarModel model, CarVisual view) : base(model, view) 
		{
			_model = model;
			_visual = view;
		}
	}
}
