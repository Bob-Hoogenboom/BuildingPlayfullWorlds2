using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Source: https://www.youtube.com/watch?v=4JaHSLA2CKs
/// 
/// This script keeps track of the Grid and all entities on the grid like Enemies, player(s) [and pick-ups*]
/// *not sure about pick ups yet but we'll see
/// </summary>
public class GridManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> rooms;
    Dictionary<Vector2Int, GridNode> grid = new Dictionary<Vector2Int, GridNode>();
    Dictionary<Vector2Int, GridNode> Grid { get { return grid; } }


    public void GenerateGrid()
    {
        for (int i = 0; i < rooms.Count; i++)
        {

            foreach (var gridNode in collection)
            {

            }
        }
        foreach (GameObject room in rooms)
        {
            var gridNodes[] = GameObject.FindGameObjectsWithTag<Node>().GetComponent<GridNode>();

        }
        /*for (int i = 0; i < rooms.Count; i++)
        {
            var gridNodes[] = rooms(i).FindGameObjectsWithTag<Node>().;
            for (int j = 0; j < length; j++)
            {

            }
        }*/
    }
}
