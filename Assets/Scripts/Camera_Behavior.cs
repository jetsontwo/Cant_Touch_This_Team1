using UnityEngine;
using System.Collections;

public class Camera_Behavior : MonoBehaviour {

    private Camera mainCamera;

    public GameObject object1, object2;

	// Use this for initialization
	void Start () {
        mainCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Puts camera between the 2 players
        print(object1.transform.position + " | " + object2.transform.position + " | " + (object1.transform.position - object2.transform.position) / 2);
        transform.position = (object1.transform.position + object2.transform.position) / 2 + new Vector3(0, 0, -10);
	}


}
