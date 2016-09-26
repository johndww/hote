using UnityEngine;
using System.Collections;

public class ClickMovement : MonoBehaviour {

	public Player player; //Attach the player object
    
    void OnMouseDown () //Start action when the mouse is clicked
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        Physics.Raycast(ray, out hit);
        
        if(hit.collider.gameObject == gameObject) //The script is attached to the plane so this detects collision with the plane as the gameObject
        {
            Vector3 newTarget = hit.point + new Vector3(0, 0.5f, 0);
            player.Target = newTarget;
        }
    }
}
