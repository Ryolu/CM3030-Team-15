using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public MapManager MM;
    public GameObject roomPrefab;
    public CameraTransition mainCam;
    public Vector3 roomDimensions;
    public Transform roomParent;
    public int currentLevel = 1;

    private List<GameObject> rooms = new List<GameObject>();
    private int currentRoom = 0;

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

        if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ChangeRoom(1);
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ChangeRoom(-1);
        }
    }

    void GenerateLevel(List<Vector3> locations)
    {
        foreach(Vector3 location in locations)
        {
            rooms.Add(Instantiate(roomPrefab, Vector3.Scale(location, roomDimensions), Quaternion.identity, roomParent));
        }
    }

    void ChangeRoom(int value)
    {
        currentRoom += value;
        MM.ChangeRoom(currentRoom);
        mainCam.target = rooms[currentRoom].transform.position - new Vector3(0, 0, 10);
    }
}
