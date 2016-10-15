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
        Vector3 midpointBetweenPlayers = (object1.transform.position + object2.transform.position) / 2;
        transform.position = midpointBetweenPlayers + new Vector3(0, 0, -10);


        changeCameraSize();

        
	}

    void changeCameraSize() {
        float distanceBetweenPlayers = Vector2.Distance(object1.transform.position, object2.transform.position);
        float playersBounding = 5 * mainCamera.aspect / 11.75f;

        float adjustmentRatio = 5 / cameraZoomRatio * mainCamera.aspect;
        //        mainCamera.orthographicSize = Mathf.Clamp(Mathf.Pow(distanceBetweenPlayers * adjustmentRatio, 1/1.1f), minCameraSize, distanceBetweenPlayers * playersBounding );
        mainCamera.orthographicSize = Mathf.Clamp(distanceBetweenPlayers * adjustmentRatio, minCameraSize, Mathf.Max(distanceBetweenPlayers * playersBounding, minCameraSize));
    }


}
