using UnityEngine;
using System.Collections;

public class TouchDetect : MonoBehaviour {

	RaycastHit hit;
	Ray ray;


	// Update is called once per frame
	void Update () {

		if (Input.touchCount == 1) {
			ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			if(Physics.Raycast(ray, out hit)) {
				Debug.Log(hit.collider.gameObject.name);
			}
		}
	
	}
}
