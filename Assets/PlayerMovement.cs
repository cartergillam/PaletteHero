using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour
{ 
    public Rigidbody2D myRigidbody;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            myRigidbody.velocity = Vector2.up * speed;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            myRigidbody.velocity = Vector2.left * speed;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            myRigidbody.velocity = Vector2.down * speed;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            myRigidbody.velocity = Vector2.right * speed;
        }

    }
}
