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
    public float waterheight;
    public float minWaterHeight;

    // Use this for initialization
    void Start()
    {
        map.GetTile(this.transform.position.x, this.transform.position.y, map.GetHeightAt(x, y), map.GetWaterHeightAt(x, y), out x, out y, out height, out waterheight);
        lastPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        int newheight, newx, newy;
        float newwaterheight;

        // Prevent movement out of bounds
        if (this.transform.position.x < 0)
        {
            this.transform.position = new Vector3(0, this.transform.position.y, 0);
        }
        if (this.transform.position.y < height + waterheight)
        {
            this.transform.position = new Vector3(this.transform.position.x, height + waterheight, 0);
        }
        if (this.transform.position.x > map.GetWidth() - 1)
        {
            this.transform.position = new Vector3(map.GetWidth() - 1, this.transform.position.y, 0);
        }
        if (this.transform.position.y > map.GetHeight() + height + waterheight - 1)
        {
            this.transform.position = new Vector3(this.transform.position.x, map.GetHeight() + height + waterheight - 1, 0);
        }

        // Get tile information from the map
        if (!movements.falling)
        {
            map.GetTile(this.transform.position.x, this.transform.position.y, height, waterheight, out newx, out newy, out newheight, out newwaterheight);
            if (newheight + Mathf.FloorToInt(newwaterheight) < height + Mathf.FloorToInt(waterheight) + 0.8f)
            {
                movements.falling = true;
                x = newx;
                y = newy;
                height = newheight;
                waterheight = newwaterheight;
            }
            else if (newheight + Mathf.FloorToInt(newwaterheight) > height + Mathf.FloorToInt(waterheight) + 0.8f)
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
                waterheight = newwaterheight;
                height = newheight;
            }

            if (waterheight > minWaterHeight)
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

            if (movements.water_movement)
            {
                sprite.sortingOrder = (-y + map.GetHeightAt(x, y) + Mathf.FloorToInt(map.GetWaterHeightAt(x, y))) * map.sortingSubdivisions + 2;
            }
            else
            {
                sprite.sortingOrder = (-y + map.GetHeightAt(x, y)) * map.sortingSubdivisions + 2;
            }
            shadow.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - waterheight, 0);
            float shadowScale = 0.2f + Mathf.Pow(0.5f, waterheight);
            shadow.transform.localScale = new Vector3(shadowScale, shadowScale, 0);
        }
        else
        {
            if (this.transform.position.y <= (y + height + waterheight))
            {
                movements.falling = false;
            }
            shadow.transform.position = new Vector3(this.transform.position.x, y + height, 0);
            float shadowScale = 1f - (this.transform.position.y - y - height);
            if (shadowScale < 0.5f)
            {
                shadowScale = 0.5f;
            }
            shadow.transform.localScale = new Vector3(shadowScale, shadowScale, 0);
        }
        shadow.GetComponent<SpriteRenderer>().sortingOrder = sprite.sortingOrder - 1;
        lastPos = this.transform.position;
    }
}
