using UnityEngine;
using System.Collections;

public enum Command
{
    NONE,
    UP, DOWN, LEFT, RIGHT,
    HIGH_UP, HIGH_DOWN, HIGH_LEFT, HIGH_RIGHT,
    LONG_UP, LONG_DOWN, LONG_LEFT, LONG_RIGHT
}

public class FishInputManager : MonoBehaviour
{
    public string left_move, right_move, up_move, down_move;
    public float doubleTapMaxTimer;

    private float doubleTapTimer;
    private string currentKey;
    private bool doubleTapped;
    private bool shortTap;
    private Command lastCommand;

    void Start()
    {
        doubleTapTimer = 0;
        doubleTapped = false;
        shortTap = false;
        lastCommand = Command.NONE;
    }

    // Update is called once per frame
    void Update ()
    {
        if (doubleTapTimer > 0)
        {
            doubleTapTimer -= Time.deltaTime;
        }
        // Sense keys once double-tap timer expired
        if (doubleTapTimer <= 0)
        {
            // New keypresses
            if (currentKey == null)
            {
                if (Input.GetKeyDown(left_move))
                {
                    currentKey = left_move;
                    doubleTapTimer = doubleTapMaxTimer;
                }
                if (Input.GetKeyDown(right_move))
                {
                    currentKey = right_move;
                    doubleTapTimer = doubleTapMaxTimer;
                }
                if (Input.GetKeyDown(up_move))
                {
                    currentKey = up_move;
                    doubleTapTimer = doubleTapMaxTimer;
                }
                if (Input.GetKeyDown(down_move))
                {
                    currentKey = down_move;
                    doubleTapTimer = doubleTapMaxTimer;
                }
            }
            // Sense results after double-tap window expires
            else
            {
                if (doubleTapped)
                {
                    if (currentKey == left_move)
                    {
                        lastCommand = Command.HIGH_LEFT;
                    }
                    if (currentKey == right_move)
                    {
                        lastCommand = Command.LONG_RIGHT;
                    }
                    if (currentKey == up_move)
                    {
                        lastCommand = Command.LONG_UP;
                    }
                    if (currentKey == down_move)
                    {
                        lastCommand = Command.LONG_DOWN;
                    }
                }
                // If they didn't double-tap ...
                else
                {
                    // See if they did a short tap
                    // Short tap = regular flopping.
                    if (shortTap)
                    {
                        if (currentKey == left_move)
                        {
                            lastCommand = Command.LEFT;
                        }
                        if (currentKey == right_move)
                        {
                            lastCommand = Command.RIGHT;
                        }
                        if (currentKey == up_move)
                        {
                            lastCommand = Command.UP;
                        }
                        if (currentKey == down_move)
                        {
                            lastCommand = Command.DOWN;
                        }
                    }
                    // See if they did a long press
                    // Long press = high jump.
                    else
                    {
                        if (currentKey == left_move)
                        {
                            lastCommand = Command.HIGH_LEFT;
                        }
                        if (currentKey == right_move)
                        {
                            lastCommand = Command.HIGH_RIGHT;
                        }
                        if (currentKey == up_move)
                        {
                            lastCommand = Command.HIGH_UP;
                        }
                        if (currentKey == down_move)
                        {
                            lastCommand = Command.HIGH_DOWN;
                        }
                    }
                }
                doubleTapped = false;
                shortTap = false;
                currentKey = null;
            }
        }
        // Sense keys inside the double-tap time window
        else
        {
            if (Input.GetKeyDown(currentKey))
            {
                if (shortTap)
                {
                    doubleTapped = true;
                }
            }
            else if (Input.GetKeyUp(currentKey))
            {
                shortTap = true;
            }
        }
    }

    public Command ConsumeCommand()
    {
        if (doubleTapTimer <= 0)
        {
            Command ret = lastCommand;
            lastCommand = Command.NONE;
            return ret;
        }
        return Command.NONE;
    }
}
