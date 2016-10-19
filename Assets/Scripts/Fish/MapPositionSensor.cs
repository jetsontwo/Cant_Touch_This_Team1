using UnityEngine;
using System.Collections;

public class MapPositionSensor : MonoBehaviour
{
    public SpriteRenderer sprite;
    public GameObject shadow;

    public MapForFish map;
    public Player_Movements movements;
    public int x;
    public int y;
    public float minWaterHeight;
    public float threshold;

    public Vector3 lastPos;
    public float lastWaterDepth;

    // Use this for initialization
    void Start()
    {
        map.GetTile(this.transform.position.x, this.transform.position.y, map.GetHeightAt(x, y), out x, out y);
        lastPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        int newx, newy;

        // Prevent movement out of bounds
        if (this.transform.position.x < 0)
        {
            this.transform.position = new Vector3(0, this.transform.position.y, 0);
        }
        if (this.transform.position.y < map.GetHeightAt(x, y))
        {
            this.transform.position = new Vector3(this.transform.position.x, map.GetHeightAt(x, y), 0);
        }
        if (this.transform.position.x > map.GetWidth() - 1)
        {
            this.transform.position = new Vector3(map.GetWidth() - 1, this.transform.position.y, 0);
        }
        if (this.transform.position.y > map.GetHeight() - 1 + map.GetHeightAt(x, y))
        {
            this.transform.position = new Vector3(this.transform.position.x, map.GetHeight() + map.GetHeightAt(x, y) - 1, 0);
        }

        // Get tile information from the map
        if (!movements.falling)
        {
            map.GetTile(this.transform.position.x, this.transform.position.y, map.GetHeightAt(x, y), out newx, out newy);
            if (map.GetGroundHeightAt(newx, newy) - map.GetHeightAt(x, y) <= threshold)
            {
                if (map.GetGroundHeightAt(newx, newy) < map.GetGroundHeightAt(x, y))
                {
                    movements.falling = true;
                }
                if (x != newx || y != newy)
                {
                    map.ApplyWaterForceAt(newx, newy, 1.2f);
                }
                x = newx;
                y = newy;
                if (map.GetWaterDepthAt(x, y) <= 0)
                {
                    lastWaterDepth = 0;
                }
            }
            else
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
            if (!movements.falling)
            {
                if (map.GetWaterDepthAt(x, y) > minWaterHeight)
                {
                    if (!movements.water_movement)
                    {
                        movements.water_movement = true;
                    }
                }
                else
                {
                    if (movements.water_movement)
                    {
                        movements.water_movement = false;
                    }
                }
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + map.GetWaterDepthAt(x, y) - lastWaterDepth, this.transform.position.z);
            }
        }
        else
        {
            if (this.transform.position.y <= (y + map.GetHeightAt(x, y)))
            {
                this.transform.position = new Vector3(this.transform.position.x, lastPos.y, this.transform.position.z);
                movements.falling = false;
                if (map.GetWaterDepthAt(x, y) > 0)
                {
                    map.ApplyWaterForceAt(x, y, 1.5f);
                }
            }
        }
        sprite.sortingOrder = (-y + Mathf.FloorToInt(map.GetHeightAt(x, y))) * map.sortingSubdivisions + 2;
        shadow.GetComponent<SpriteRenderer>().sortingOrder = sprite.sortingOrder - 1;
        shadow.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - map.GetWaterDepthAt(x, y), 0);
        shadow.transform.localScale = new Vector3(1, 1, 1);
        lastPos = this.transform.position;
        lastWaterDepth = map.GetWaterDepthAt(x, y);
    }
}
