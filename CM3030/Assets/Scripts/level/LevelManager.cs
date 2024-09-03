using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Overall level manager.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public MapManager MM;
    public GameObject roomPrefab;
    public CameraTransition mainCam;
    public Vector3 roomDimensions;
    public Transform roomParent;
    public int currentLevel = 1;

    List<GameObject> rooms = new List<GameObject>();
    int currentRoom = 0;

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel(MM.InitRooms(currentLevel));
    }

    // Update is called once per frame
    void Update()
    {
        // test go next level
        if (Input.GetKeyDown("space"))
        {
            ++currentLevel;
            MM.InitRooms(currentLevel);
        }

        // room changing test code
        if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ChangeRoom(1);
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ChangeRoom(-1);
        }
    }

    /// <summary>
    /// Generate the level based on the provided locations.
    /// </summary>
    /// <param name="locations">Pre-generated locations of rooms.</param>
    void GenerateLevel(List<Vector3> locations)
    {
        foreach(Vector3 location in locations)
            rooms.Add(Instantiate(roomPrefab, Vector3.Scale(location, roomDimensions * 1.1f), Quaternion.identity, roomParent));
    }

    /// <summary>
    /// Room change sequence.
    /// </summary>
    /// <param name="value">Room to change to.</param>
    void ChangeRoom(int value)
    {
        currentRoom += value;
        MM.ChangeRoom(currentRoom);
        mainCam.target = rooms[currentRoom].transform.position - new Vector3(0, 0, 10);
    }
}
