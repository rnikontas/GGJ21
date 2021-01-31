using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Photon.Pun;
using Unity.Jobs;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.Collections;
using UnityEngine.AI;

public class MazeGenerator : MonoBehaviour
{
    public bool debug;

    [SerializeField] private int testSeed;
    [SerializeField] private int xSize;
    [SerializeField] private int zSize;

    [SerializeField] private GameObject[] wallGO;
    [SerializeField] private GameObject floorGO;
    [SerializeField] private GameObject startGO;
    [SerializeField] private GameObject finishGO;
    [SerializeField] private GameObject torch;
    public GameObject blackOut;
    
    [SerializeField] private int torchGap;
                     private int currentTorchGap = 0;

    private Cell[,] cells;

    private int endXGridPos = 0;
    private int endZGridPos = 0;
    private int maxDepth = 0;
    private int currentDepth = 0;

    [SerializeField] private int powerUpsToSpawn;
    [SerializeField] private GameObject[] powerUps;
    
    [SerializeField] private GameObject player;

    [SerializeField] GameObject networkedPlayer;

    [SerializeField] private int extraWallsToRemove;
        
    private List<GameObject> floorList = new List<GameObject>();

    [SerializeField] private GameObject biblico;
    [SerializeField] private GameObject stompus;
    [SerializeField] private GameObject stratus;

    void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("GetSeed", RpcTarget.Others, GameManager.Instance.seed);
        }
    }

    [PunRPC]
    void GetSeed(int seed)
    {
        Debug.LogError($"Got seed {seed}");
        GameManager.Instance.seed = seed;
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
            Random.InitState(GameManager.Instance.seed);
        else if (debug)
        {
            Random.InitState(testSeed);
        }
        else
        {
            Random.InitState(GameManager.Instance.seed);
        }
        var xGridPos = Random.Range(0, xSize);
        var zGridPos = Random.Range(0, zSize);
        GenerateGap(xGridPos, zGridPos);
        Instantiate(startGO, new Vector3(cells[xGridPos, zGridPos].xWorldCoordinate, 2, cells[xGridPos, zGridPos].zWorldCoordinate), Quaternion.identity);
        CheckAndSetOccupied(cells[xGridPos, zGridPos]);
        var playerPosition = new Vector3(cells[xGridPos, zGridPos].xWorldCoordinate, 2, cells[xGridPos, zGridPos].zWorldCoordinate);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(networkedPlayer.name, playerPosition, Quaternion.identity);
        }
        else if(debug)
        {
            Instantiate(player, playerPosition, Quaternion.identity);
        }
        Instantiate(finishGO, new Vector3(cells[endXGridPos, endZGridPos].xWorldCoordinate, 2, cells[endXGridPos, endZGridPos].zWorldCoordinate), Quaternion.identity);
        CheckAndSetOccupied(cells[endXGridPos, endZGridPos]);
        RemoveExtraWalls();
        SpawnPowerUps();
        //BuildingNavMesh();
        //DropEnemies();
    }

    private void DropEnemies()
    {
        var biblicoxGridPos = Random.Range(0, xSize);
        var biblicozGridPos = Random.Range(0, zSize);
        Instantiate(biblico, new Vector3(cells[biblicoxGridPos, biblicozGridPos].xWorldCoordinate, 2, cells[biblicoxGridPos, biblicozGridPos].zWorldCoordinate), Quaternion.identity);

        var stompusxGridPos = Random.Range(0, xSize);
        var stompuszGridPos = Random.Range(0, zSize);
        Instantiate(stompus, new Vector3(cells[stompusxGridPos, stompuszGridPos].xWorldCoordinate, 2, cells[stompusxGridPos, stompuszGridPos].zWorldCoordinate), Quaternion.identity);

        var stratusxGridPos = Random.Range(0, xSize);
        var stratuszGridPos = Random.Range(0, zSize);
        Instantiate(stratus, new Vector3(cells[stratusxGridPos, stratuszGridPos].xWorldCoordinate, 2, cells[stratusxGridPos, stratuszGridPos].zWorldCoordinate), Quaternion.identity);
    }

    private void BuildingNavMesh()
    {
        foreach (var floor in floorList)
        {
            floor.GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }

    private void GenerateWalls()
    {
        var wallXSize = wallGO[0].GetComponent<MeshRenderer>().bounds.extents.x * 2;
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

                var tempFloor = Instantiate(floorGO, new Vector3(xPos, 0.2f, zPos), Quaternion.identity);
                floorList.Add(tempFloor);
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

        Dictionary<String, Cell> neighbours = GetUnvisitedNeighbours(x, z);
        
        while (neighbours.Count != 0)
        {
            var neighbourToSelect = Random.Range(0, neighbours.Count);
            var selectedNeighbour = neighbours.ElementAt(neighbourToSelect);
            if (!selectedNeighbour.Value.visited)
            {
                RemoveWall(selectedNeighbour.Key, x, z);
                if (currentTorchGap == torchGap)
                {
                    currentTorchGap = 0;
                    AddTorch(cells[x, z]);
                }
                else currentTorchGap++;

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
            var powerUpToSpawn = powerUps[Random.Range(0, powerUps.Length)];
            var powerUpGridPosX = Random.Range(0, xSize);
            var powerUpGridPosZ = Random.Range(0, zSize);
            if (!CheckAndSetOccupied(cells[powerUpGridPosX, powerUpGridPosZ]))
            {
                var powerUpWorldPos = new Vector3(cells[powerUpGridPosX, powerUpGridPosZ].xWorldCoordinate, 2,
                    cells[powerUpGridPosX, powerUpGridPosZ].zWorldCoordinate);
                Instantiate(powerUpToSpawn, powerUpWorldPos, Quaternion.identity);
                powerUpsSpawned++;
            }
        }
    }

    private void AddTorch(Cell cell)
    {
        if (cell.bottomWall != null)
        {
            cell.torch = Instantiate(torch, new Vector3(cell.xWorldCoordinate, 2, cell.zWorldCoordinate - 0.775f), Quaternion.Euler(20, 0,0));
        }
        else if (cell.leftWall != null)
        {
            cell.torch = Instantiate(torch, new Vector3(cell.xWorldCoordinate - 0.775f, 2, cell.zWorldCoordinate), Quaternion.Euler(0, 0,-20));
        }
        else if (cell.topWall != null)
        {
            cell.torch = Instantiate(torch, new Vector3(cell.xWorldCoordinate, 2, cell.zWorldCoordinate + 0.775f), Quaternion.Euler(-20, 0,0));
        }
        else if (cell.rightWall != null)
        {
            cell.torch = Instantiate(torch, new Vector3(cell.xWorldCoordinate + 0.775f, 2, cell.zWorldCoordinate), Quaternion.Euler(0, 0,20));
        }
    }

    private void RemoveExtraWalls()
    {
        for (int i = 0; i < extraWallsToRemove; i++)
        {
            var x = Random.Range(0, xSize);
            var z = Random.Range(0, zSize);
            var neighbours = GetNeighboursWithWalls(x, z);
            
            if (neighbours.Count != 0)
            {
                var neighbourToSelect = Random.Range(0, neighbours.Count);
                var selectedNeighbour = neighbours.ElementAt(neighbourToSelect);
                RemoveWall(selectedNeighbour.Key, x, z);
            }
            else
            {
                i--;
            }
        }
    }

    private void RemoveWall(string wallToRemove, int x, int z)
    {
        switch (wallToRemove)
        {
            case "left":
                GameObject.Destroy(cells[x, z].leftWall);
                cells[x, z].leftWall = null;
                cells[x - 1, z].rightWall = null;
                RemoveTorch(x, z);
                RemoveTorch(x-1, z);
                break;
            case "right":
                GameObject.Destroy(cells[x, z].rightWall);
                cells[x, z].rightWall = null;
                cells[x + 1, z].leftWall = null;
                RemoveTorch(x, z);
                RemoveTorch(x+1, z);
                break;
            case "top":
                GameObject.Destroy(cells[x, z].topWall);
                cells[x, z].topWall = null;
                cells[x, z-1].bottomWall = null;
                RemoveTorch(x, z);
                RemoveTorch(x, z-1);
                break;
            case "bottom":
                GameObject.Destroy(cells[x, z].bottomWall);
                cells[x, z].bottomWall = null;
                cells[x, z+1].topWall = null;
                RemoveTorch(x, z);
                RemoveTorch(x, z+1);
                break;
        }
        

    }

    private Dictionary<String, Cell> GetUnvisitedNeighbours(int x, int z)
    {
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

        return neighbours;
    }
    
    private Dictionary<String, Cell> GetNeighboursWithWalls(int x, int z)
    {
        Dictionary<String, Cell> neighbours = new Dictionary<string, Cell>();
        if (x > 0)
        {
            if (cells[x, z].leftWall != null)
            {
                neighbours["left"] = cells[x-1, z];
            }
        }
        if (z < zSize - 1)
        {
            if (cells[x, z].bottomWall != null)
            {
                neighbours["bottom"] = cells[x, z+1];
            }
        }
        if (x < xSize - 1)
        {
            if (cells[x, z].rightWall != null)
            {
                neighbours["right"] = cells[x+1, z];
            }
        }
        if (z > 0)
        {
            if (cells[x, z].topWall != null)
            {
                neighbours["top"] = cells[x, z-1];
            }
        }

        return neighbours;
    }

    private bool CheckAndSetOccupied(Cell cell)
    {
        if (cell.isOccupied)
            return true;
        else
        {
            cell.isOccupied = true;
            return false;
        }
    }

    private void RemoveTorch(int x, int z)
    {
        if (cells[x, z].torch != null)
        {
            Destroy(cells[x, z].torch);
            cells[x, z].torch = null;
        }
    }
}
