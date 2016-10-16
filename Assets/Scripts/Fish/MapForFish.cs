using UnityEngine;
using System.Collections.Generic;

public enum State
{
    Ground,
    Water,
    Air
}

public class MapForFish : MonoBehaviour {
    
    public Sprite[] boardSprites;
    public Sprite wallSprite;
    public Sprite waterSprite;
    public Material spriteMat;
    public int sortingSubdivisions;
    public float waterVolume;
    public Vector2[] initialWaterArea;

    private int waterArea;
    private float[,] waterHeights = new float[8, 8];
    private GameObject[,] waterTiles = new GameObject[8, 8];

    private int[,] heights = new int[,]{{0, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 1, 1, 1, 1, 0, 0},
                                        {0, 0, 1, 2, 2, 1, 0, 0},
                                        {0, 0, 1, 2, 2, 1, 0, 0},
                                        {0, 0, 1, 1, 1, 1, 0, 0},
                                        {0, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 0, 0, 0, 0}};

    private int[,] tiles = new int[,]{{0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0}};

    void Start()
    {
        for (int i = 0; i < tiles.GetLength(0); ++i)
        {
            for (int j = 0; j < tiles.GetLength(1); ++j)
            {
                Vector2 position = new Vector2(i, j + heights[i, j]);
                GameObject tileInstance = new GameObject();
                tileInstance.AddComponent<SpriteRenderer>().sprite = boardSprites[tiles[i, j]];
                tileInstance.GetComponent<SpriteRenderer>().sortingOrder = (-(int)j + heights[i, j]) * sortingSubdivisions;
                tileInstance.GetComponent<SpriteRenderer>().material = spriteMat;
                tileInstance.transform.position = position;
                tileInstance.transform.parent = this.transform;
                tileInstance.name = "tile: " + i + ", " + j;
                for (int k = 1; k <= heights[i, j]; ++k)
                {
                    Vector2 wallPosition = new Vector2(position.x, position.y - k);
                    GameObject wallInstance = new GameObject();
                    wallInstance.AddComponent<SpriteRenderer>().sprite = wallSprite;
                    wallInstance.GetComponent<SpriteRenderer>().sortingOrder = (-(int)j + heights[i, j]) * sortingSubdivisions;
                    wallInstance.GetComponent<SpriteRenderer>().material = spriteMat;
                    wallInstance.transform.position = wallPosition;
                    wallInstance.transform.parent = this.transform;
                    wallInstance.name = "wall: " + i + ", " + (j + k);
                }
            }
        }
        waterArea = initialWaterArea.Length;
        for (int i = 0; i < waterTiles.GetLength(0); ++i)
        {
            for (int j = 0; j < waterTiles.GetLength(1); ++j)
            {
                waterHeights[i, j] = 0;
                GameObject waterInstance = new GameObject();
                waterInstance.AddComponent<SpriteRenderer>().sprite = waterSprite;
                waterInstance.GetComponent<SpriteRenderer>().material = spriteMat;
                waterTiles[i, j] = waterInstance;
                waterInstance.name = "water: " + i + ", " + j;
                waterInstance.transform.parent = this.transform;
            }
        }
        for (int i = 0; i < initialWaterArea.Length; ++i)
        {
            waterHeights[(int)initialWaterArea[i].x, (int)initialWaterArea[i].y] = waterVolume / initialWaterArea.Length;
        }
        UpdateWaterSprites();
    }

    void Update()
    {
        HashSet<Vector2> newWaterArea = new HashSet<Vector2>();

        // Spread water out
        for (int i = 0; i < waterHeights.GetLength(0); ++i)
        {
            for (int j = 0; j < waterHeights.GetLength(1); ++j)
            {
                // check right
                if (waterHeights[i, j] > 0 && i < waterHeights.GetLength(0) - 1 && heights[i + 1, j] + waterHeights[i + 1, j] < heights[i, j] + waterHeights[i, j])
                {
                    if (waterHeights[i + 1, j] == 0)
                    {
                        ++waterArea;
                    }
                    float diff = (waterHeights[i, j] + heights[i, j] + waterHeights[i + 1, j] + heights[i + 1, j]) / 2f;
                    waterHeights[i + 1, j] = diff - heights[i + 1, j];
                    waterHeights[i, j] = diff - heights[i, j];
                    if (waterHeights[i, j] <= 0)
                    {
                        waterHeights[i, j] = 0;
                        --waterArea;
                    }
                    if (waterHeights[i + 1, j] <= 0)
                    {
                        waterHeights[i + 1, j] = 0;
                        --waterArea;
                    }
                }
                // check left
                if (waterHeights[i, j] > 0 && i > 0 && heights[i - 1, j] + waterHeights[i - 1, j] < heights[i, j] + waterHeights[i, j])
                {
                    if (waterHeights[i - 1, j] == 0)
                    {
                        ++waterArea;
                    }
                    float diff = (waterHeights[i, j] + heights[i, j] + waterHeights[i - 1, j] + heights[i - 1, j]) / 2f;
                    waterHeights[i - 1, j] = diff - heights[i - 1, j];
                    waterHeights[i, j] = diff - heights[i, j];
                    if (waterHeights[i, j] <= 0)
                    {
                        waterHeights[i, j] = 0;
                        --waterArea;
                    }
                    if (waterHeights[i - 1, j] <= 0)
                    {
                        waterHeights[i - 1, j] = 0;
                        --waterArea;
                    }
                }
                // check up
                if (waterHeights[i, j] > 0 && j < waterHeights.GetLength(1) - 1 && heights[i, j + 1] + waterHeights[i, j + 1] < heights[i, j] + waterHeights[i, j])
                {
                    if (waterHeights[i, j + 1] == 0)
                    {
                        ++waterArea;
                    }
                    float diff = (waterHeights[i, j] + heights[i, j] + waterHeights[i, j + 1] + heights[i, j + 1]) / 2f;
                    waterHeights[i, j + 1] = diff - heights[i, j + 1];
                    waterHeights[i, j] = diff - heights[i, j];
                    if (waterHeights[i, j] <= 0)
                    {
                        waterHeights[i, j] = 0;
                        --waterArea;
                    }
                    if (waterHeights[i, j + 1] <= 0)
                    {
                        waterHeights[i, j + 1] = 0;
                        --waterArea;
                    }
                }
                // check down
                if (waterHeights[i, j] > 0 && j > 0 && heights[i, j - 1] + waterHeights[i, j - 1] < heights[i, j] + waterHeights[i, j])
                {
                    if (waterHeights[i, j - 1] == 0)
                    {
                        ++waterArea;
                    }
                    float diff = (waterHeights[i, j] + heights[i, j] + waterHeights[i, j - 1] + heights[i, j - 1]) / 2f;
                    waterHeights[i, j - 1] = diff - heights[i, j - 1];
                    waterHeights[i, j] = diff - heights[i, j];
                    if (waterHeights[i, j] <= 0)
                    {
                        waterHeights[i, j] = 0;
                        --waterArea;
                    }
                    if (waterHeights[i, j - 1] <= 0)
                    {
                        waterHeights[i, j - 1] = 0;
                        --waterArea;
                    }
                }
            }
        }
        // Update water sprites
        UpdateWaterSprites();
    }

    void UpdateWaterSprites()
    {
        for (int i = 0; i < waterTiles.GetLength(0); ++i)
        {
            for (int j = 0; j < waterTiles.GetLength(1); ++j)
            {
                if (waterTiles[i, j].GetComponent<SpriteRenderer>().enabled)
                {
                    if (waterHeights[i, j] <= 0)
                    {
                        waterTiles[i, j].GetComponent<SpriteRenderer>().enabled = false;
                    }
                    waterTiles[i, j].GetComponent<SpriteRenderer>().sortingOrder = (-(int)j + heights[i, j] + Mathf.CeilToInt(waterHeights[i, j])) * sortingSubdivisions;
                    waterTiles[i, j].transform.position = new Vector2(i, j + heights[i, j] + waterHeights[i, j]);
                }
                else
                {
                    if (waterHeights[i, j] > 0)
                    {
                        waterTiles[i, j].GetComponent<SpriteRenderer>().enabled = true;
                    }
                }

            }
        }
    }

    public void GetTile(float screenx, float screeny, int height, float waterHeight, out int x, out int y, out int newheight, out float newWaterHeight)
    {
        x = Mathf.RoundToInt(screenx);
        y = Mathf.RoundToInt(screeny - height - waterHeight);
        newheight = heights[x, y];
        newWaterHeight = waterHeights[x, y];
    }

    public int GetHeightAt(int x, int y)
    {
        return heights[x, y];
    }

    public float GetWaterHeightAt(int x, int y)
    {
        return waterHeights[x, y];
    }

    public int GetWidth()
    {
        return tiles.GetLength(0);
    }

    public int GetHeight()
    {
        return tiles.GetLength(1);
    }
}
