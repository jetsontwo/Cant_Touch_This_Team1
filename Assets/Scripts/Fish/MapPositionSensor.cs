using UnityEngine;
using System.Collections;

public class MapPositionSensor : MonoBehaviour
{
    public SpriteRenderer sprite;
    public GameObject shadow;

    public MapForFish map;
    public Player_Movements movements;
    private Vector3 lastPos;
    public int x;
    public int y;
    public int height;
    public float tolerance;
    public float minWaterHeight;
    private float oldWaterHeight;

    // Use this for initialization
    void Start()
    {
        map.GetTile(x, y, map.GetHeightAt(x, y), map.GetWaterHeightAt(x, y), out x, out y);
        this.transform.position = new Vector3(x, y + map.GetHeightAt(x, y) + map.GetWaterHeightAt(x, y), 0);
        lastPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Prevent movement out of bounds
        if (this.transform.position.x < 0)
        {
            this.transform.position = new Vector3(0, this.transform.position.y, 0);
        }
        if (this.transform.position.y < map.GetHeightAt(x, y) + map.GetWaterHeightAt(x, y))
        {
            this.transform.position = new Vector3(this.transform.position.x, map.GetHeightAt(x, y) + map.GetWaterHeightAt(x, y), 0);
        }
        if (this.transform.position.x > map.GetWidth() - 1)
        {
            this.transform.position = new Vector3(map.GetWidth() - 1, this.transform.position.y, 0);
        }
        if (this.transform.position.y > map.GetHeight() + map.GetHeightAt(x, y) + map.GetWaterHeightAt(x, y) - 1)
        {
            this.transform.position = new Vector3(this.transform.position.x, map.GetHeight() + map.GetHeightAt(x, y) + map.GetWaterHeightAt(x, y) - 1, 0);
        }

        int newx, newy;

        // Get tile information from the map
        map.GetTile(this.transform.position.x, this.transform.position.y, height, map.GetWaterHeightAt(x, y), out newx, out newy);

        float newHeight = map.GetHeightAt(newx, newy);// + map.GetWaterHeightAt(newx, newy);
        float oldHeight = map.GetHeightAt(x, y) + map.GetWaterHeightAt(x, y);
            
        if (newHeight - oldHeight < tolerance)
        {
            if (oldHeight - newHeight - map.GetWaterHeightAt(newx, newy) > tolerance)
            {
                movements.falling = true;
            }
            x = newx;
            y = newy;
            height = map.GetHeightAt(newx, newy);
        }
        else if (newHeight - oldHeight > tolerance)
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
            movements.falling = false;
            x = newx;
            y = newy;
            height = map.GetHeightAt(newx, newy);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (map.GetWaterHeightAt(x, y) - oldWaterHeight), 0);
        }

        if (map.GetWaterHeightAt(x, y) > minWaterHeight)
        {
            movements.water_movement = true;
        }
        else
        {
            movements.water_movement = false;
        }

        sprite.sortingOrder = (-y + map.GetHeightAt(x, y) + Mathf.FloorToInt(map.GetWaterHeightAt(x, y))) * map.sortingSubdivisions + 2;
        shadow.GetComponent<SpriteRenderer>().sortingOrder = sprite.sortingOrder - 1;

        shadow.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - map.GetWaterHeightAt(x, y), 0);
        float shadowScale = 1f - map.GetWaterHeightAt(x, y) / (this.transform.position.y - map.GetWaterHeightAt(x, y));
        if (shadowScale < 0.5f)
        {
            shadowScale = 0.5f;
        }
        if (shadowScale > 1)
        {
            shadowScale = 1;
        }
        shadow.transform.localScale = new Vector3(shadowScale, shadowScale, 0);

        lastPos = this.transform.position;
        oldWaterHeight = map.GetWaterHeightAt(x, y);
    }
}
