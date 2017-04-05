using UnityEngine;
using System.Collections;

public class Camera_Behavior : MonoBehaviour {

    private Camera mainCamera;
    private float cameraZoomRatio;
    public float minCameraSize;
    public float moveSpeed;

    private float baseCamSize, totalBaseCamSize, sizeYLimit;

    public GameObject object1, object2;

    public bool shakeCam;
    private IEnumerator cameraShakeCoroutine;
    private float[] pastValues = new float[10];

	// Use this for initialization
	void Start () {
        mainCamera = GetComponent<Camera>();
        cameraZoomRatio = 8;

        baseCamSize = mainCamera.orthographicSize;
        totalBaseCamSize = baseCamSize * 2;
        sizeYLimit = totalBaseCamSize * 5;

        shakeCam = false;
        for (int i = 0; i < pastValues.Length; ++i)
        {
            pastValues[i] = mainCamera.orthographicSize;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //Puts camera between the 2 players
        Vector3 midpointBetweenPlayers = (object1.transform.position + object2.transform.position) / 2;
        transform.position = Vector3.Lerp(transform.position, midpointBetweenPlayers + new Vector3(0, 0, -10), moveSpeed * Time.fixedDeltaTime);

        changeCameraSize();

        if (shakeCam) {
            cameraShakeCoroutine = shakeCamera(.15f, .1f);
            StartCoroutine(cameraShakeCoroutine);
        }
    }

    private void changeCameraSize() {
        float distanceBetweenPlayers = Vector2.Distance(object1.transform.position, object2.transform.position);
        float playersBoundingY = minCameraSize * mainCamera.aspect / totalBaseCamSize;
        float adjustmentRatioY = minCameraSize / (totalBaseCamSize * 3/4) * mainCamera.aspect;
        float difference = adjustmentRatioY * distanceBetweenPlayers - playersBoundingY * distanceBetweenPlayers;

        float linearTransitionForCamera = distanceBetweenPlayers * playersBoundingY + difference * (1 - (distanceBetweenPlayers) / sizeYLimit);
        //print(linearTransitionForCamera);
        //print(distanceBetweenPlayers * playersBounding + " | " + difference * (distanceBetweenPlayers / 50) + " | " + linearTransitionForCamera + " | " + distanceBetweenPlayers * playersBounding);
        //        mainCamera.orthographicSize = Mathf.Clamp(Mathf.Pow(distanceBetweenPlayers * adjustmentRatio, 1/1.1f), minCameraSize, distanceBetweenPlayers * playersBounding );

        float avgValue = 0;
        for (int i = 0; i < pastValues.Length; ++i)
        {
            avgValue += pastValues[i];
        }
        avgValue /= pastValues.Length;

        mainCamera.orthographicSize = Mathf.Lerp(avgValue, Mathf.Clamp(linearTransitionForCamera, minCameraSize, Mathf.Max(sizeYLimit, minCameraSize)), Time.deltaTime);
        
        for (int i = 0; i < pastValues.Length - 1; ++i)
        {
            pastValues[i] = pastValues[i + 1];
        }
        pastValues[pastValues.Length - 1] = mainCamera.orthographicSize;
    }

    public IEnumerator shakeCamera(float inititalShakeDisplacement, float shakeTime) {
        Vector3 displacement = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0).normalized * inititalShakeDisplacement;
        float delay = .05f;

        while (shakeTime > 0) {
            transform.position += displacement;
            
            yield return new WaitForSeconds(delay);
            shakeTime -= delay;
        }
        shakeCam = false;

    }


}
