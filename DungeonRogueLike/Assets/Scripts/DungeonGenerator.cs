using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Source: https://www.youtube.com/watch?v=gHU5RQWbmWE&t=323s
/// </summary>

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    public Vector2 size;
    public int startPos = 0;

    List<Cell> dungeon;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MazeGenerator()
    {
        dungeon = new List<Cell>();

        for(int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                dungeon.Add(new Cell());
            }
        }

        int currentCell = startPos;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while (k < 1000)
        {
            k++;
            dungeon[currentCell].visited = true;

            //check cells neighbour
        }
    }

    private List<int> CheckNeighbours(int cell)
    {
        List<int> neighbours = new List<int>();

        //check north neighbour
        //check east neighbour
        //check south neighbour
        //check west neighbour

        return neighbours;
    }
}
