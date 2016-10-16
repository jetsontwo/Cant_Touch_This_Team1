using UnityEngine;
using System.Collections;

public class MapPositionSensor : MonoBehaviour
{
    public SpriteRenderer sprite;

    private MapForFish map;
    private Player_Movements movements;
    private Vector3 lastPos;
    public int x;
    public int y;
    public int height;

    // Use this for initialization
    void Start()
    {
        map = FindObjectOfType<MapForFish>();
        map.GetTile(this.transform.position.x, this.transform.position.y, map.GetHeightAt(x, y), out x, out y, out height);
        movements = GetComponent<Player_Movements>();
        lastPos = this.transform.position;
    }

    // Update is called once per frame
    // @TODO: Animations should play regardless of whether player can go to the next space or not.
    void Update()
    {
        int newheight, newx, newy;
        
        // Prevent movement out of bounds
        if (this.transform.position.x < 0)
        {
            this.transform.position = new Vector3(0, this.transform.position.y, 0);
        }
        if (this.transform.position.y < height)
        {
            this.transform.position = new Vector3(this.transform.position.x, height, 0);
        }
        if (this.transform.position.x > map.GetWidth() - 1)
        {
            this.transform.position = new Vector3(map.GetWidth() - 1, this.transform.position.y, 0);
        }
        if (this.transform.position.y > map.GetHeight() + height - 1)
        {
            this.transform.position = new Vector3(this.transform.position.x, map.GetHeight() + height - 1, 0);
        }

        // Get tile information from the map
        if (!movements.falling)
        {
            map.GetTile(this.transform.position.x, this.transform.position.y, height, out newx, out newy, out newheight);
            if (newheight < height)
            {
                movements.falling = true;
                x = newx;
                y = newy;
                height = newheight;
            }
            else if (!movements.falling && newheight > height)
            {
                if (newy != y)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
                    this.transform.position = new Vector3(this.transform.position.x, lastPos.y, 0);
                }
                if (newx != x)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
                    this.transform.position = new Vector3(lastPos.x, this.transform.position.y, 0);
                }
            }
            else
            {
                x = newx;
                y = newy;
            }
        }
        else
        {
            if (this.transform.position.y <= (y + height))
            {
                movements.falling = false;
            }
        }
        sprite.sortingOrder = -(y * 2) + map.GetHeightAt(x, y)*2 + 1;
        lastPos = this.transform.position;
    }
}
