using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public float gameMaxTimer;
    public float invincibilityMaxTimer;
    public Timer gameTimer;

    private bool player1_is_it = false;
    private float invincibilityTimer;
    
	void Start ()
    {
        invincibilityTimer = 0;
        if (Random.value > 0.5)
        {
            player1_is_it = true;
        }
        gameTimer.timeLeft = gameMaxTimer;
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
