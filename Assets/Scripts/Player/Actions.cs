using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; 

public class Actions : Shootable
{
    [SerializeField] private Vector3 startPos; 
    [SerializeField] private float speed; 
    [SerializeField] private GameObject[] respawnList; 
    [SerializeField] private float pMaxHealth; 
    [SerializeField] private float dashSpeed; 
    private Transform t;
    private Vector3 intention; 
    private Vector2[] movements; 
    private int moveIndex = 0; // remaining iterations (ri*20 = remaining time in milliseconds)
    private Rigidbody2D rb;
    private bool complete = true; 
    private int checkpoint = 0;
    private bool followingPath = false; 
    private Path path; 
    int currentWaypoint = 0; 
    float nextWaypointDistance = 3f;
    Seeker seeker; 

    // Start is called before the first frame update
    private void Start()
    {
        intention = startPos; 
        rb = gameObject.GetComponent<Rigidbody2D>();
        t = gameObject.GetComponent<Transform>();
        rb.position = startPos;
        maxHealth = pMaxHealth; 
        health = maxHealth; 
        seeker = GetComponent<Seeker>();
    }

    private void FixedUpdate()
    {
        if (!complete) 
            approachPoint(intention);
    }

    private void approachPoint(Vector2 position) {
        if (moveIndex < movements.Length && !followingPath) {
            // rb.AddForce(movements[moveIndex]);
            rb.velocity = 50*movements[moveIndex];
            Debug.Log(rb.velocity);
            // t.Translate(movements[moveIndex]);
            moveIndex++;
        } else if (followingPath) { 
            if (currentWaypoint >= path.vectorPath.Count) {
                complete = true; 
                rb.velocity = Vector2.zero;
            } else { 
                rb.MovePosition(((Vector2)path.vectorPath[currentWaypoint] - rb.position) * speed * 0.5f + rb.position);
                if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance ) 
                    currentWaypoint++; 
            }
        } else {
            rb.velocity = Vector2.zero; 
            moveIndex = 0; 
            complete = true; 
        }
    }

    public void setIntent(Vector2 intent, bool dashing = false) {
        // Determine the sequence of movements 
        float ts = dashing ? dashSpeed : speed; 
        float m = (intent-rb.position).magnitude;
        int ti = dashing ? 5 : (int)(m/ts)+1; // Gets the number of iterations (final iteration may be 0,0)
        Vector2 s = (intent - rb.position) / m; // Unit direction (s.magnitude = 1)
        // movements = new List<Vector2>(ti); 
        movements = new Vector2[ti]; 
        movements[ti-1] = dashing? Vector2.zero : ts*(m/ts-(ti-1))*s;
        for (int i = 0; i < ti-1; i++) {
            movements[i] = s*ts;
        }
        moveIndex = 0; 
        complete = false; 
        intention = intent; 
        followingPath = false; 
    }

    override protected void Death() { 
        t.position = respawnList[checkpoint].transform.position;
        health = maxHealth;
        rb.velocity = Vector2.zero; 
    }

    void OnCollisionEnter2D(Collision2D collision) { 
        if (collision.gameObject.GetComponent<Rigidbody2D>().isKinematic) {
            seeker.StartPath(rb.position, intention, OnPathComplete); 
        }
    }

    void OnPathComplete(Path p) { 
        if (!p.error) {
            path = p; 
            currentWaypoint = 0; 
            followingPath = true; 
        }
    }
}