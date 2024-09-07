using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator TurnOff()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(1);
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
