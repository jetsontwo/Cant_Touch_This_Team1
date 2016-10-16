using UnityEngine;
using System.Collections;

public class Slap_Player : MonoBehaviour {

    private Collider2D thisCollider;
    private GameManagerScript gameScript;
    public float stunTime, anglesPerSecond;

	// Use this for initialization
	void Start () {
        thisCollider = GetComponent<Collider2D>();
        gameScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collided) {
        if (collided.tag == "Player") {
            GameObject player = collided.gameObject;
            gameScript.stunPlayer(player, gameObject, stunTime, anglesPerSecond);
        }
    }
}
