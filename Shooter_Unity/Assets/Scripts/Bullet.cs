using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	private Rigidbody rigidbody;

	public void Shoot(float force) {
		SetRigidbodyKinematic(false);
		AddForwardImpulseForce(force);
	}

	void Awake() {
		rigidbody = GetComponent<Rigidbody>();
	}

	private void SetRigidbodyKinematic(bool kinematic) {
		rigidbody.isKinematic = kinematic;
	}

	private void AddForwardImpulseForce(float force) {
		Vector3 forceVector = new Vector3(0, 0, force);
		rigidbody.AddRelativeForce(forceVector, ForceMode.Impulse);
	}
}
