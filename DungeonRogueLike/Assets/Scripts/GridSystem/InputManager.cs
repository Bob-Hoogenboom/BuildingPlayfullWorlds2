using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject playerOBJ;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LayerMask gridLayer;

    private void Start()
    {
        if(gridManager == null)
        {
            Debug.LogWarning("DEV CLICHÉ #1: forgot to assign a reference");
            gridManager = FindObjectOfType<GridManager>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit);

            if (!hasHit) return;
            if (hit.transform.tag == "GridNode")
            {
                Debug.Log("GridNodeHit");
                MoveToTile(hit);
            }
            if (hit.transform.tag == "Unit")
            {
                Debug.Log("UnitHit");
                InteractWithUnit();
            }
        }
    }

    //Handles the logic for moving and interacting with a grid tile
    private void MoveToTile(RaycastHit hit)
    {
        //get the targetPosition for the player to move to
        GridNode gridNode = hit.transform.GetComponent<GridNode>();
        var targetCords = gridNode.GetCoords();
        var targetPos = gridManager.GetWorldPosFromGrid(targetCords);

        var startCords = gridManager.GetGridPosFromWorld(playerOBJ.transform.position);

        playerOBJ.transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
    }

    //Handles the logic for interacting with another gameObject on the grid
    private void InteractWithUnit()
    {

    }
}
