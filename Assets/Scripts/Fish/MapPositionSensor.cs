﻿using UnityEngine;
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
    public float minWaterHeight;
    private float oldWaterHeight;

    // Use this for initialization
    void Start()
    {
        map.GetTile(this.transform.position.x, this.transform.position.y, map.GetHeightAt(x, y), map.GetWaterHeightAt(x, y), out x, out y);
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

        // Get tile information from the map
        if (!movements.falling)
        {
            map.GetTile(this.transform.position.x, this.transform.position.y, map.GetHeightAt(x, y), map.GetWaterHeightAt(x, y), out newx, out newy);
            if (map.GetHeightAt(newx, newy) + Mathf.FloorToInt(map.GetWaterHeightAt(newx, newy)) < map.GetHeightAt(x, y) + Mathf.FloorToInt(map.GetWaterHeightAt(x, y)))
            {
                movements.falling = true;
                x = newx;
                y = newy;
            }
            else if (map.GetHeightAt(newx, newy) + Mathf.FloorToInt(map.GetWaterHeightAt(newx, newy)) > map.GetHeightAt(x, y) + Mathf.FloorToInt(map.GetWaterHeightAt(x, y)))
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
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (map.GetWaterHeightAt(x, y) - oldWaterHeight), 0);
                oldWaterHeight = map.GetWaterHeightAt(x, y);
            }

            if (map.GetWaterHeightAt(x, y) > minWaterHeight)
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
            shadow.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - map.GetWaterHeightAt(x, y), 0);
            float shadowScale = 0.2f + Mathf.Pow(0.5f, map.GetWaterHeightAt(x, y));
            shadow.transform.localScale = new Vector3(shadowScale, shadowScale, 0);
        }
        else
        {
            if (this.transform.position.y <= (y + map.GetHeightAt(x, y) + map.GetWaterHeightAt(x, y)))
            {
                movements.falling = false;
            }
            shadow.transform.position = new Vector3(this.transform.position.x, y + map.GetHeightAt(x, y), 0);
            float shadowScale = 1f - (this.transform.position.y - y - map.GetHeightAt(x, y));
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
