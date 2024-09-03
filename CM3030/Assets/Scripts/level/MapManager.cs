using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class MapManager : MonoBehaviour
{
	public GameObject room;
	public GameObject door;
    public Camera miniCam;
    public Transform minimap;
    public int roomLimit;

    int numRooms = 0;
    Vector3[] directions = { new Vector3(1, 0, 0), new Vector3(0, -1, 0), new Vector3(0, 1, 0), new Vector3(0, -1, 0) };
    List<Vector3> locations = new List<Vector3>();
    List<GameObject> rooms = new List<GameObject>();
    List<GameObject> doors = new List<GameObject>();
    GameObject playerIndicator;
	
    /// <summary>
    /// Start creating the rooms based on the current level.
    /// </summary>
    /// <param name="currentLevel">Current level.</param>
    /// <returns></returns>
    public List<Vector3> InitRooms(int currentLevel)
    {
        // reset all rooms first
        ResetRooms();

        // update the room limit
        roomLimit = currentLevel * 2 + 1;

        // generate the rooms until there are no exceptions thrown
        while (numRooms == 0)
            GenerateRooms();

        // return the center point of each room
        return locations;
    }

    /// <summary>
    /// Spawn the rooms based on current level.
    /// </summary>
    /// <param name="currentLevel">Current level.</param>
    void GenerateRooms()
    {
        SpawnRooms();

        // reset all rooms if the number of rooms is too low
        if (numRooms != roomLimit)
            ResetRooms();
    }

    /// <summary>
    /// Spawn the rooms.
    /// </summary>
    void SpawnRooms()
    {
        // make the player indicator start from the center
        playerIndicator = Instantiate(door, Vector3.zero, Quaternion.identity, minimap);
        playerIndicator.transform.localScale *= 2.5f;
        playerIndicator.GetComponent<SpriteRenderer>().color = Color.green;

        // actually spawn the rooms now
        //SpawnRandom(Vector3.zero, Vector3.zero);
        SpawnStraight();

        // starting rooms is blue
        rooms[0].GetComponent<SpriteRenderer>().color = Color.blue;
        // bossroom is reed
        rooms[roomLimit - 1].GetComponent<SpriteRenderer>().color = Color.red;

        // center the minimap camera onto the room the player starts from
        Vector3 start = Vector3.zero;
        start.z = -10f;
        miniCam.transform.position = start;
    }

    /// <summary>
    /// Spawn rooms in a straight line
    /// </summary>
    void SpawnStraight()
    {
        for (int i = 0; i < roomLimit; ++i)
        {
            // determine the room position
            Vector3 roomPos = new Vector3(0, i, 0);
            // create rooms and add to list
            rooms.Add(Instantiate(room, roomPos, Quaternion.identity, minimap));
            // add room center
            locations.Add(roomPos);
            // add door if not first room
            if (i != 0)
                doors.Add(Instantiate(door, roomPos - Vector3.up / 2, Quaternion.identity, minimap));
            numRooms++;
        }
    }

    /// <summary>
    /// Spawn rooms in a random manner.
    /// </summary>
    /// <param name="pos">Position of new room.</param>
    /// <param name="doorPos">Position of door to previous room.</param>
    void SpawnRandom(Vector3 pos, Vector3 doorPos)
    {
        // if room loops back to origin
        if (numRooms > 0 && pos.Equals(Vector3.zero))
            return;
        // if room already exists in new location or limit has been reached
        if (locations.Contains(pos) || numRooms == roomLimit)
            return;

        // create room and add location to list
        rooms.Add(Instantiate(room, pos, Quaternion.identity, minimap));
        locations.Add(pos);
        numRooms++;

        // room not at origin, add door
        if (pos != Vector3.zero)
            doors.Add(Instantiate(door, pos - doorPos, Quaternion.identity, minimap));

        // shuffle directions then spawn room there based on chance
        Shuffle(directions);
        for (int i = 0; i < 4; i++)
            if (Random.Range(0f, 1f) < 0.25f)
                SpawnRandom(pos + directions[i], directions[i] / 2);
    }

    /// <summary>
    /// Fisher-Yates shuffle on generic list
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="list">List</param>
    void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // resets anything map related
    void ResetRooms()
    {
        numRooms = 0;
        locations.Clear();

        foreach (Transform child in minimap)
            Destroy(child.gameObject);

        doors.Clear();
        rooms.Clear();

        Destroy(playerIndicator);
    }

    /// <summary>
    /// Move the minimap camera and player indicator to the room based on argument value.
    /// </summary>
    /// <param name="value">Room to move to.</param>
    public void ChangeRoom(int value)
    {
        playerIndicator.transform.position = locations[value];
        miniCam.transform.position = locations[value] - new Vector3(0, 0, 10);
    }
}
