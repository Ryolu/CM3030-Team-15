using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	public GameObject room;
	public GameObject door;
    public Camera miniCam;
    public Transform minimap;

    private int numRooms = 0;
    public int roomLimit;
    private Vector3[] directions = { new Vector3(1, 0, 0), new Vector3(0, -1, 0), new Vector3(0, 1, 0), new Vector3(0, -1, 0) };
    private List<Vector3> locations = new List<Vector3>();
    private List<GameObject> rooms = new List<GameObject>();
    private List<GameObject> doors = new List<GameObject>();
    private GameObject playerIndicator;
	
    // Start is called before the first frame update
    void Start()
    {
    }

    public List<Vector3> InitRooms(int currentLevel)
    {
        ResetRooms();
        roomLimit = currentLevel * 2 + 1;

        while (numRooms == 0)
        {
            GenerateRooms(currentLevel);
        }

        return locations;
    }

    private void Update()
    {

    }

    void GenerateRooms(int currentLevel)
    {
        SpawnRooms(currentLevel);

        if (numRooms != roomLimit)
            ResetRooms();
    }

    void SpawnRooms(int currentLevel)
    {
        Vector3 start = Vector3.zero;
        playerIndicator = Instantiate(door, start, Quaternion.identity, minimap);
        playerIndicator.transform.localScale *= 2.5f;
        playerIndicator.GetComponent<SpriteRenderer>().color = Color.green;
        //SpawnRandom(start, Vector3.zero);
        SpawnStraight(start);

        rooms[0].GetComponent<SpriteRenderer>().color = Color.blue;
        rooms[roomLimit - 1].GetComponent<SpriteRenderer>().color = Color.red;

        Vector3 center = Vector3.zero;
        center.z = -10;
        foreach (Vector3 pos in locations)
            center += pos;

        center /= locations.Count;
        miniCam.transform.position = center;
        if (currentLevel > 5)
            miniCam.GetComponent<Camera>().orthographicSize = currentLevel + 0.5f;
    }

    void SpawnStraight(Vector3 start)
    {
        Vector3 roomPos;
        for (int i = 0; i < roomLimit; ++i)
        {
            roomPos = start + new Vector3(0, i, 0);
            rooms.Add(Instantiate(room, roomPos, Quaternion.identity, minimap));
            locations.Add(roomPos);
            if (i != 0)
                doors.Add(Instantiate(door, roomPos - Vector3.up / 2, Quaternion.identity, minimap));
            numRooms++;
        }
    }

    void SpawnRandom(Vector3 pos, Vector3 doorPos)
    {
        if (numRooms > 0 && pos.Equals(Vector3.zero))
            return;
        if (locations.Contains(pos) || numRooms == roomLimit)
            return;

        rooms.Add(Instantiate(room, pos, Quaternion.identity, minimap));
        locations.Add(pos);
        numRooms++;

        if (pos != Vector3.zero)
            doors.Add(Instantiate(door, pos - doorPos, Quaternion.identity, minimap));

        for (int i = 0; i < 4; i++)
        {
            bool generate = Random.Range(0f, 1f) < 0.3 ? true : false;
            if (generate)
            {
                SpawnRandom(pos + directions[i], directions[i] / 2);
            }
        }
    }

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

    public void ChangeRoom(int value)
    {
        playerIndicator.transform.position = locations[value];
    }
}
