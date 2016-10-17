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
    public Sprite wallTopSprite;
    public Sprite[] wallSideSprites;
    public Sprite[] waterSprites;
    public Material spriteMat;
    public int sortingSubdivisions;
    public float waterVolume;
    public Vector2[] initialWaterArea;
    public float spriteMaxTimer;

    private int waterArea;
    public float spriteTimer = 0;
    private float[,] waterHeights = new float[8, 8];
    private GameObject[,] waterTiles = new GameObject[8, 8];
    private int[,] waterTileSprites = new int[8, 8];
    private bool switchFirsts = true;
    private float max_height = 2;

    private int[,] heights = new int[,]{{0, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 1, 1, 1, 1, 0, 0},
                                        {0, 0, 1, 1, 1, 1, 0, 0}};

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
                float shade = 0.8f + 0.2f * heights[i, j] / max_height;
                tileInstance.GetComponent<SpriteRenderer>().color = new Color(shade, shade, shade);
                tileInstance.transform.position = position;
                tileInstance.transform.parent = this.transform;
                tileInstance.name = "tile: " + i + ", " + j;
                for (int k = 1; k <= heights[i, j]; ++k)
                {
                    Vector2 wallPosition = new Vector2(position.x, position.y - k);
                    GameObject wallInstance = new GameObject();
                    if (k == 1)
                    {
                        wallInstance.AddComponent<SpriteRenderer>().sprite = wallTopSprite;
                    }
                    else
                    {
                        int rand = Random.Range(-1, wallSideSprites.Length);
                        if (rand < 0)
                        {
                            rand = 0;
                        }
                        wallInstance.AddComponent<SpriteRenderer>().sprite = wallSideSprites[rand];
                    }
                    wallInstance.GetComponent<SpriteRenderer>().sortingOrder = (-(int)j + heights[i, j]) * sortingSubdivisions;
                    wallInstance.GetComponent<SpriteRenderer>().material = spriteMat;
                    shade = 0.8f + 0.2f * k / max_height;
                    tileInstance.GetComponent<SpriteRenderer>().color = new Color(shade, shade, shade);
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
                waterTileSprites[i, j] = j % waterSprites.Length;
                waterInstance.AddComponent<SpriteRenderer>().sprite = waterSprites[waterTileSprites[i, j]];
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
        bool switchSprite = false;
        if (spriteTimer > spriteMaxTimer)
        {
            switchSprite = true;
            spriteTimer = 0;
            switchFirsts = !switchFirsts;
        }
        else
        {
            spriteTimer += Time.deltaTime;
        }
        for (int i = 0; i < waterTiles.GetLength(0); ++i)
        {
            for (int j = 0; j < waterTiles.GetLength(1); ++j)
            {
                if (switchSprite)
                {
                    if ((switchFirsts && (i + j) % 2 == 0) || (!switchFirsts && (i + j) % 2 != 0))
                    {
                        waterTileSprites[i, j] = (waterTileSprites[i, j] + 1) % waterSprites.Length;
                    }
                }
                if (waterTiles[i, j].GetComponent<SpriteRenderer>().enabled)
                {
                    if (waterHeights[i, j] <= 0)
                    {
                        waterTiles[i, j].GetComponent<SpriteRenderer>().enabled = false;
                    }
                    waterTiles[i, j].GetComponent<SpriteRenderer>().sprite = waterSprites[waterTileSprites[i, j]];
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
        if (x < 0 || x > tiles.GetLength(0) || y < 0 || y > tiles.GetLength(1))
        {
            Debug.Log("Trying to access: " + x + ", " + y);
        }
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
