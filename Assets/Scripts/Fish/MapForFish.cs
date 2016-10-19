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
    public Sprite waterSideSprite;
    public Material spriteMat;
    public int sortingSubdivisions;
    public float waterVolume;
    public float wavePropagationSpeed;  // Should be < 1/waterSimulationResolution
    public float spriteMaxTimer;
    
    public float spriteTimer = 0;
    private float[,] waterHeights = new float[16, 16];
    private float[,] waterVelocities = new float[16, 16];
    private GameObject[,] waterTiles = new GameObject[16, 16];
    private GameObject[,] waterSides = new GameObject[16, 16];
    private int[,] waterTileSprites = new int[16, 16];
    private bool switchFirsts = true;
    private float max_height = 2;
    private float initialWaterHeight;

    private int[,] heights = new int[,]{{0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 2, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
                                        {0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 2, 2, 0},
                                        {0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 2, 2, 0},
                                        {0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0},
                                        {0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 2, 0, 0},
                                        {0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};

    private int[,] tiles = new int[,]{{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1},
                                      {0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1},
                                      {0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};

    void Start()
    {
        for (int i = 0; i < tiles.GetLength(0); ++i)
        {
            for (int j = 0; j < tiles.GetLength(1); ++j)
            {
                Vector2 position = new Vector2(i, j + heights[i, j]);
                GameObject tileInstance = new GameObject();
                tileInstance.AddComponent<SpriteRenderer>().sprite = boardSprites[tiles[i, j]];
                tileInstance.GetComponent<SpriteRenderer>().sortingOrder = (-j + heights[i, j]) * sortingSubdivisions;
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
                    wallInstance.GetComponent<SpriteRenderer>().sortingOrder = (-(j + k) + heights[i, j]) * sortingSubdivisions;
                    wallInstance.GetComponent<SpriteRenderer>().material = spriteMat;
                    shade = 0.8f + 0.2f * k / max_height;
                    tileInstance.GetComponent<SpriteRenderer>().color = new Color(shade, shade, shade);
                    wallInstance.transform.position = wallPosition;
                    wallInstance.transform.parent = this.transform;
                    wallInstance.name = "wall: " + i + ", " + (j + k);
                }
            }
        }
        initialWaterHeight = waterVolume / (waterTiles.GetLength(0) * waterTiles.GetLength(1));
        for (int i = 0; i < waterHeights.GetLength(0); ++i)
        {
            for (int j = 0; j < waterHeights.GetLength(1); ++j)
            {
                waterHeights[i, j] = initialWaterHeight;
            }
        }
        for (int i = 0; i < waterTiles.GetLength(0); ++i)
        {
            for (int j = 0; j < waterTiles.GetLength(1); ++j)
            {
                GameObject waterInstance = new GameObject();
                waterTileSprites[i, j] = j % waterSprites.Length;
                waterInstance.AddComponent<SpriteRenderer>().sprite = waterSprites[waterTileSprites[i, j]];
                waterInstance.GetComponent<SpriteRenderer>().material = spriteMat;
                waterTiles[i, j] = waterInstance;
                waterInstance.name = "water: " + i + ", " + j;
                waterInstance.transform.parent = this.transform;
                
                GameObject waterSideInstance = new GameObject();
                waterSideInstance.AddComponent<SpriteRenderer>().sprite = waterSideSprite;
                waterSideInstance.GetComponent<SpriteRenderer>().material = spriteMat;
                waterSideInstance.transform.parent = this.transform;
                waterSideInstance.name = "water side: " + i + ", " + j;
                waterSides[i, j] = waterSideInstance;
            }
        }
        /*
        waterHeights[0, 0] += 0.2f;
        waterHeights[waterHeights.GetLength(0) - 1, 0] += 0.2f;
        waterHeights[0, waterHeights.GetLength(1) - 1] += 0.2f;
        waterHeights[waterHeights.GetLength(0) - 1, waterHeights.GetLength(1) - 1] += 0.2f;*/
    }

    void Update()
    {
        UpdateWater();
        UpdateWaterSprites();
    }

    void UpdateWater()
    {
        float[,] tempWaterHeights = new float[waterHeights.GetLength(0), waterHeights.GetLength(1)];
        for (int i = 0; i < waterHeights.GetLength(0); ++i)
        {
            for (int j = 0; j < waterHeights.GetLength(1); ++j)
            {
                if (waterHeights[i, j] < heights[i, j])
                {
                    waterVelocities[i, j] = 0;
                }
                float leftHeight = waterHeights[i, j];
                float rightHeight = leftHeight;
                float upHeight = leftHeight;
                float downHeight = leftHeight;
                if (i - 1 >= 0)
                {
                    leftHeight = waterHeights[i - 1, j];
                }
                if (i + 1 <= waterHeights.GetLength(0) - 1)
                {
                    rightHeight = waterHeights[i + 1, j];
                }
                if (j + 1 <= waterHeights.GetLength(1) - 1)
                {
                    upHeight = waterHeights[i, j + 1];
                }
                if (j - 1 >= 0)
                {
                    downHeight = waterHeights[i, j - 1];
                }
                float force = wavePropagationSpeed * wavePropagationSpeed * (leftHeight + rightHeight + upHeight + downHeight - 4 * waterHeights[i, j]);
                waterVelocities[i, j] += force * Time.deltaTime;
                waterVelocities[i, j] *= 0.999f;
                tempWaterHeights[i, j] = waterHeights[i, j] + waterVelocities[i, j] * Time.deltaTime;
                if (Mathf.Abs(waterVelocities[i, j]) < 0.01f)
                {
                    tempWaterHeights[i, j] = initialWaterHeight;
                }
            }
        }
        for (int i = 0; i < waterHeights.GetLength(0); ++i)
        {
            for (int j = 0; j < waterHeights.GetLength(1); ++j)
            {
                waterHeights[i, j] = tempWaterHeights[i, j];
            }
        }
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
                waterTiles[i, j].GetComponent<SpriteRenderer>().enabled = waterHeights[i, j] > heights[i, j];
                if (waterTiles[i, j].GetComponent<SpriteRenderer>().enabled)
                {
                    waterTiles[i, j].GetComponent<SpriteRenderer>().sprite = waterSprites[waterTileSprites[i, j]];
                    waterTiles[i, j].GetComponent<SpriteRenderer>().sortingOrder = (-(int)j + Mathf.RoundToInt(waterHeights[i, j])) * sortingSubdivisions;
                    waterTiles[i, j].transform.position = new Vector2(i, j + waterHeights[i, j]);
                    waterSides[i, j].GetComponent<SpriteRenderer>().sortingOrder = (-j + Mathf.RoundToInt(waterHeights[i, j])) * sortingSubdivisions;
                    waterSides[i, j].transform.position = new Vector2(i, j + waterHeights[i, j] - 1);
                    waterTiles[i, j].GetComponent<SpriteRenderer>().sortingOrder = waterTiles[i, j].GetComponent<SpriteRenderer>().sortingOrder;
                }
            }
        }
    }

    public void GetTile(float screenx, float screeny, float height, out int x, out int y)
    {
        x = Mathf.RoundToInt(screenx);
        y = Mathf.RoundToInt(screeny - height);
    }

    public float GetHeightAt(int x, int y)
    {
        if (heights[x, y] > waterHeights[x, y])
        {
            return heights[x, y];
        }
        return waterHeights[x, y];
    }

    public float GetWaterDepthAt(int x, int y)
    {
        if (heights[x, y] > waterHeights[x, y])
        {
            return 0;
        }
        return waterHeights[x, y] - heights[x, y];
    }

    public float GetGroundHeightAt(int x, int y)
    {
        return heights[x, y];
    }

    public int GetWidth()
    {
        return tiles.GetLength(0);
    }

    public int GetHeight()
    {
        return tiles.GetLength(1);
    }

    public void ApplyWaterForceAt(int x, int y, float force)
    {
        waterHeights[x, y] = waterHeights[x, y] / force;
    }
}

