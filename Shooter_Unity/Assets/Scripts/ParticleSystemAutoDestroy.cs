using UnityEngine;
using System.Collections;

public class ParticleSystemAutoDestroy : MonoBehaviour {

	private ParticleSystem particleSystem;

	void Start () {
		particleSystem = GetComponent<ParticleSystem>();
	}
	
	void Update () {
		if (particleSystem) {
			if(!particleSystem.IsAlive()) {
				Destroy(gameObject);
			}
		}
	}
}
