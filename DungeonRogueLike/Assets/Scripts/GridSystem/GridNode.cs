using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used to hold data about each individual node
/// its position in coordinatres, if it is occupied, etc.
/// </summary>
public class GridNode : MonoBehaviour
{
    //variables
    [SerializeField] private new Renderer renderer;
    [SerializeField] private Material glowMaterial;
    [SerializeField] private Material unselectedMaterial;
    [SerializeField] private int _coordX;
    [SerializeField] private int _coordY;

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
    public void CheckStartOccupied()
    {
        var gridStaticMask = LayerMask.GetMask("GridStatic");
        var unitMask = LayerMask.GetMask("Unit");
 
        RaycastHit hit;

        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z), Vector3.up, out hit, 1f, gridStaticMask))
        {
            Debug.Log("Hit gridStaticMask object!");
            Destroy(gameObject);
        }
        else if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z), Vector3.up, out hit, 1f, unitMask))
        {
            Debug.Log("Hit unitMask object!");
            SetOccupation(hit.transform.gameObject, true);

            //Make redcat detect its first gridnode instead of the gridnode detecting redcat
            if (hit.transform.GetComponent<PlayerController>())
            {
                var PlayerCont = hit.transform.GetComponent<PlayerController>();
                PlayerCont.SetFirstGridNode(this);
            }

            if (hit.transform.GetComponent<Enemy>())
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                enemy._currentGridNode = this;
            }
        }
    }

    public void SetOccupation(GameObject obj, bool occupied)
    {
        isOccupied = occupied;

        if (isOccupied)
        {
            objectOnThisNode = obj;
            return;
        }

        objectOnThisNode = null;
    }

    public void ToggleGlow(bool glowState)
    {
        if (glowState)
        {
            renderer.material = glowMaterial;
            //activate glow
            return;
        }
        renderer.material = unselectedMaterial;
        //deactivate glow
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(1,1,1));         
    }
}
