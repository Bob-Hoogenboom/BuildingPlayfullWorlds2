using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LayerMask gridLayer;

    // Start is called before the first frame update
    void Start()
    {
        if(gridManager == null)
        {
            Debug.LogWarning("DEV CLICHÉ #1: forgot to assign a reference");
            gridManager = FindObjectOfType<GridManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Returns the GridNode object if the mouse Hovers over the GridNode
    private GridNode GetMouseHoverNode()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return null;
    }
}
