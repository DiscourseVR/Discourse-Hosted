using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class movement : NetworkBehaviour
{
    private Rigidbody rb;

    [SyncVar(hook = nameof(changed))]
    private Vector3 position = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (isLocalPlayer)
        {
            position = rb.position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (!(h==0 && v == 0))
            {
                rb.position = rb.position + new Vector3(h * 0.05f, 0, v * 0.05f);
                position = rb.position;
                setPosition(position);
            }
        }
    }

    [Command]
    void setPosition(Vector3 Position)
    {
        position = Position;
    }


    int adjustTime = -1;
    Vector3 addVector;
    int lerps;

    IEnumerator lerp(int listenValue)
    {
        Debug.Log("Lerping");

        for (int i = 1; i <= lerps; i++)
        {
            if (listenValue != adjustTime)
            {
                break;
            }

            rb.position = rb.position + addVector;
            yield return 0;
        }

        if (listenValue == adjustTime)
        {
            rb.position = position;
        }
    }

    void changed(Vector3 old, Vector3 newPosition)
    {
        if (!isLocalPlayer)
        {
            Debug.Log("Adjusting location");
            lerps = (int)Math.Ceiling(Vector3.Distance(newPosition, old) / 0.05f);
            addVector = (newPosition - old) / lerps;

            DateTime dt = DateTime.Now;
            adjustTime = dt.Millisecond;

            Debug.Log("Adjusting location 2");
            StartCoroutine(nameof(lerp), (adjustTime));
        }
    }
    
}
