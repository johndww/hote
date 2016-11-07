using UnityEngine;


public class CollectLineLocationUI : MonoBehaviour
{
	private Vector3 firstPoint;
	private Vector3 secondPoint;

	public Vector3 GetMoveDelta() {
		//TODO figure out what we actually need for rotations
		return this.firstPoint - this.secondPoint;
	}

	public Vector3 GetLocation() {
		return (this.firstPoint + this.secondPoint) / 2;
	}

	public bool IsComplete() {
		return this.firstPoint != Vector3.zero && this.secondPoint != Vector3.zero;
	}

	void Update () {
		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
			// this detects if the user clicked a UI element. ignore those clicks
			return;
		}

		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			handleMobile();
		}
		else {
			handleDesktop();
		}
			
		if (IsComplete()) {
			GetComponent<CollectLineLocationUI>().enabled = false;
		}
	}

	void handleDesktop ()
	{
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			this.firstPoint = capturePoint(ray);
		}
		if (Input.GetMouseButtonUp(0) && this.firstPoint != Vector3.zero) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			this.secondPoint = capturePoint(ray);
		}
	}

	void handleMobile ()
	{
		if (Input.touchCount != 1) {
			return;
		}

		Touch touch = Input.GetTouch(0);

		if (touch.phase == TouchPhase.Began) {
			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			this.firstPoint = capturePoint(ray);
		}
		if (touch.phase == TouchPhase.Ended && this.firstPoint != Vector3.zero) {
			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			this.secondPoint = capturePoint(ray);
		}

	}

	Vector3 capturePoint (Ray ray)
	{
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 200)) {
			return hit.point;
		}
		return Vector3.zero;
	}

	public void Reset() {
		this.firstPoint = Vector3.zero;
		this.secondPoint = Vector3.zero;
	}
}
