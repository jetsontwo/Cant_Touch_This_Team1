using UnityEngine;
using System.Collections;

public class Player_Movements : MonoBehaviour {

    public float acceleration, deceleration, max_vel, hop_timer, hop_decrease, hop_speed, threshold;
    public string left_move, right_move, up_move, down_move;
    public Animator am;
    private SpriteRenderer sprite_holder;
    public string last_dir;
    public Sprite[] Idles;

    public bool water_movement, falling, jumping, stop_moving;
    public Vector3 vel, accel;

    private Rigidbody2D rb;
    private float hop_timer_buffer;
    public GameObject sprite_holder_go;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        hop_timer_buffer = hop_timer;
        sprite_holder = sprite_holder_go.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {

        vel = rb.velocity;
        Vector3 moveAccel = new Vector3();

        am.SetBool("in_water", water_movement);

        if (hop_timer > 0)
        {
            hop_timer -= hop_decrease;
        }

        if (!stop_moving) 
        {
            if (Input.GetKey(left_move))
            {
                moveAccel.x -= acceleration;
                am.SetBool("moving_right", false);
                am.SetBool("moving_left", true);
                last_dir = "left";
            }
            if (Input.GetKey(right_move))
            {
                moveAccel.x += acceleration;
                am.SetBool("moving_right", true);
                am.SetBool("moving_left", false);
                last_dir = "right";
            }
            if (Input.GetKey(up_move))
            {
                moveAccel.y += acceleration;
                am.SetBool("moving_up", true);
                am.SetBool("moving_down", false);
                last_dir = "up";
            }
            if (Input.GetKey(down_move))
            {
                moveAccel.y -= acceleration;
                am.SetBool("moving_down", true);
                am.SetBool("moving_up", false);
                last_dir = "down";
            }
        }

        if (falling)
        {
            accel.z = -1;
        }
        else if (jumping)
        {
            accel.z = 0.5f;
        }
        else
        {
            if (moveAccel.magnitude > 0)
            {
                if (!water_movement)
                {
                    if (hop_timer <= 0)
                    {
                        moveAccel = moveAccel.normalized * hop_speed;
                        hop_timer = hop_timer_buffer;
                    }
                }
            }
            else
            {
                accel = vel.normalized * -deceleration;
            }
        }

        // Check map for other velocities, accelerations

        // Apply accelerations to velocity
        moveAccel *= Time.deltaTime;
        accel *= Time.deltaTime;
        vel += moveAccel + accel;

        // Cap 2D velocity to max velocity
        Vector2 velcap = new Vector3(vel.x, vel.y);
        float maxV = (water_movement)? max_vel : hop_speed;
        if (velcap.magnitude > maxV)
        {
            velcap = velcap.normalized * maxV;
            vel.x = velcap.x;
            vel.y = velcap.y;
        }
        
        // Set velocity to zero once it gets low enough
        if (moveAccel.magnitude == 0 && accel.magnitude == 0)
        {
            if (vel.x > -threshold && vel.x < threshold)
            {
                vel.x = 0;
                am.SetBool("moving_right", false);
                am.SetBool("moving_left", false);
            }
            if (vel.y > -threshold && vel.y < threshold)
            {
                vel.y = 0;
                am.SetBool("moving_up", false);
                am.SetBool("moving_down", false);
            }
        }

        // Prevent player from exiting map bounds

        // Apply velocity
        rb.velocity = vel;
        
        //Makes the fish face the last direction of input and turns off the animation controller to keep the fish from returning to its default position
        if (!(am.GetBool("moving_up") || am.GetBool("moving_down") || am.GetBool("moving_left") || am.GetBool("moving_right")))
        {
            Get_Idle();
            sprite_holder_go.transform.position = gameObject.transform.position + new Vector3(0, 0.1f, 0);
        }
        else
        {
            am.enabled = true;
        }
    }



    private void Get_Idle()
    {
        am.enabled = false;
        if (last_dir == "right")
        {
            if (water_movement)
                sprite_holder.sprite = Idles[0];
            else
                sprite_holder.sprite = Idles[4];
        }
        else if (last_dir == "left")
        {
            if (water_movement)
                sprite_holder.sprite = Idles[1];
            else
                sprite_holder.sprite = Idles[5];
        }
        else if (last_dir == "up")
        {
            if (water_movement)
                sprite_holder.sprite = Idles[2];
            else
                sprite_holder.sprite = Idles[6];
        }
        else if (last_dir == "down")
        {
            if (water_movement)
                sprite_holder.sprite = Idles[3];
            
            else
                sprite_holder.sprite = Idles[7];
        }
    }
}
