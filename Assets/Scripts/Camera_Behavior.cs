using UnityEngine;
using System.Collections;

public class Camera_Behavior : MonoBehaviour {

    private Camera mainCamera;
    private float cameraZoomRatio;
    private float minCameraSize;

    public GameObject object1, object2;

	// Use this for initialization
	void Start () {
        mainCamera = GetComponent<Camera>();
        cameraZoomRatio = 8;
        minCameraSize = 5;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Puts camera between the 2 players
        transform.position = (object1.transform.position + object2.transform.position) / 2 + new Vector3(0, 0, -10);
        mainCamera.orthographicSize = Mathf.Clamp(Vector2.Distance(object1.transform.position, object2.transform.position) / cameraZoomRatio * 5 * mainCamera.aspect, minCameraSize, 50);

        
	}


}
