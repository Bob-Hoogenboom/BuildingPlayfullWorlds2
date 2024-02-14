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
    [SerializeField] private GameObject gridNode;
    private GameObject[,] grid;

    [SerializeField] private Vector2Int gridSize;

    [Tooltip("Defines the amount of nodes per room")]
    [SerializeField] private int roomSize;

    [Tooltip("Defines the offset for the origin of the grid")]
    [SerializeField] private float gridOffset;

    [Tooltip("Defines the spcae between each Node")] 
    [SerializeField] private float nodeOffset = 4f;


    private void Start()
    {
        GenerateGrid();
    }

    //handles all logic for generating the gird
    public void GenerateGrid()
    {
        //Instantiate gird object
        grid = new GameObject[gridSize.x * roomSize, gridSize.y * roomSize];

        //GameObject Assign Fail
        if(gridNode == null)
        {
            gridNode = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Debug.LogError("DEV CLICHÉ #1: forgot to assign a reference for 'GameObject'");
            return;
        }

        for (int x = 0; x < gridSize.x * roomSize; x++) 
        {
            for (int y = 0; y < gridSize.y * roomSize; y++)
            {
                grid[x, y] = Instantiate(gridNode, new Vector3(x * nodeOffset - gridOffset, 0f, y * nodeOffset - gridOffset),Quaternion.identity);
                grid[x, y].transform.parent = transform;
                grid[x, y].gameObject.name = "GridNode (X-" + x.ToString() + " Y-" + y.ToString() + ")"; 
            }
        }
    }
}
