using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : MonoBehaviour {
	[SerializeField] private Bullet bulletPrefab;
	private Queue<Bullet> spawnedBullets;
	
	public void Shoot(Vector3 direction, RaycastHit hit) {
		if (hit.collider == null) return;

		CreateBulletAtHit(direction, hit);
	}

	private void Awake() {
		spawnedBullets = new Queue<Bullet>();
	}

	private void CreateBulletAtHit(Vector3 direction, RaycastHit hit) {
		int numPooledBullets = bulletPrefab.CountPooled();
		if (numPooledBullets == 0) {
			Bullet oldestBullet = spawnedBullets.Dequeue();
			if (oldestBullet != null) ObjectPool.Recycle(oldestBullet);
		}
		Bullet bullet = bulletPrefab.Spawn(hit.point, Quaternion.LookRotation(direction));
		bullet.InitializeAtHitAndActivate(direction, hit);
		spawnedBullets.Enqueue(bullet);
	}
}
