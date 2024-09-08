using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform spawnPoint;
    public Door otherDoor;

    public IEnumerator TurnOff()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(1);
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void SetOther(List<Door> doors)
    {
        foreach (Door door in doors)
            if (door.gameObject != gameObject)
                otherDoor = door;
    }

    public Vector3 EnterDoor()
    {
        return otherDoor.spawnPoint.position;
    }
}
