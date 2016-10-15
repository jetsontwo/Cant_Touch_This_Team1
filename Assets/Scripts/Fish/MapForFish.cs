using UnityEngine;
using System.Collections;

public enum State
{
    Ground,
    Water,
    Air
}

public class MapForFish : MonoBehaviour {
    
    public Sprite[] boardSprites;
    public Sprite wallSprite;

    private int[,] heights = new int[,]{{1, 1, 1, 1, 1, 1, 1, 3},
                                        {1, 0, 1, 1, 1, 1, 1, 1},
                                        {1, 0, 1, 1, 1, 1, 1, 1},
                                        {0, 0, 1, 1, 1, 1, 1, 1},
                                        {0, 0, 1, 1, 1, 1, 1, 1},
                                        {0, 0, 1, 1, 1, 1, 1, 1},
                                        {0, 0, 1, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 0, 0, 0, 0}};

    /* 1 = Grass
     * 0 = Sand
     */

    private int[,] tiles = new int[,]{{0, 0, 0, 0, 0, 0, 0, 1},
                                      {0, 1, 0, 0, 0, 0, 0, 0},
                                      {0, 1, 0, 0, 0, 0, 0, 0},
                                      {1, 1, 0, 0, 0, 0, 0, 0},
                                      {1, 1, 0, 0, 0, 0, 0, 0},
                                      {1, 1, 0, 0, 0, 0, 0, 0},
                                      {1, 1, 0, 1, 1, 1, 1, 1},
                                      {1, 1, 1, 1, 1, 1, 1, 1}};

    void Start()
    {
        for (int i = 0; i < tiles.GetLength(0); ++i)
        {
            for (int j = 0; j < tiles.GetLength(1); ++j)
            {
                Vector2 position = new Vector2(i, j + heights[i, j]);
                for (int k = 1; k <= heights[i, j]; ++k)
                {
                    Vector2 wallPosition = new Vector2(position.x, position.y - k);
                    GameObject wallInstance = new GameObject();
                    wallInstance.AddComponent<SpriteRenderer>().sprite = wallSprite;
                    wallInstance.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y * 2);
                    wallInstance.transform.position = wallPosition;
                    wallInstance.transform.parent = this.transform;
                }
                GameObject tileInstance = new GameObject();
                tileInstance.AddComponent<SpriteRenderer>().sprite = boardSprites[tiles[i, j]];
                tileInstance.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y * 2);
                tileInstance.transform.position = position;
                tileInstance.transform.parent = this.transform;
            }
        }
    }

    public void GetTile(float screenx, float screeny, int height, out int x, out int y, out int newheight)
    {
        x = Mathf.RoundToInt(screenx);
        y = Mathf.RoundToInt(screeny) - height;
        newheight = heights[x, y];
    }

    public int GetHeightAt(int x, int y)
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
}
