using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour
{
    public float invincibilityMaxTimer;

    private bool player1_is_it = false;
    private float invincibilityTimer;
    
	void Start ()
    {
        invincibilityTimer = 0;
        if (Random.value > 0.5)
        {
            player1_is_it = true;
        }
	}

    public void NotifyTouched()
    {
        if (invincibilityTimer <= 0)
        {
            player1_is_it = !player1_is_it;
            invincibilityTimer = invincibilityMaxTimer;
        }
    }
	
	void Update ()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
	}
}
