using UnityEngine;
using System.Collections;

public class BulletExplosion : MonoBehaviour {
	private ParticleSystem particleSystem;

	private void Awake() {
		particleSystem = GetComponent<ParticleSystem>();
	}

	private void OnEnable() {
		particleSystem.Clear();
	}

	private void Update() {
		if(!particleSystem.IsAlive()) {
			gameObject.Recycle();
		}
	}
}
