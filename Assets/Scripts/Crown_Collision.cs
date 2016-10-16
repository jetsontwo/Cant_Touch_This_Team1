using UnityEngine;
using System.Collections;

public class Crown_Collision : MonoBehaviour {

    private BoxCollider2D bc;
    private Rigidbody2D rb;
    private float cooldown;
    public GameObject crown_holder;
    public string player_running;

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        cooldown = 0.5f;

    }

    public void Knocked_Off()
    {
        transform.position = transform.parent.position;
        transform.parent = crown_holder.transform;
        transform.rotation = crown_holder.transform.rotation;
        cooldown = .5f;
        bc.enabled = true;
        player_running = "None";

        rb.velocity = new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f));
    }


    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Player" && cooldown <= 0)
        {
            if (c.gameObject.name == "Player_1")
                player_running = "Player_1";
            else
                player_running = "Player_2";
            rb.velocity = Vector3.zero;
            transform.parent = c.gameObject.transform;
            bc.enabled = false;
        }
    }

    void Update()
    {
        cooldown -= Time.deltaTime;
        if(player_running != "None")
            transform.localPosition = new Vector3(0, .75f, 0);
        rb.velocity = new Vector2(rb.velocity.x / 10, rb.velocity.y / 10);
    }
}
