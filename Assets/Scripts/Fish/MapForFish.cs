using UnityEngine;
using System.Collections;

public class MapForFish : MonoBehaviour {
    
    public Sprite[] boardSprites;
    public Sprite wallSprite;

    private int[,] heights = new int[,]{{0, 0, 1, 1, 1, 1, 1, 3},
                                        {0, 0, 1, 1, 1, 1, 1, 1},
                                        {0, 0, 1, 1, 1, 1, 1, 1},
                                        {0, 0, 1, 1, 1, 1, 1, 1},
                                        {0, 0, 1, 1, 1, 1, 1, 1},
                                        {0, 0, 1, 1, 1, 1, 1, 1},
                                        {0, 0, 1, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 0, 0, 0, 0}};

    /* 1 = Grass
     * 0 = Sand
     */

    private int[,] tiles = new int[,]{{1, 1, 0, 0, 0, 0, 0, 1},
                                      {1, 1, 0, 0, 0, 0, 0, 0},
                                      {1, 1, 0, 0, 0, 0, 0, 0},
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

    public int GetValidX(int oldx, int y, int newx, int maxHeight)
    {
        int retx = newx;
        if (newx < 0)
        {
            retx = 0;
        }
        if (newx >= tiles.GetLength(0))
        {
            retx = tiles.GetLength(0) - 1;
        }
        // Get the maximum distance a player can go without running into something they can't jump over.
        while (retx != oldx && heights[retx, y] - heights[oldx, y] > maxHeight)
        {
            retx = retx + ((retx < oldx) ? 1 : -1);
        }
        return retx;
    }

    public int GetValidY(int x, int oldy, int newy, int maxHeight)
    {
        int rety = newy;
        if (newy < 0)
        {
            rety = 0;
        }
        if (newy >= tiles.GetLength(1))
        {
            rety = tiles.GetLength(1) - 1;
        }
        // Get the maximum distance a player can go without running into something they can't jump over.
        while (rety != oldy && heights[x, rety] - heights[x, oldy] > maxHeight)
        {
            rety = rety + ((rety < oldy) ? 1 : -1);
        }
        return rety;
    }

    public int GetHeightAt(int x, int y)
    {
        return heights[x, y];
    }
}
