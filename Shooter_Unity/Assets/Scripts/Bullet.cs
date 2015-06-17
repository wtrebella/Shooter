using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	private Rigidbody rigidbody;

	public void AddImpulseForce(Vector3 force) {
		rigidbody.AddForce(force, ForceMode.Impulse);
	}

	public void AddImpulseTorque(Vector3 torque) {
		rigidbody.AddTorque(torque, ForceMode.Impulse);
	}

	private void Awake() {
		rigidbody = GetComponent<Rigidbody>();
	}
}
