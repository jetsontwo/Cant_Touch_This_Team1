using UnityEngine;
using System.Collections;

public class FishMovement : MonoBehaviour
{
    public int x, y, longJumpRange;

    private float keyTimer;
    private MapForFish map;
    private FishInputManager input;

    // Use this for initialization
    void Start()
    {
        keyTimer = 0;
        map = FindObjectOfType<MapForFish>();
        input = GetComponent<FishInputManager>();
    }

    // Update is called once per frame
    // @TODO: Animations should play regardless of whether player can go to the next space or not.
    void Update()
    {
        switch (input.ConsumeCommand())
        {
            case (Command.UP):
                y = map.GetValidY(x, y, y - 1, 0);
                break;
            case (Command.DOWN):
                y = map.GetValidY(x, y, y + 1, 0);
                break;
            case (Command.LEFT):
                x = map.GetValidX(x, y, x - 1, 0);
                break;
            case (Command.RIGHT):
                x = map.GetValidX(x, y, x + 1, 0);
                break;
        }
        Debug.Log(x + ", " + y);
    }
}
