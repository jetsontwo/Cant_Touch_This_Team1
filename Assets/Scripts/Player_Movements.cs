using UnityEngine;
using System.Collections;

public class Player_Movements : MonoBehaviour {

    public float speed, max_vel;
    private Rigidbody2D rb;
    public string left_move, right_move, up_move, down_move;
    

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        int horiz_move = 0;
        int vert_move = 0;

        if (Input.GetKey(left_move))
        {
            horiz_move += 1;
        }
        if (Input.GetKey(right_move))
        {
            horiz_move -= 1;
        }

        if (Input.GetKey(up_move))
        {
            vert_move += 1;
        }
        if (Input.GetKey(down_move))
        {
            vert_move -= 1;
        }

        if (horiz_move != 0 && rb.velocity.magnitude <= max_vel)
        {
            rb.AddForce(new Vector2(horiz_move * speed, vert_move * speed));
        }
	}
}
