using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Source: https://www.youtube.com/watch?v=qkSSdqOAAl4 
/// 
/// This script keeps track of the Grid and all entities on the grid like Enemies, player(s) [and pick-ups*]
/// *not sure about pick ups yet but we'll see
/// </summary>
public class GridManager : MonoBehaviour
{
    [Tooltip("Scriptable Object with all the statistics to generate the grid over the maze")]
    [SerializeField] private GenerationData genData;

    private void Start()
    {
        GenerateGrid();
    }

    //handles all logic for generating the gird
    public void GenerateGrid()
    {
        //Instantiate gird object
        genData.grid = new GameObject[genData.dungeonSize.x * genData.roomSize, genData.dungeonSize.y * genData.roomSize];

        //GameObject Assign Fail
        if(genData.gridNode == null)
        {
            genData.gridNode = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Debug.LogError("DEV CLICHÉ #1: forgot to assign a reference");
            return;
        }

        for (int x = 0; x < genData.dungeonSize.x * genData.roomSize; x++) 
        {
            for (int y = 0; y < genData.dungeonSize.y * genData.roomSize; y++)
            {
                genData.grid[x, y] = Instantiate(genData.gridNode, new Vector3(x * genData.nodeOffset - genData.gridOffset, 0f, y * genData.nodeOffset - genData.gridOffset),Quaternion.identity);
                genData.grid[x, y].GetComponent<GridNode>().SetCoords(x, y);
                genData.grid[x, y].GetComponent<GridNode>().CheckStartOccupied();
                genData.grid[x, y].transform.parent = transform;
                genData.grid[x, y].gameObject.name = "GridNode (X-" + x.ToString() + " Y-" + y.ToString() + ")"; 
            }
        }
    }

    //Gets the Grid Coördinates of a node via the World Position
    public Vector2Int GetGridPosFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / genData.nodeOffset);
        int y = Mathf.FloorToInt(worldPosition.y / genData.nodeOffset);

        x = Mathf.Clamp(x, 0, genData.dungeonSize.x);
        y = Mathf.Clamp(y, 0, genData.dungeonSize.y);

        return new Vector2Int(x, y);
    }

    //Gets the World Position of a node via the Grid Coördinates
    public Vector3 GetWorldPosFromGrid(Vector2Int gridCoord)
    {
        float x = gridCoord.x * genData.nodeOffset - genData.gridOffset;
        float y = gridCoord.y * genData.nodeOffset - genData.gridOffset;

        return new Vector3(x, 0, y);
    }
}
