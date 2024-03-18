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
        public GameObject enemy;
    }

    [Tooltip("Scriptable Object with all the statistics to generate the maze")]
    [SerializeField] private GenerationData genData;

    private bool bossHasSpawned = false;

    List<Cell> dungeon;

    //this function is set to Awake() because it is the first thing that shgould happen before anything else!
    private void Awake()
    {
        MazeGenerator();
    }

    private void GenerateDungeon()
    {
        for(int i =0; i < genData.dungeonSize.x; i++)
        {
            for(int j = 0; j < genData.dungeonSize.y; j++)
            {
                var newRoom = Instantiate(genData.dungeonRoom, new Vector3(i * genData.roomOffset.x, 0f , j * genData.roomOffset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                newRoom.UpdateRoom(dungeon[Mathf.FloorToInt(i + j * genData.dungeonSize.x)].status);

                if(dungeon[Mathf.FloorToInt(i + j * genData.dungeonSize.x)].enemy != null){
                    newRoom.SpawnEnemy(dungeon[Mathf.FloorToInt(i + j * genData.dungeonSize.x)].enemy);
                }

                newRoom.name += " " + i + " - " + j;
            }
        }
    }

    private void MazeGenerator()
    {
        dungeon = new List<Cell>();

        for(int i = 0; i < genData.dungeonSize.x; i++)
        {
            for (int j = 0; j < genData.dungeonSize.y; j++)
            {
                dungeon.Add(new Cell());
            }
        }

        int currentCell = genData.startPos;

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
                    //Done generating, place Redcat[Player]
                    break;
                } 
                else
                {
                    dungeon[currentCell].enemy = SetEnemy();
                    //Stack.pop() returns the first value in the stack
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
        if (cell - genData.dungeonSize.x >= 0 && !dungeon[Mathf.FloorToInt(cell-genData.dungeonSize.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell -genData.dungeonSize.x));
        }

        //EAST > 
        //checks if the generatoir is at the most east side of the dungeon
        if ((cell + 1) % genData.dungeonSize.x != 0 && !dungeon[Mathf.FloorToInt(cell + 1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + 1));
        }

        //SOUTH V
        if (cell + genData.dungeonSize.x < dungeon.Count && !dungeon[Mathf.FloorToInt(cell + genData.dungeonSize.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + genData.dungeonSize.x));
        }

        //WEST < 
        //checks if the generatoir is at the most west side of the dungeon
        if (cell % genData.dungeonSize.x != 0 && !dungeon[Mathf.FloorToInt(cell - 1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbours;
    }

    
    //returns a boss first and then returns enemies after the boss
    private GameObject SetEnemy()
    {
        if (!bossHasSpawned)
        {
            bossHasSpawned = true;
            return genData.levelBoss;
        }

        //generate a random number inside the bounds of the list of enemies.length to spawn a random enemy
        var rando = Random.Range(0, genData.enemies.Length);
        return genData.enemies[rando];
    }
}
