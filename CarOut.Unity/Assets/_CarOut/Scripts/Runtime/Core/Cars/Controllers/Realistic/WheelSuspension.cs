using UnityEngine;

namespace CarOut.Cars.Controller 
{
	public class WheelSuspension : MonoBehaviour
	{
		private Rigidbody _carRigidbody;
		
		[field: SerializeField] public bool IsFrontWheel { get; private set; }
		
		[Header("Wheel Attributes")]
		[SerializeField] private AnimationCurve _powerCurve;
		[SerializeField] private float _mass;
		[SerializeField] private float _restDistance = 2f;
		[SerializeField] private float _strength = 100f;
		[SerializeField] private float _dampingForce = 1f;
		[SerializeField] private float _carTopSpeed = 200f;
		[SerializeField, Range(0, 1)] private float _tireGripFactor = 1f;
		private float _wheelAngle;
		public float SteerAngle;
		private float _steerTime;
		
		[field: SerializeField] public float RampLength { get; private set; }
		public Vector3 SpringDirection { get; private set; }
		public Vector3 SteeringDirection { get; private set; }
		public Vector3 AccelerationDirection { get; private set; }
		public Vector3 TireWorldVelocity { get; private set; }
		
		
		private void Awake() 
		{
			_carRigidbody = transform.root.GetComponent<Rigidbody>();
		}
		
		private void Update()
		{
			SetDirections();
			RotateWheel();
			
			TireWorldVelocity = _carRigidbody.GetPointVelocity(transform.position);
		}
		
		private void RotateWheel() 
		{
			_wheelAngle = Mathf.Lerp(_wheelAngle, SteerAngle, _steerTime * Time.deltaTime);
			transform.localRotation = Quaternion.Euler(Vector3.up * SteerAngle);
		}
		
		private void SetDirections() 
		{
			SpringDirection = transform.up;
			SteeringDirection = transform.right;
			AccelerationDirection = transform.forward;
		}
		
		public float GetScalarSuspensionForce(RaycastHit hit) 
		{
			float offset = _restDistance - hit.distance;
			float velocity = Vector3.Dot(SpringDirection, TireWorldVelocity);
			float force = (offset * _strength) - (velocity * _dampingForce);
			
			return force;
		}
		
		public Vector3 GetSteeringForce() 
		{
			float steeringVelocity = Vector3.Dot(SteeringDirection, TireWorldVelocity);
			float desiredVelocityChange = -steeringVelocity * _tireGripFactor; // steering
			float desiredAcceleration = desiredVelocityChange / Time.fixedDeltaTime; // steering
			
			Vector3 steeringForce = SteeringDirection * _mass * desiredAcceleration;
			
			return steeringForce;
		}
		
		public float GetAcceleration(float accelerationInput) 
		{
			float carSpeed = Vector3.Dot(_carRigidbody.transform.forward, _carRigidbody.velocity);
			float normalizeSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / _carTopSpeed);
			float avaliableTorque = _powerCurve.Evaluate(normalizeSpeed) * accelerationInput;
			
			return avaliableTorque;
		}
	}
}
