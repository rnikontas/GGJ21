using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private int xSize;
    [SerializeField] private int zSize;

    [SerializeField] private GameObject wallGO;

    private Cell[,] cells;

    void Start()
    {
        cells = new Cell[xSize, zSize];
        GenerateWalls();
        
        GenerateGap(Random.Range(0, xSize-1), Random.Range(0, zSize-1));
    }

    private void GenerateWalls()
    {
        var wallXSize = wallGO.transform.localScale.x;
        var xPos = -0.5f * (xSize - 1) * wallXSize;
        var zPos = 0.5f * (zSize - 1) * wallXSize;
        var distFromCellCenter = wallXSize * 0.5f;

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < zSize; j++)
            {
                var cell = new Cell();
                cell.xGridCoordinate = i;
                cell.zGridCoordinate = j;
                
                cell.rightWall = Instantiate(wallGO, new Vector3(xPos + distFromCellCenter, 0, zPos),
                    Quaternion.identity * Quaternion.Euler(0, 90, 0));
                cell.bottomWall = Instantiate(wallGO, new Vector3(xPos, 0, zPos - distFromCellCenter),
                    Quaternion.identity);
                if (j == 0)
                {
                    cell.leftWall = Instantiate(wallGO, new Vector3(xPos - distFromCellCenter, 0, zPos),
                        Quaternion.identity * Quaternion.Euler(0, 90, 0));
                }
                else
                {
                    cell.leftWall = cells[i, j - 1].rightWall;
                }

                if (i == 0)
                {
                    cell.topWall = Instantiate(wallGO, new Vector3(xPos, 0, zPos + distFromCellCenter),
                        Quaternion.identity);
                }
                else
                {
                    cell.topWall = cells[i - 1, j].bottomWall;
                }

                cells[i, j] = cell;
                xPos += wallXSize;
            }

            xPos = -0.5f * (xSize - 1) * wallXSize;
            zPos -= wallXSize;
        }
    }

    private void GenerateGap(int x, int z)
    {
        cells[x, z].visited = true;
        Dictionary<String, Cell> neighbours = new Dictionary<string, Cell>();
        if (x > 0)
        {
            if (!cells[x-1, z].visited)
            {
                neighbours["top"] = cells[x-1, z];
            }
        }
        if (z < zSize - 1)
        {
            if (!cells[x, z + 1].visited)
            {
                neighbours["right"] = cells[x, z+1];
            }
        }
        if (x < xSize - 1)
        {
            if (!cells[x+1, z].visited)
            {
                neighbours["bottom"] = cells[x+1, z];
            }
        }
        if (z > 0)
        {
            if (!cells[x, z-1].visited)
            {
                neighbours["left"] = cells[x, z-1];
            }
        }

        while (neighbours.Count != 0)
        {
            var neighbourToSelect = Random.Range(0, neighbours.Count - 1);
            var selectedNeighbour = neighbours.ElementAt(neighbourToSelect);
            if (!selectedNeighbour.Value.visited)
            {
            switch (selectedNeighbour.Key)
            {
                case "left":
                    GameObject.Destroy(cells[x, z].leftWall);
                    break;
                case "right":
                    GameObject.Destroy(cells[x, z].rightWall);
                    break;
                case "top":
                    GameObject.Destroy(cells[x, z].topWall);
                    break;
                case "bottom":
                    GameObject.Destroy(cells[x, z].bottomWall);
                    break;
            }
            GenerateGap(selectedNeighbour.Value.xGridCoordinate, selectedNeighbour.Value.zGridCoordinate);
            }
            neighbours.Remove(selectedNeighbour.Key);
        }
    }
}
