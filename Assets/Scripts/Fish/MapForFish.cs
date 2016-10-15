using UnityEngine;
using System.Collections;

public class MapForFish : MonoBehaviour {

    private int[,] heights = new int[,]{{1, 1, 1, 1, 1, 1, 1, 1},
                                        {1, 1, 1, 1, 1, 1, 1, 1},
                                        {1, 1, 1, 1, 1, 1, 1, 1},
                                        {1, 1, 1, 1, 1, 1, 1, 1},
                                        {1, 1, 1, 1, 1, 1, 1, 1},
                                        {1, 1, 1, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 0, 0, 0, 0},
                                        {0, 0, 0, 0, 0, 0, 0, 0}};

    /* 1 = Grass
     * 0 = Sand
     */

    private int[,] tiles = new int[,]{{1, 1, 1, 1, 1, 1, 1, 1},
                                      {1, 1, 1, 1, 1, 1, 1, 1},
                                      {1, 1, 1, 1, 1, 1, 1, 1},
                                      {1, 1, 1, 1, 1, 1, 1, 1},
                                      {1, 1, 1, 1, 1, 1, 1, 1},
                                      {1, 1, 1, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0},
                                      {0, 0, 0, 0, 0, 0, 0, 0}};

    public int GetWidth()
    {
        return heights.GetLength(0);
    }

    public int GetHeight()
    {
        return heights.GetLength(1);
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

    public int heightBetween(int x0, int y0, int x1, int y1)
    {
        return heights[x1, y1] - heights[x0, y0];
    }
}
