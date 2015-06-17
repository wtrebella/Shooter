using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	[SerializeField] private Bullet bulletPrefab;
	[SerializeField] private float bulletRicochetMagnitude = 2;
	[SerializeField] private float bulletRandomRicochetDirectionMagnitude = 0.2f;
	[SerializeField] private float bulletRandomTorqueMagnitude = 2;

	public void Shoot(Vector3 direction, RaycastHit hit) {
		if (hit.collider == null) return;

		Bullet bullet = CreateBullet(direction, hit.point);
		RicochetBullet(bullet, direction, hit);
	}

	private Bullet CreateBullet(Vector3 direction, Vector3 bulletPosition) {
		Bullet bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.LookRotation(direction)) as Bullet;
		return bullet;
	}

	private void RicochetBullet(Bullet bullet, Vector3 direction, RaycastHit hit) {
		float randomizedX = direction.x + Random.Range(-bulletRandomRicochetDirectionMagnitude, bulletRandomRicochetDirectionMagnitude);
		float randomizedY = direction.y + Random.Range(-bulletRandomRicochetDirectionMagnitude, bulletRandomRicochetDirectionMagnitude);
		float randomizedZ = direction.z + Random.Range(-bulletRandomRicochetDirectionMagnitude, bulletRandomRicochetDirectionMagnitude);
		Vector3 randomizedDirection = new Vector3(randomizedX, randomizedY, randomizedZ);
		Vector3 force = Vector3.Reflect(randomizedDirection, hit.normal).normalized;
		force *= bulletRicochetMagnitude;
		bullet.AddImpulseForce(force);
	}

	private void AddRandomTorqueToBullet(Bullet bullet) {
		float torqueX = Random.Range(-bulletRandomTorqueMagnitude, bulletRandomTorqueMagnitude);
		float torqueY = Random.Range(-bulletRandomTorqueMagnitude, bulletRandomTorqueMagnitude);
		float torqueZ = Random.Range(-bulletRandomTorqueMagnitude, bulletRandomTorqueMagnitude);
		Vector3 torque = new Vector3(torqueX, torqueY, torqueZ);
		bullet.AddImpulseTorque(torque);
	}
}
