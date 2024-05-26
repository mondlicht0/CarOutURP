using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
	private Controls _controls;
	public Vector2 MovementInput { get; private set; }
	
	private void OnEnable()
	{
		_controls.Enable();
	}
	
	private void OnDisable()
	{
		_controls.Disable();
	}
	
	private void Awake()
	{
		_controls = new Controls();
	}
	
	public void OnMoveInput(InputAction.CallbackContext context)
	{
		MovementInput = context.ReadValue<Vector2>();
	}
}
