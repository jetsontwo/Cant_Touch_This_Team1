using UnityEngine;
using System.Collections;

public class Crown_Collision : MonoBehaviour {

    private BoxCollider2D bc;
    private Rigidbody2D rb;
    private float cooldown;
    public GameObject crown_holder;
    public string player_has_crown;

    private const string PLAYER1 = "Player_1";
    private const string PLAYER2 = "Player_2";
    private const string NONE = "None";

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        player_has_crown = NONE;
        cooldown = 0.5f;

    }

    public void Knocked_Off()
    {
        transform.position = transform.parent.position;
        transform.parent = crown_holder.transform;
        transform.rotation = crown_holder.transform.rotation;
        cooldown = .5f;
        bc.enabled = true;
        player_has_crown = NONE;

        rb.velocity = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
    }


    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Player" && cooldown <= 0)
        {
            if (c.gameObject.name == PLAYER1)
                player_has_crown = PLAYER1;
            else
                player_has_crown = PLAYER2;
            rb.velocity = Vector3.zero;
            transform.parent = c.gameObject.transform;
            bc.enabled = false;
        }
    }

    void Update()
    {
        print(rb.velocity);
        cooldown -= Time.deltaTime;
        if (player_has_crown != NONE)
            transform.localPosition = new Vector3(0, .75f, 0);
        rb.velocity -= new Vector2(rb.velocity.x, rb.velocity.y) * Time.deltaTime;
        if(rb.velocity.magnitude < 0.5)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void Put_Crown_On_Player(GameObject winner)
    {
        transform.parent = winner.transform;
        transform.rotation = Quaternion.identity;
        if (winner.name == PLAYER1)
            player_has_crown = PLAYER1;
        else
            player_has_crown = PLAYER2;
    }
}
