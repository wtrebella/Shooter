using UnityEngine;
using System.Collections;

public class CursorSetup : MonoBehaviour {

	void Awake () {

	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			LockCursor();
			HideCursor();
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			UnlockCursor();
			ShowCursor();
		}
	}

	private void LockCursor() {
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void HideCursor() {
		Cursor.visible = false;
	}

	private void UnlockCursor() {
		Cursor.lockState = CursorLockMode.None;
	}
	
	private void ShowCursor() {
		Cursor.visible = true;
	}
}
