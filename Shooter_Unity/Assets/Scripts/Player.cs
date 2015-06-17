using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public Transform firstPersonCharacter;
	public LayerMask playerLayerMask;

	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Collider collider = GetColliderAtReticle();
			if (collider != null) Debug.Log(collider.name);
		}
	}

	private Collider GetColliderAtReticle() {
		RaycastHit hit;
		Ray ray = new Ray(firstPersonCharacter.position, firstPersonCharacter.forward);
		Physics.Raycast(ray, out hit, Mathf.Infinity, ~playerLayerMask.value);

		return hit.collider;
	}
}
