using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	[SerializeField] private Gun gunPrefab;
	[SerializeField] private Transform gunHolder;
	[SerializeField] private Transform playerCamera;
	[SerializeField] private LayerMask playerLayerMask;

	private Gun gun;

	private void Awake() {
		CreateGun();
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0)) ShootGun();
	}

	private void CreateGun() {
		gun = Instantiate(gunPrefab) as Gun;
		gun.transform.parent = gunHolder;
		gun.transform.localPosition = Vector3.zero;
		gun.transform.localRotation = Quaternion.identity;
	}

	private void ShootGun() {
		RaycastHit hit = GetHitAtReticle();
		gun.Shoot(GetLookDirection(), hit);
	}

	private RaycastHit GetHitAtReticle() {
		RaycastHit hit;
		Ray ray = new Ray(playerCamera.position, GetLookDirection());
		Physics.Raycast(ray, out hit, Mathf.Infinity, ~playerLayerMask.value);

		return hit;
	}

	private Vector3 GetLookDirection() {
		return playerCamera.forward;
	}
}
