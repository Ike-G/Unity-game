using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHits : Shootable
{
    [SerializeField] float speed;
    [SerializeField] float findRange;
    [SerializeField] Transform player;
    [SerializeField] float stoppingDistance;
    [SerializeField] float dashSpeed; 
    [SerializeField] GameObject explosion;
	[SerializeField] private float pMaxHealth;
    [SerializeField] private GameObject dtext; 
    private Rigidbody2D rb;
    private bool detected;
    private bool primed;
    
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        detected = false;
        primed = true;
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
                if (primed == false)
                {
                    primed = true;
                }
            }
            else
            {
                if (primed == true && Vector2.Distance(transform.position, player.position) <= stoppingDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.position, dashSpeed * Time.deltaTime);
                }
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
            Shootable md = other.gameObject.GetComponent<Shootable>();
            if (md != null) 
                md.takeDamage(60);
            Explode();
        }
    }
    private void Explode()
    {
        Destroy(gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

    override protected void Death() { 
	    Destroy(gameObject, 0);
    }
	
}