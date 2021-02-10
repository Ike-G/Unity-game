using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; 
using System; 

public abstract class Unit : MonoBehaviour
{
    // This refers to anything which the player may place down that will remain present
    protected float viewRange; 
    protected string targetTag; 
    protected float checkCooldown = 0; 
    protected bool initialised = false; 
    protected GameObject target; 
    protected Seeker seeker; 
    protected Path path; 
    protected bool reachedEndOfPath = false; 
    protected float nextWaypointDistance = 3f;
    protected int currentWaypoint; 
    protected float speed; 
    protected Rigidbody2D rb;

    public abstract void initialise();
    protected GameObject getTarget() { 
        GameObject target = null; 
        RaycastHit2D hit; 
        float minDist = float.MaxValue; 
        for (float i = 0; i < 2*Mathf.PI; i+=Mathf.PI/6) {
            hit = Physics2D.Raycast(transform.position, viewRange * new Vector2(Mathf.Cos(i), Mathf.Sin(i))); 
            try {
                float dist = Vector2.Distance(transform.position, hit.collider.transform.position);
                if (hit.collider.tag == targetTag && dist < minDist) {
                    dist = minDist; 
                    target = hit.collider.gameObject;  
                }
            } catch (Exception) {}
        }
        return target; 
    }

    protected void pathing() { 
        checkCooldown = Mathf.Max(0, checkCooldown - Time.deltaTime); 
        if (initialised) { 
            if (checkCooldown == 0) {
                target = getTarget(); 
                Debug.Log($"enemy location: {target.transform.position}");
                seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
                checkCooldown = 1f; 
            }
            if (path != null) {
                // if (currentWaypoint >= path.vectorPath.Count) 
                //     reachedEndOfPath = true; 
                // else {
                //     Vector2 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
                //     Vector2 force = direction * speed * Time.deltaTime; 
                //     rb.AddForce(force); 
                //     float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

                //     if (distance < nextWaypointDistance) {
                //         currentWaypoint++; 
                //     }
                // }
                if (currentWaypoint >= path.vectorPath.Count) {
                    reachedEndOfPath = true; 
                    rb.velocity = Vector2.zero;
                } else { 
                    rb.velocity = ((Vector2)path.vectorPath[currentWaypoint] - rb.position) / Vector2.Distance((Vector2)path.vectorPath[currentWaypoint], rb.position) * speed * 50;
                    if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]) < 0.5f ) 
                        currentWaypoint++; 
                }
            }
        }
    }

    protected void OnPathComplete(Path p) { 
        if (!p.error) { 
            path = p; 
            currentWaypoint = 0; 
        }
    }
}
