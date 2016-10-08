using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public GameObject camera; // attach the camera
	public GameObject camera_object; // attach the camera object that will be transformed during panning and rotation
	public GameObject camera_poivot_point; // attach the center object around which the camera will rotate and towards which it will zoom in



	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		// One touch means there's one finger and therefore we're panning the camera
		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			// Get movement of the finger since last frame
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

			// Adjust the speed of the camera based on height... faster when you're higher up
			float camera_y_pos = Mathf.Round(camera.transform.position.y);

			// Move object across XY plane
			camera_object.transform.Translate(-touchDeltaPosition.x / (150/camera_y_pos), 0, -touchDeltaPosition.y / (150/camera_y_pos));
		}



		// Two touches means two fingers and therefore a zoom or rotate
		if(Input.touchCount == 2) {

			// Store both touches
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			// Store the current positions of each touch
			Vector2 touchZeroPos = touchZero.position;
			Vector2 touchOnePos = touchOne.position;

			// Store the position of each touch in the previous frame
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// Find the distance between the touches in each frame (magnitude)
			float prevTouchDistance = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float currentTouchDistance = (touchZero.position - touchOne.position).magnitude;

			// Find the change in the distances between each frame
			float touchDistanceChange = prevTouchDistance - currentTouchDistance;

			// if the camera is not at the minimum limit, allow movement
			if (camera.transform.position.y > 40 && camera.transform.position.y < 70) {
				camera.transform.position = Vector3.MoveTowards (camera.transform.position, camera_poivot_point.transform.position, -touchDistanceChange / 5);
			
			// if the camera reaches the lowest limit, allow zoom out only
			} else if (touchDistanceChange > 0 && camera.transform.position.y < 70) {
				camera.transform.position = Vector3.MoveTowards (camera.transform.position, camera_poivot_point.transform.position, -touchDistanceChange / 5);
			} else if (touchDistanceChange < 0 && camera.transform.position.y > 40) {
				camera.transform.position = Vector3.MoveTowards (camera.transform.position, camera_poivot_point.transform.position, -touchDistanceChange / 5);
			}


			// Rotate
			Vector2 currentTouchDifference = touchZeroPos - touchOnePos;
			Vector2 prevTouchDifference = touchZeroPrevPos - touchOnePrevPos;
			float currentAngle = Mathf.Atan2(currentTouchDifference.y, currentTouchDifference.x)*Mathf.Rad2Deg;
			float prevAngle = Mathf.Atan2(prevTouchDifference.y, prevTouchDifference.x)*Mathf.Rad2Deg;
			float transformAngle = (currentAngle - prevAngle)*7/2;

			transform.Rotate(0, transformAngle, 0);


		}


	}
}
