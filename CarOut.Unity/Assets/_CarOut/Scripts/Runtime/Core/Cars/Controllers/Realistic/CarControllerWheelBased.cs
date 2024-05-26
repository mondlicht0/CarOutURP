using System;
using CarOut.Cars.Attributes;
using UnityEngine;

namespace CarOut.Cars.Controller 
{
	public class CarControllerWheelBased : MonoBehaviour, ICarController
	{
		[Header("Wheels")]
		[SerializeField] private WheelSuspension[] _wheelsSuspension;
		[SerializeField] private LayerMask _layerMask;
		[SerializeField] private TrailRenderer[] _wheelsTrail;
		
		private bool _isGrounded;
		private bool _trailActive;
		private Rigidbody _carRigidbody;
		
		private float _ackermannAngleLeft;
		private float _ackermannAngleRight;

		private bool _isInitiasied = false;
		
		private void Awake()
		{
			_carRigidbody = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			SetWheelsEmmiting();
		}

		private void SetWheelsEmmiting()
		{
			_wheelsTrail[0].emitting = _trailActive;
			_wheelsTrail[1].emitting = _trailActive;
		}

		public void Move(Vector2 direction, float accelSpeed, float wheelBase, float turnRadius, float rearTrack)
		{
			foreach (var wheel in _wheelsSuspension)
			{
				WheelMovement(wheel, direction.y, accelSpeed);
			}
			
			AckermannWheelAngle(direction.x, wheelBase, turnRadius, rearTrack);
		}
		
		private void ApplyForces(WheelSuspension wheel, RaycastHit hit, float accelDirection, float accelSpeed) 
		{
			SuspensionForce(wheel, hit);
			SteeringForce(wheel);
			Acceleration(wheel, accelDirection, accelSpeed);
		}

		private void SuspensionForce(WheelSuspension wheel, RaycastHit hit)
		{
			Vector3 suspension = wheel.SpringDirection * wheel.GetScalarSuspensionForce(hit);
			_carRigidbody.AddForceAtPosition(suspension, wheel.transform.position);
		}
		
		private void SteeringForce(WheelSuspension wheel)
		{
			Vector3 steering = wheel.GetSteeringForce();
			_carRigidbody.AddForceAtPosition(steering, wheel.transform.position);
		}
		
		private void Acceleration(WheelSuspension wheel, float accelDirection, float accelSpeed)
		{
			Vector3 acceleration = wheel.AccelerationDirection * wheel.GetAcceleration(accelDirection);
			_carRigidbody.AddForceAtPosition(acceleration * accelSpeed, wheel.transform.position);
		}
		
		private void WheelMovement(WheelSuspension wheel, float accelDirection, float accelSpeed) 
		{
			if (Physics.Raycast(wheel.transform.position, -wheel.transform.up, out RaycastHit hit, wheel.RampLength, _layerMask)) 
			{
				ApplyForces(wheel, hit, accelDirection, accelSpeed);
				_isGrounded = true;
				
				if (!wheel.IsFrontWheel) 
				{
					_trailActive = true;
				}
			}
			
			else 
			{
				_isGrounded = false;
				_trailActive = false;
			}
		}	
		
		private void AckermannWheelAngle(float steerDirection, float wheelBase, float turnRadius, float rearTrack) 
		{
			_ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2 * steerDirection > 0 ? 1 : -1)) * steerDirection); 
			_ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2 * steerDirection > 0 ? -1 : 1)) * steerDirection); 
			
			if (steerDirection == 0) 
			{
				_ackermannAngleRight = 0;
				_ackermannAngleLeft = 0;
			}
			
			_wheelsSuspension[0].SteerAngle = _ackermannAngleLeft;
			_wheelsSuspension[1].SteerAngle = _ackermannAngleRight;
		}
	
		private void Break() 
		{
			
		}
	}
}
