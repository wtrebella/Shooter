using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {
	public static float ClampAngle (float angle, float min, float max) {
		if (angle < -360f) angle += 360f;
		if (angle > 360f) angle -= 360f;
		return Mathf.Clamp (angle, min, max);
	}
	
	[SerializeField] private Transform PlayerCamera;

	private enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	[SerializeField] private RotationAxes axes = RotationAxes.MouseXAndY;
	[SerializeField] private float sensitivityX = 15f;
	[SerializeField] private float sensitivityY = 15f;
	
	[SerializeField] private float minimumX = -360f;
	[SerializeField] private float maximumX = 360f;
	
	[SerializeField] private float minimumY = -60f;
	[SerializeField] private float maximumY = 60f;
	
	[SerializeField] private float rotationX = 0f;
	[SerializeField] private float rotationY = 0f;
	
	private Quaternion originalRotation;
	private PlayerInputController input;
	
	void Start () {
		input = gameObject.GetComponent<PlayerInputController>();
		originalRotation = transform.localRotation;
	}
	
	private void Update() {
		UpdateLookDirection();
	}
	
	private void UpdateLookDirection() {
		if (axes == RotationAxes.MouseXAndY) {
			rotationX += input.Current.MouseInput.x * sensitivityX;
			rotationY += input.Current.MouseInput.y * sensitivityY;
			
			rotationX = ClampAngle (rotationX, minimumX, maximumX);
			rotationY = ClampAngle (rotationY, minimumY, maximumY);
			
			Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
			Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);
			
			PlayerCamera.transform.localRotation = originalRotation * xQuaternion * yQuaternion;
		}
		else if (axes == RotationAxes.MouseX) {
			rotationX += input.Current.MouseInput.x * sensitivityX;
			rotationX = ClampAngle (rotationX, minimumX, maximumX);
			
			Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
			PlayerCamera.transform.localRotation = originalRotation * xQuaternion;
		}
		else {
			rotationY += input.Current.MouseInput.y * sensitivityY;
			rotationY = ClampAngle (rotationY, minimumY, maximumY);
			
			Quaternion yQuaternion = Quaternion.AngleAxis (-rotationY, Vector3.right);
			PlayerCamera.transform.localRotation = originalRotation * yQuaternion;
		}
	}
}
