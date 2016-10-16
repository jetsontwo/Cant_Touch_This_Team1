﻿using UnityEngine;
using System.Collections;

public class Camera_Behavior : MonoBehaviour {

    private Camera mainCamera;
    private float cameraZoomRatio;
    private float minCameraSize;

    private float baseCamSize, totalBaseCamSize, sizeYLimit;

    public GameObject object1, object2;

	// Use this for initialization
	void Start () {
        mainCamera = GetComponent<Camera>();
        cameraZoomRatio = 8;
        minCameraSize = 5;

        baseCamSize = mainCamera.orthographicSize;
        totalBaseCamSize = baseCamSize * 2;
        sizeYLimit = totalBaseCamSize * 5;
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
        float playersBoundingY = minCameraSize * mainCamera.aspect / totalBaseCamSize;
        float adjustmentRatioY = minCameraSize / (totalBaseCamSize * 3/4) * mainCamera.aspect;
        float difference = adjustmentRatioY * distanceBetweenPlayers - playersBoundingY * distanceBetweenPlayers;

        float linearTransitionForCamera = distanceBetweenPlayers * playersBoundingY + difference * (1 - (distanceBetweenPlayers) / sizeYLimit);
        //print(linearTransitionForCamera);
        //print(distanceBetweenPlayers * playersBounding + " | " + difference * (distanceBetweenPlayers / 50) + " | " + linearTransitionForCamera + " | " + distanceBetweenPlayers * playersBounding);
        //        mainCamera.orthographicSize = Mathf.Clamp(Mathf.Pow(distanceBetweenPlayers * adjustmentRatio, 1/1.1f), minCameraSize, distanceBetweenPlayers * playersBounding );
        mainCamera.orthographicSize = Mathf.Clamp(linearTransitionForCamera, minCameraSize, Mathf.Max(sizeYLimit, minCameraSize));
    }


}
