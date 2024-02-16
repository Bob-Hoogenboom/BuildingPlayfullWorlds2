using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used to hold data about each individual node
/// its position in coordinatres, if it is occupied, etc.
/// </summary>
public class GridNode : MonoBehaviour
{
    private int _coordX;
    private int _coordY;

    //Occupied space variables
    public GameObject objectOnThisNode = null;
    public bool isoccupied = false;

    //Sets this GridNode's coördinates
    public void SetCoords(int x, int y)
    {
        _coordX = x;
        _coordY = y;
    }

    //Gets this GridNode's coördinates
    public Vector2Int GetCoords()
    {
        return new Vector2Int(_coordX, _coordY);
    }
}
