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
    [HideInInspector] public int currentLevel = 1;
    public static Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };

    Transform roomManager;
    List<GameObject> rooms = new List<GameObject>();
    int currentRoom = 0;

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel(MM.InitRooms(currentLevel));
        roomManager = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        // test go next level
        if (Input.GetKeyDown("space"))
            ChangeLevel(0);

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
            rooms.Add(Instantiate(roomPrefab, Vector3.Scale(location, roomDimensions * 1.05f), Quaternion.identity, roomParent));

        List<Vector3> doorLocations = MM.GetDoorLocations();
        foreach(Vector3 location in doorLocations)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 dir = directions[i];

                RaycastHit2D hit = Physics2D.Raycast(Vector3.Scale(location, roomDimensions * 1.05f), dir, 0.5f, LayerMask.GetMask("doorLayer"));
                if (hit.collider != null)
                {
                    StartCoroutine(hit.collider.GetComponent<Door>().TurnOff());
                    foreach (Transform child in hit.collider.transform)
                        child.gameObject.SetActive(true);
                }
            }
        }
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

    void ChangeLevel(int value)
    {
        ++currentLevel;
        foreach (Transform room in roomManager)
            Destroy(room.gameObject);

        rooms.Clear();
        GenerateLevel(MM.InitRooms(currentLevel));
        mainCam.target = new Vector3(0, 0, -10);
        currentRoom = 0;
    }
}
