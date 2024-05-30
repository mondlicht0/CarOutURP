using System;
using CarOut.Cars.Attributes;
using UnityEngine;

namespace CarOut.Cars.MVP 
{
	public class CarPresenter : Presenter
	{
		private CarVisual _visual;
		private CarModel _model;
		private ICarController _controller;

		private CarPresenter(CarModel model, CarVisual view, ICarController controller) : base(model, view) 
		{
			_model = model;
			_visual = view;
			_controller = controller;
			
			_visual.Init(this);
		}

		public void LogicUpdate(Vector2 input)
		{
			_model.UpdateCarInput(input);
		}

		public void PhysicsUpdate()
		{
			_controller.Move(
				_model.MoveDirection, 
				_model.CarData.AccelerationSpeed, 
				_model.CarData.WheelBase, 
				_model.CarData.TurnRadius, 
				_model.CarData.RearTrack
			);
		}
		
		public class Builder
		{
			private CarModel _model;

			public Builder WithConfig(CarConfig config)
			{
				_model = new CarModel(config);
				return this;
			}

			public CarPresenter Build(CarVisual visual, ICarController controller)
			{
				if (visual == null)
				{
					throw new Exception("Visual is null");
				}

				if (controller == null)
				{
					throw new Exception("Controller is null");
				}
			
				return new CarPresenter(_model, visual, controller);
			}
		}
	}
}
