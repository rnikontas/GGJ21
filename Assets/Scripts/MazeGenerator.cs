using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour
{
    public bool debug;

    [SerializeField] private int testSeed;
    [SerializeField] private int xSize;
    [SerializeField] private int zSize;

    [SerializeField] private GameObject[] wallGO;
    [SerializeField] private GameObject startGO;
    [SerializeField] private GameObject finishGO;

    private Cell[,] cells;

    private int endXGridPos = 0;
    private int endZGridPos = 0;
    private int maxDepth = 0;
    private int currentDepth = 0;

    [SerializeField] private int powerUpsToSpawn;
    [SerializeField] private GameObject[] powerUps;
    
    [SerializeField] private GameObject player;

    [SerializeField] GameObject networkedPlayer;

    void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("GetSeed", RpcTarget.Others, GameManager.instance.seed);
        }
    }

    [PunRPC]
    void GetSeed(int seed)
    {
        Debug.LogError($"Got seed {seed}");
        GameManager.instance.seed = seed;
        Generate();
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            Generate();
        if (debug)
        {
            Generate();
        }
    }

    private void Generate()
    {
        cells = new Cell[xSize, zSize];
        GenerateWalls();
        
        if (PhotonNetwork.IsMasterClient)
            Random.InitState(GameManager.instance.seed);
        else
            Random.InitState(testSeed);
        var xGridPos = Random.Range(0, xSize);
        var zGridPos = Random.Range(0, zSize);

        GenerateGap(xGridPos, zGridPos);
        Instantiate(startGO, new Vector3(cells[xGridPos, zGridPos].xWorldCoordinate, 0, cells[xGridPos, zGridPos].zWorldCoordinate), Quaternion.identity);
        var playerPosition = new Vector3(cells[xGridPos, zGridPos].xWorldCoordinate, 0, cells[xGridPos, zGridPos].zWorldCoordinate);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(networkedPlayer.name, playerPosition, Quaternion.identity);
        }
        else if(debug)
        {
            Instantiate(player, playerPosition, Quaternion.identity);
        }
        Instantiate(finishGO, new Vector3(cells[endXGridPos, endZGridPos].xWorldCoordinate, 0, cells[endXGridPos, endZGridPos].zWorldCoordinate), Quaternion.identity);

        SpawnPowerUps();
    }

    private void GenerateWalls()
    {
        var wallXSize = wallGO[0].transform.GetChild(0).GetComponent<MeshRenderer>().bounds.extents.x * 2;
        var xPos = -0.5f * (xSize - 1) * wallXSize;
        var zPos = 0.5f * (zSize - 1) * wallXSize;
        var distFromCellCenter = wallXSize * 0.5f;

        for (int i = 0; i < zSize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                var wallToGenerate = wallGO[Random.Range(0, wallGO.Length)];
                var cell = new Cell();
                cell.xWorldCoordinate = xPos;
                cell.zWorldCoordinate = zPos;
                cell.xGridCoordinate = j;
                cell.zGridCoordinate = i;
                
                cell.rightWall = Instantiate(wallToGenerate, new Vector3(xPos + distFromCellCenter, 0, zPos),
                    Quaternion.identity * Quaternion.Euler(0, 90, 0));
                cell.bottomWall = Instantiate(wallToGenerate, new Vector3(xPos, 0, zPos - distFromCellCenter),
                    Quaternion.identity);
                if (j == 0)
                {
                    cell.leftWall = Instantiate(wallToGenerate, new Vector3(xPos - distFromCellCenter, 0, zPos),
                        Quaternion.identity * Quaternion.Euler(0, 90, 0));
                }
                else
                {
                    cell.leftWall = cells[j-1, i].rightWall;
                }

                if (i == 0)
                {
                    cell.topWall = Instantiate(wallToGenerate, new Vector3(xPos, 0, zPos + distFromCellCenter),
                        Quaternion.identity);
                }
                else
                {
                    cell.topWall = cells[j, i-1].bottomWall;
                }

                cells[j, i] = cell;
                xPos += wallXSize;
            }

            xPos = -0.5f * (xSize - 1) * wallXSize;
            zPos -= wallXSize;
        }
    }

    private void GenerateGap(int x, int z)
    {
        if (currentDepth > maxDepth)
        {
            maxDepth = currentDepth;
            endXGridPos = x;
            endZGridPos = z;
        }
        
        cells[x, z].visited = true;
        Dictionary<String, Cell> neighbours = new Dictionary<string, Cell>();
        if (x > 0)
        {
            if (!cells[x-1, z].visited)
            {
                neighbours["left"] = cells[x-1, z];
            }
        }
        if (z < zSize - 1)
        {
            if (!cells[x, z + 1].visited)
            {
                neighbours["bottom"] = cells[x, z+1];
            }
        }
        if (x < xSize - 1)
        {
            if (!cells[x+1, z].visited)
            {
                neighbours["right"] = cells[x+1, z];
            }
        }
        if (z > 0)
        {
            if (!cells[x, z-1].visited)
            {
                neighbours["top"] = cells[x, z-1];
            }
        }

        while (neighbours.Count != 0)
        {
            var neighbourToSelect = Random.Range(0, neighbours.Count);
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

            currentDepth++;
            GenerateGap(selectedNeighbour.Value.xGridCoordinate, selectedNeighbour.Value.zGridCoordinate);
            currentDepth--;
            }
            neighbours.Remove(selectedNeighbour.Key);
        }
    }

    private void SpawnPowerUps()
    {
        var powerUpsSpawned = 0;
        while (powerUpsSpawned < powerUpsToSpawn)
        {
            powerUpsSpawned++;
            var powerUpToSpawn = powerUps[Random.Range(0, powerUps.Length)];
            var powerUpGridPosX = Random.Range(0, xSize);
            var powerUpGridPosZ = Random.Range(0, zSize);
            var powerUpWorldPos = new Vector3(cells[powerUpGridPosX, powerUpGridPosZ].xWorldCoordinate, 0, cells[powerUpGridPosX, powerUpGridPosZ].zWorldCoordinate);
            Instantiate(powerUpToSpawn, powerUpWorldPos, Quaternion.identity);
        }
    }
}
