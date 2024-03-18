using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Source: https://www.youtube.com/watch?v=gHU5RQWbmWE&t=323s
/// </summary>

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls;


    //Sets the entrances active and inactive in a single room and needs to be read by DungeonGenerator.cs
    public void UpdateRoom(bool[] status)
    {
        for(int i = 0; i < status.Length; i++)
        {
            walls[i].SetActive(!status[i]);
        }
    }

    public void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, transform.position, Quaternion.identity);
    }
}
