using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoves : Shootable
{
    [SerializeField] float speed;
    [SerializeField] float stoppingDistance;
    [SerializeField] float retreatDistance;
    [SerializeField] float findRange;
    [SerializeField] private float pMaxHealth; 
    private float shotInterval;
    [SerializeField] float shotStartInterval;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform player;
	[SerializeField] GameObject dtext; 
    private bool detected;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        shotInterval = shotStartInterval;
        
        maxHealth = pMaxHealth; 
        
        health = maxHealth; 

		damageText = dtext; 
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
			if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        	{
            	transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        	} else if (Vector2.Distance(transform.position, player.position) < stoppingDistance &&
                   		Vector2.Distance(transform.position, player.position) > retreatDistance)
        	{
            	transform.position = this.transform.position;
        	} else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        	{
            	transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        	}

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
    }
    
    override protected void Death()
    {
	    Destroy(gameObject, 0);
    }
}