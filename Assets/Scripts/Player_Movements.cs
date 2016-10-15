using UnityEngine;
using System.Collections;

public class Player_Movements : MonoBehaviour {

    public float acceleration, max_vel, hop_timer, hop_decrease, hop_speed;
    private Rigidbody2D rb;
    public string left_move, right_move, up_move, down_move;
    public Animator am;
    public bool water_movement, falling;
    private float hop_timer_buffer;
    private bool stop_falling;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        hop_timer_buffer = hop_timer;
    }
	
	// Update is called once per frame
	void Update () {
        int horiz_move = 0;
        int vert_move = 0;

        if (Input.GetKey(left_move))
        {
            horiz_move -= 1;
        }
        if (Input.GetKey(right_move))
        {
            horiz_move += 1;
        }

        if (Input.GetKey(up_move))
        {
            vert_move += 1;
        }
        if (Input.GetKey(down_move))
        {
            vert_move -= 1;
        }

        if (!falling)
        {
            if (stop_falling)
                rb.velocity = Vector2.zero;
            if (water_movement)
            {
                if ((horiz_move != 0 || vert_move != 0) && rb.velocity.magnitude <= max_vel)
                {
                    rb.velocity += new Vector2(horiz_move * acceleration, vert_move * acceleration);
                }
                else
                {
                    rb.velocity -= new Vector2(rb.velocity.x / 10, rb.velocity.y / 10);
                }
            }
            else
            {
                if ((horiz_move != 0 || vert_move != 0) && rb.velocity.magnitude <= max_vel && hop_timer <= 0)
                {
                    print("hi");
                    rb.velocity += new Vector2(horiz_move * acceleration * hop_speed, vert_move * acceleration * hop_speed);
                    hop_timer = hop_timer_buffer;
                }
                else
                {
                    rb.velocity -= new Vector2(rb.velocity.x / 10, rb.velocity.y / 10);
                }
                hop_timer -= hop_decrease;
            }
        }
        else
        {
            rb.velocity -= new Vector2(rb.velocity.x / 20, 1);
            stop_falling = true;
        }
        


        if (horiz_move != 0 || vert_move != 0)
        {
            if(horiz_move > 0)
            {
                am.SetBool("moving_right", true);
                am.SetBool("moving_left", false);
            }
            else if (horiz_move < 0)
            {
                am.SetBool("moving_left", true);
                am.SetBool("moving_right", false);
            }


            if(vert_move > 0)
            {
                am.SetBool("moving_up", true);
                am.SetBool("moving_down", false);
            }
            else if(vert_move < 0)
            {
                am.SetBool("moving_down", true);
                am.SetBool("moving_up", false);
            }

        }
        else
        {
            am.SetBool("moving_left", false);
            am.SetBool("moving_right", false);
            am.SetBool("moving_up", false);
            am.SetBool("moving_down", false);
        }
    }
}
