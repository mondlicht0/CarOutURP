using System;
using UnityEngine;

namespace CarOut.Cars.MVP 
{
	public class CarVisual : View
	{
		[SerializeField] private MeshFilter _carMesh;

		private void Start()
		{
			InitVisual();
		}

		private void InitVisual()
		{
			if (_carMesh != null)
			{
				return;
			}
			
			if (TryGetComponent(out MeshFilter meshFilter))
			{
				_carMesh = meshFilter;
			}

			else
			{
				throw new Exception("Car Mesh is null");
			}
		}
	}
}
