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
    public Crown_Collision cc;
    public Camera mainCamera;
    private Camera_Behavior camBehavior;

    private string player_has_crown = "None";
    private float invincibilityTimer;

    private int player1_score;
    private int player2_score;
    private Color green = new Color(0, 255, 0);
    private Color white = new Color(255, 255, 255);
    private Color red = new Color(255, 0, 0);

    private IEnumerator spinCoroutine;

    private const string PLAYER1 = "Player_1";
    private const string PLAYER2 = "Player_2";
    private const string NONE = "None";

    void Start ()
    {
        invincibilityTimer = 0;
        player1_score = 0;
        player2_score = 0;
        player1rb = player1.GetComponent<Rigidbody2D>();
        player2rb = player2.GetComponent<Rigidbody2D>();

        backToMain.gameObject.SetActive(false);
        playAgain.gameObject.SetActive(false);
        gameTimer.timeLeft = gameMaxTimer;

        stunDuration = 1.1f;
        camBehavior = mainCamera.GetComponent<Camera_Behavior>();
    }
    
    public void NotifyTouched()
    {
        if (player1rb.IsTouching(player2.GetComponent<Collider2D>())) {
            if (invincibilityTimer <= 0 && player_has_crown != NONE) {
                stunPlayer(player_has_crown == PLAYER1 ? player1 : player2,
                           player_has_crown == PLAYER1 ? player2 : player1,
                           stunDuration, 1600);
                invincibilityTimer = invincibilityMaxTimer;
            }
        }

    }

    public void stunPlayer(GameObject stunnedPlayer, GameObject other, float stunTime, float anglesPerSecond) {
        //Stun the player who is not it


        camBehavior.shakeCam = true;

        Vector2 newVelocity = (stunnedPlayer.transform.position - other.transform.position).normalized * pushPower;
        stunnedPlayer.GetComponent<Rigidbody2D>().velocity = newVelocity;

        if (spinCoroutine != null) {
            StopCoroutine(spinCoroutine);
        }
       
        spinCoroutine = spinPlayer(stunnedPlayer, stunTime, anglesPerSecond);
        
        
        StartCoroutine(spinCoroutine);
        
    }

    private IEnumerator spinPlayer(GameObject player, float stunTime, float anglesPerSecond) {
        audioslap.Play();
        if (player_has_crown == player.name) {
            cc.Knocked_Off();
        }
        //float delay = 0.01f;
        int rotateLeftOrRight = Random.Range(-1, 1) >= 0 ? 1 : -1;
        Vector3 spinVector = new Vector3(0, 0, anglesPerSecond * Time.deltaTime * rotateLeftOrRight);

        Player_Movements playerMovements = player.GetComponent<Player_Movements>();
        playerMovements.stop_moving = true;

        while (stunTime > 0) {
            player.transform.Rotate(spinVector);
            stunTime -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        playerMovements.stop_moving = false;
    }
	
	void Update ()
    {
        if ((int)gameTimer.timeLeft > 0)
        {
            NotifyTouched();
            if (invincibilityTimer >= 0)
            {
                invincibilityTimer -= Time.deltaTime;
            }
            player_has_crown = cc.player_has_crown;


            if (player_has_crown.Equals(PLAYER1))
                ++player1_score;
            else if(player_has_crown.Equals(PLAYER2))
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
            if (player1_score > player2_score)
                cc.Put_Crown_On_Player(player1);
            else if (player2_score > player1_score)
                cc.Put_Crown_On_Player(player2);
            else
                cc.Put_Crown_On_Player(null);
            gameOver.color = red;
            gameOver.fontSize = 50;
            gameOver.text = "GAME OVER";
            backToMain.gameObject.SetActive(true);
            playAgain.gameObject.SetActive(true);
        }
	}
}
