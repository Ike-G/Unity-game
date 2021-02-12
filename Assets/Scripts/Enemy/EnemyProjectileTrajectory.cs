using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileTrajectory : MonoBehaviour
{

    [SerializeField] float speed;
    
    [SerializeField] GameObject explosion;
    
    [SerializeField] float activeTime;

    [SerializeField] float tickerTime;

    private Transform player;

    private Vector2 target;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        target = new Vector2(player.position.x, player.position.y);
    }

    void FixedUpdate()
    {
        activeTime -= Time.fixedDeltaTime; 
        if (activeTime <= 0) { 
            Explode();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
 
        
        if (transform.position.x == player.position.x && transform.position.y == player.position.y)
        {
            Explode();
        }
    }
    
    void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Collision detected");
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
            Shootable ep = other.gameObject.GetComponent<Shootable>();
            if (ep != null) 
                ep.takeDamage(30);
            Explode();
        }
    }
    private void Explode() {
        Destroy(gameObject, tickerTime);
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}

