using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Source: https://www.youtube.com/watch?v=gHU5RQWbmWE&t=323s
/// </summary>

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls;

    public bool[] testStatus;



    private void Start()
    {
        UpdateRoom(testStatus);
    }


    //Sets the entrances active and inactive in a single room
    private void UpdateRoom(bool[] status)
    {
        for(int i = 0; i < status.Length; i++)
        {
            walls[i].SetActive(status[i]);
        }
    }
}
