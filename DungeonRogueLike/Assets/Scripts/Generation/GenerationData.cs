using UnityEngine;

[CreateAssetMenu]

public class GenerationData : ScriptableObject
{
    [Header("General")]

    [Tooltip("The size of the dungeon")]
    public Vector2Int dungeonSize = new Vector2Int(3, 3);

    [Tooltip("A prefab of the player object")]
    public GameObject player;

    [Header("Maze Generation")]

    [Tooltip("The position from where the alghoritm should start, '0' is the first room")]
    public int startPos = 0;

    [Tooltip("A prefab of the room that is used to make up the Maze")]
    public GameObject dungeonRoom;

    [Tooltip("Distance between each of the rooms")]
    public Vector2 roomOffset;

    [Tooltip("The random chance an item will spawn between 0 and how high this value is set")]
    public int itemChance = 10;

    [Tooltip("")]
    public GameObject item;


    [Header("Grid Generation")]

    [Tooltip("")]
    public GameObject gridNode;

    [Tooltip("")]
    public GameObject[,] grid;

    [Tooltip("Defines the amount of nodes per room")]
    public int roomSize;

    [Tooltip("Defines the offset for the origin of the grid")]
    public float gridOffset;

    [Tooltip("Defines the spcae between each Node")]
    public float nodeOffset = 4f;

    [Header("Enemies")]

    [Tooltip("The boss that spawns at the end of the dungeon")]
    public GameObject levelBoss;

    [Tooltip("The enemies that are spawned after the boss is spawned")]
    public GameObject[] enemies;


    /*
      ____        _     _            __          __         _    _               
     |  _ \      | |   | |           \ \        / /        | |  | |              
     | |_) | ___ | |__ | |__  _   _   \ \  /\  / /_ _ ___  | |__| | ___ _ __ ___ 
     |  _<  / _ \| '_ \| '_ \| | | |   \ \/  \/ / _` / __| |  __  |/ _ \ '__/ _ \
     | |_) | (_) | |_) | |_) | |_| |    \  /\  / (_| \__ \ | |  | |  __/ | |  __/
     |____/ \___/|_.__/|_.__/ \__, |     \/  \/ \__,_|___/ |_|  |_|\___|_|  \___|
                               __/ |                                             
                              |___/                                              */
}
