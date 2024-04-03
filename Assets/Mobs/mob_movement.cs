using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPatrol : MonoBehaviour {
    public GameObject pointA;
    public GameObject pointB;
    public float speed; // Declare speed variable
    private Rigidbody2D rb; // Corrected variable name
    private Animator anim;
    private Transform currentPoint;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("isRunning", true);
    }

    void Update() {
        Vector2 point = currentPoint.position - (Vector3)transform.position;
        if (currentPoint == pointB.transform) {
            rb.velocity = new Vector2(speed, 0); // right point
        } else {
            rb.velocity = new Vector2(-speed, 0); // left point
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform) {
            currentPoint = pointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform) {
            currentPoint = pointB.transform;
        }
    }
}
