using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; 
using System; 

public abstract class Unit : Shootable
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
    protected int currentWaypoint; 
    protected float radius; 
    protected float speed; 
    protected Rigidbody2D rb;
    public float damageMod { get; set; }

    public abstract void initialise();
    protected override void Death() { 

    } 
    protected GameObject getTarget() { 
        GameObject target = null; 
        // RaycastHit2D hit; 
        float minDist = float.MaxValue; 
        List<Collider2D> potentialTargets = new List<Collider2D>(); 
        Physics2D.OverlapCircle(transform.position, viewRange, new ContactFilter2D().NoFilter(), potentialTargets);
        foreach (Collider2D c in potentialTargets) {
            float dist = Vector2.Distance(transform.position, c.transform.position);
            if (c.CompareTag(targetTag) && dist < minDist) {
                dist = minDist; 
                target = c.gameObject; 
            }
        }
        Debug.Log($"Got target at {target.transform.position}");
        return target; 
    }

    protected void pathing() { 
        checkCooldown = Mathf.Max(0, checkCooldown - Time.deltaTime); 
        if (initialised) { 
            if (checkCooldown == 0) {
                target = getTarget(); 
                // Debug.Log($"enemy location: {target.transform.position}");
                if (target != null) 
                    seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
                checkCooldown = 1f; 
            }
            if (path != null) {
                if (currentWaypoint >= path.vectorPath.Count || Vector2.Distance(transform.position, target.transform.position) < radius) {
                    // reachedEndOfPath = true; 
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