using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Source: https://www.youtube.com/watch?v=gHU5RQWbmWE&t=323s
/// 
/// This script generatres the dungeon via the Depth First Search (or BackTracking) algorythm
/// It starts at a random position, then checks the neighbours of all directions
/// If at least 'one' of the neighbouring tiles has not been generated it proceeds down that path
/// Otherwise it will backtrack and checks the neighbouring cells again to see if it can generate another path.
/// Rinse and repeat until its back at the starting path
/// </summary>


public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    [SerializeField] private Vector2 size;
    [SerializeField] private int startPos = 0;
    [SerializeField] private GameObject dungeonRoom;
    [SerializeField] private Vector2 roomOffset;

    List<Cell> dungeon;


    private void Start()
    {
        MazeGenerator();
    }

    private void GenerateDungeon()
    {
        for(int i =0; i < size.x; i++)
        {
            for(int j = 0; j < size.y; j++)
            {
                var newRoom = Instantiate(dungeonRoom, new Vector3(i * roomOffset.x, 0f , j * roomOffset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                newRoom.UpdateRoom(dungeon[Mathf.FloorToInt(i + j * size.x)].status);

                newRoom.name += " " + i + " - " + j;
            }
        }
        
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

        //K is not a good naming convention*
        int k = 0;

        while (k < 1000)
        {
            k++;
            dungeon[currentCell].visited = true;

            List<int> neighbours = CheckNeighbours(currentCell);

            //checks if the maze is finished generating
            //# reverse if for cleaner code?
            if(neighbours.Count == 0)
            {
                if(path.Count == 0)
                {
                    break;
                } 
                else
                {
                    //research path.pop*
                    currentCell = path.Pop();
                }
            }
            else
            {
                //research path.push
                path.Push(currentCell);

                int newCell = neighbours[Random.Range(0, neighbours.Count)];

                if(newCell > currentCell)
                {
                    //# Dont Repeat Yourself (DRY)?

                    //SouthV or East>
                    if(newCell - 1 == currentCell)
                    {
                        dungeon[currentCell].status[2] = true;
                        currentCell = newCell;
                        dungeon[currentCell].status[3] = true;
                    }
                    else
                    {
                        dungeon[currentCell].status[1] = true;
                        currentCell = newCell;
                        dungeon[currentCell].status[0] = true;
                    }
                }
                else
                {
                    //North^ or West<
                    if (newCell + 1 == currentCell)
                    {
                        dungeon[currentCell].status[3] = true;
                        currentCell = newCell;
                        dungeon[currentCell].status[2] = true;
                    }
                    else
                    {
                        dungeon[currentCell].status[0] = true;
                        currentCell = newCell;
                        dungeon[currentCell].status[1] = true;
                    }
                }

            }
        }
        GenerateDungeon();
    }

    private List<int> CheckNeighbours(int cell)
    { 
        List<int> neighbours = new List<int>();

        //# Dont Repeat Yourself (DRY)?

        //NORTH ^
        if (cell - size.x >= 0 && !dungeon[Mathf.FloorToInt(cell-size.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell -size.x));
        }

        //EAST > 
        //checks if the generatoir is at the most east side of the dungeon
        if ((cell + 1) % size.x != 0 && !dungeon[Mathf.FloorToInt(cell + 1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + 1));
        }

        //SOUTH V
        if (cell + size.x < dungeon.Count && !dungeon[Mathf.FloorToInt(cell + size.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + size.x));
        }

        //WEST < 
        //checks if the generatoir is at the most west side of the dungeon
        if (cell % size.x != 0 && !dungeon[Mathf.FloorToInt(cell - 1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbours;
    }
}
