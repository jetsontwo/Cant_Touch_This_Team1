using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public float gameMaxTimer;
    public float invincibilityMaxTimer;
    public Timer gameTimer;
    public Text score_1;
    public Text score_2;
    public Text gameOver;
    public Button backToMain;
    public Button playAgain;
    public Object player1;
    public Object player2;

    private bool player1_is_it = false;
    private float invincibilityTimer;

    private int player1_score;
    private int player2_score;
    private Color green = new Color(0, 255, 0);
    private Color white = new Color(255, 255, 255);
    private Color red = new Color(255, 0, 0);

    

	void Start ()
    {
        invincibilityTimer = 0;
        player1_score = 0;
        player2_score = 0;
        backToMain.gameObject.SetActive(false);
        playAgain.gameObject.SetActive(false);
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
        if ((int)gameTimer.timeLeft > 0)
        {
            if (invincibilityTimer >= 0)
            {
                invincibilityTimer -= Time.deltaTime;
            }
            if (player1_is_it)
                ++player2_score;
            else
                ++player1_score;
            if (player1_score > player2_score)
            {
                score_1.color = green;
                score_2.color = white;
            }
            else if (player1_score == player2_score)
            {
                score_1.color = white;
                score_2.color = white;
            }
            else
            {
                score_1.color = white;
                score_2.color = green;
            }
            score_1.text = (player1_score / 5).ToString();
            score_2.text = (player2_score / 5).ToString();
        }
        else
        {
            gameOver.color = red;
            gameOver.fontSize = 50;
            gameOver.text = "GAME OVER";
            backToMain.gameObject.SetActive(true);
            playAgain.gameObject.SetActive(true);
        }
	}
}
