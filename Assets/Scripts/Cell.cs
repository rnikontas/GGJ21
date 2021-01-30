using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public GameObject topWall;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject bottomWall;

    public bool visited;

    public int xGridCoordinate;
    public int zGridCoordinate;

    public float xWorldCoordinate;
    public float zWorldCoordinate;

    public Cell()
    {
        visited = false;
    }
    
}
