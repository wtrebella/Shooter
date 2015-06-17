using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	[SerializeField] private ParticleSystem bulletExplosionPrefab;
	[SerializeField] private float bulletRicochetMagnitude = 20;
	[SerializeField] private float bulletRandomRicochetDirectionMagnitude = 0.1f;
	[SerializeField] private float bulletRandomTorqueMagnitude = 4;
	private Rigidbody rigidbody;
	public bool isActive {get; private set;}

	public void InitializeAtHitAndActivate(Vector3 direction, RaycastHit hit) {
		CreateBulletExplosion(hit.point);
		Activate();
		AddRandomTorque();
		Ricochet(direction, hit);
	}

	private void Activate() {
		rigidbody.isKinematic = false;
	}

	private void Deactivate() {
		rigidbody.isKinematic = false;
	}

	private void Awake() {
		rigidbody = GetComponent<Rigidbody>();
	}

	private void OnEnable() {
		Deactivate();
	}
	
	private void CreateBulletExplosion(Vector3 position) {
		bulletExplosionPrefab.Spawn<ParticleSystem>(position);
	}

	private void Ricochet(Vector3 direction, RaycastHit hit) {
		float randomizedX = direction.x + Random.Range(-bulletRandomRicochetDirectionMagnitude, bulletRandomRicochetDirectionMagnitude);
		float randomizedY = direction.y + Random.Range(-bulletRandomRicochetDirectionMagnitude, bulletRandomRicochetDirectionMagnitude);
		float randomizedZ = direction.z + Random.Range(-bulletRandomRicochetDirectionMagnitude, bulletRandomRicochetDirectionMagnitude);
		Vector3 randomizedDirection = new Vector3(randomizedX, randomizedY, randomizedZ);
		Vector3 force = Vector3.Reflect(randomizedDirection, hit.normal).normalized;
		force *= bulletRicochetMagnitude;
		rigidbody.AddForce(force, ForceMode.Impulse);
	}
	
	private void AddRandomTorque() {
		float torqueX = Random.Range(-bulletRandomTorqueMagnitude, bulletRandomTorqueMagnitude);
		float torqueY = Random.Range(-bulletRandomTorqueMagnitude, bulletRandomTorqueMagnitude);
		float torqueZ = Random.Range(-bulletRandomTorqueMagnitude, bulletRandomTorqueMagnitude);
		Vector3 torque = new Vector3(torqueX, torqueY, torqueZ);
		rigidbody.AddTorque(torque, ForceMode.Impulse);
	}
}