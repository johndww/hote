using UnityEngine;
using System.Collections;

public class AttackUIOverride : MonoBehaviour {

	private Vector3 location = Vector3.zero;

	// Update is called once per frame
	void Update () {
		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
			// this detects if the user clicked a UI element. ignore those clicks
			return;
		}

		if (PlayerInput.getPlayerInput().Equals(PlayerInput.Type.SELECT)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 200)) {
				this.location = hit.point;
				// we got a location, we're done
				GetComponent<AttackUIOverride>().enabled = false;
			}
		}
	}

	public Vector3 GetLocation() {
		return this.location;
	}

	public bool IsLocationSelected() {
		return this.location != Vector3.zero;
	}

	public void Reset() {
		this.location = Vector3.zero;
	}
}