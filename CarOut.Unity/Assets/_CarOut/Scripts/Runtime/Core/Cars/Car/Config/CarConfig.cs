using UnityEngine;
using Zenject;

namespace CarOut.Cars.Attributes 
{
	[CreateAssetMenu(fileName = "New Car Config", menuName = "Configs/Car Config", order = 0)]
	public class CarConfig : ScriptableObject
	{
		[field: SerializeField] public string Name { get; private set; } = "Default";
		[field: SerializeField] public float Speed { get; private set; }
		[field: SerializeField] public float ReverseSpeed { get; private set; }
		[field: SerializeField] public float TurnSpeed { get; private set; }
		[field: SerializeField] public float Mass { get; private set; }
		
		[field: SerializeField] public float AirDrag { get; private set; }
		[field: SerializeField] public float GroundDrag { get; private set; }
		[field: SerializeField] public float AngularDrag { get; private set; }

		[field: SerializeField] public float WheelBase { get; private set; }
		[field: SerializeField] public float RearTrack { get; private set; }
		[field: SerializeField] public float TurnRadius { get; private set; }
		[field: SerializeField] public float AccelerationSpeed { get; private set; }
		
		public void SetupToCarRigidbody(Rigidbody rigidbody) 
		{
			rigidbody.mass = Mass;
			rigidbody.drag = GroundDrag;
			rigidbody.angularDrag = AngularDrag;
		}
	}
}
