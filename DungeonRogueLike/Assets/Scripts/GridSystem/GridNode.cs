using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used to hold data about each individual node
/// its position in coordinatres, if it is occupied, etc.
/// </summary>
public class GridNode : MonoBehaviour
{
    //debugging
    [SerializeField] private Renderer _renderer;
        
    //variables
    private LayerMask _gridStatic;
    private int _coordX;
    private int _coordY;

    //Occupied space variables
    public GameObject objectOnThisNode = null;
    public bool isOccupied = false;

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

    //checks on start if a static object is on top of the node and marks this node as forever occupied
    public void CheckStaticOccupied()
    {
        _gridStatic = LayerMask.GetMask("GridStatic");
        if(Physics.CheckSphere(transform.position, 1f, _gridStatic)) 
        {
            isOccupied = true;
            _renderer.material.color = Color.red;
        }
    }

    public void GridOccupation(bool occupied)
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(1,1,1));         
    }
}
