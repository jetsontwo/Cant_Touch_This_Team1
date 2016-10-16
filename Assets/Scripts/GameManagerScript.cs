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
    public GameObject player1;
    public GameObject player2;
    private Rigidbody2D player1rb, player2rb;
    private float stunDuration;
    public int pushPower;
    public AudioSource audioslap;

    private bool player1_is_running = false;
    private float invincibilityTimer;

    private int player1_score;
    private int player2_score;
    private Color green = new Color(0, 255, 0);
    private Color white = new Color(255, 255, 255);
    private Color red = new Color(255, 0, 0);

    private IEnumerator spinCoroutine;

	void Start ()
    {
        invincibilityTimer = 0;
        player1_score = 0;
        player2_score = 0;
        player1rb = player1.GetComponent<Rigidbody2D>();
        player2rb = player2.GetComponent<Rigidbody2D>();

        backToMain.gameObject.SetActive(false);
        playAgain.gameObject.SetActive(false);
        if (Random.value > 0.5)
        {
            player1_is_running = true;
        }
        gameTimer.timeLeft = gameMaxTimer;

        stunDuration = 1.25f;
    }
    
    public void NotifyTouched()
    {
        if (player1rb.IsTouching(player2.GetComponent<Collider2D>())) {
            if (invincibilityTimer <= 0) {
                stunPlayer();
                player1_is_running = !player1_is_running;
                invincibilityTimer = invincibilityMaxTimer;
            }
        }

    }

    public void stunPlayer() {
        //Stun the player who is not it
        GameObject stunnedPlayer = player1_is_running ? player1 : player2;
        GameObject otherPlayer = player1_is_running ? player2 : player1;

        Vector2 newVelocity = (stunnedPlayer.transform.position - otherPlayer.transform.position).normalized * pushPower;
        stunnedPlayer.GetComponent<Rigidbody2D>().velocity = newVelocity;

        spinCoroutine = spinPlayer(1300, stunnedPlayer);
        StartCoroutine(spinCoroutine);
        
    }

    private IEnumerator spinPlayer(float anglesPerSecond, GameObject player) {
        audioslap.Play();
        float delay = 0.05f;
        float stunTimeLeft = stunDuration;
        int rotateLeftOrRight = Random.Range(-1, 1) >= 0 ? 1 : -1;
        Vector3 spinVector = new Vector3(0, 0, anglesPerSecond * delay * rotateLeftOrRight);

        Player_Movements playerMovements = player.GetComponent<Player_Movements>();
        playerMovements.stop_moving = true;

        while (stunTimeLeft > 0) {
            player.transform.Rotate(spinVector);
            stunTimeLeft -= delay;
            yield return new WaitForSeconds(delay);
        }
        player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        playerMovements.stop_moving = false;
    }
	
	void Update ()
    {
        print(player1_is_running);
        if ((int)gameTimer.timeLeft > 0)
        {
            NotifyTouched();
            if (invincibilityTimer >= 0)
            {
                invincibilityTimer -= Time.deltaTime;
            }
            if (player1_is_running)
                ++player1_score;
            else
                ++player2_score;
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
