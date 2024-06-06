using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public Vector3 target;
    private float moveSpeed = .25f;

    // Start is called before the first frame update
    void Start()
    {
        target = new Vector3(0, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, target) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed);
        }
    }
}
