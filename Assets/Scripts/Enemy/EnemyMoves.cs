using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; 

public class EnemyMoves : Shootable
{
    [SerializeField] float speed;
    // [SerializeField] float stoppingDistance;
    // [SerializeField] float retreatDistance;
	[SerializeField] float findRange; 
    [SerializeField] private float pMaxHealth; 
    private float shotInterval;
    [SerializeField] float shotStartInterval;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform player;
	[SerializeField] float viewRange = 10; 
	[SerializeField] float radius; 
	[SerializeField] float attackRange; 
	float checkCooldown = 0; 
	float attackCD = 0; 
	
	[SerializeField] GameObject dtext; 
    private bool detected;
	Seeker seeker; 
	Rigidbody2D rb;
	Path path; 
	GameObject target; 
	int currentWaypoint; 
	SpriteRenderer sr; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        shotInterval = shotStartInterval;
        
        maxHealth = pMaxHealth; 
        
        health = maxHealth; 

		damageText = dtext; 
		seeker = GetComponent<Seeker>(); 
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

	    if (detected == false && Vector2.Distance(transform.position, player.position) < findRange)
	    {
		    detected = true;
	    }
	    
        if (detected == true)
			{
			// if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        	// {
            // 	transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        	// } else if (Vector2.Distance(transform.position, player.position) < stoppingDistance &&
            //        		Vector2.Distance(transform.position, player.position) > retreatDistance)
        	// {
            // 	transform.position = this.transform.position;
        	// } else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        	// {
            // 	transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        	// }
			pathing(); 

        	if (shotInterval <= 0)
        	{
            	Instantiate(projectile, transform.position, Quaternion.identity);
            	shotInterval = shotStartInterval;
        	}
        	else
        	{
            	shotInterval -= Time.deltaTime;
        	}
		}
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.2f+health / (maxHealth*2)); 
	}
    
    override protected void Death()
    {
	    Destroy(gameObject, 0);
    }

	GameObject getTarget() { 
        GameObject target = null; 
        // RaycastHit2D hit; 
        float minDist = float.MaxValue; 
        List<Collider2D> potentialTargets = new List<Collider2D>(); 
        Physics2D.OverlapCircle(transform.position, viewRange, new ContactFilter2D().NoFilter(), potentialTargets);
        foreach (Collider2D c in potentialTargets) {
            float dist = Vector2.Distance(transform.position, c.transform.position);
            if (c.CompareTag("Player") && dist < minDist) {
                dist = minDist; 
                target = c.gameObject; 
            }
        }
        Debug.Log($"Got target at {target.transform.position}");
        return target; 
    }

	void pathing() { 
        checkCooldown = Mathf.Max(0, checkCooldown - Time.deltaTime); 
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

	void OnPathComplete(Path p) { 
        if (!p.error) { 
            path = p; 
            currentWaypoint = 0; 
        }
    }
}