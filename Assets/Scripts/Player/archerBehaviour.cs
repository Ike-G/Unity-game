using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; 

public class archerBehaviour : Unit
{
    public string setTargetTag { set { targetTag = value; }}
    public float setViewRange { set { viewRange = value; }}
    public float setSpeed { set { speed = value; }}
    public float setRadius { set { radius = value; }}
    [SerializeField] GameObject arrow; 
    [SerializeField] float arrowCD; 
    bool shooting = false; 
    float shootCD = 0; 
    int statTime; 
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>(); 
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        attackTarget(); 
        pathing(); 
    }

    void FixedUpdate() {
        shootCD = Mathf.Max(0, shootCD - Time.fixedDeltaTime); 
    }

    // GameObject enemyCheck() {
    //     GameObject enemy = null; 
    //     List<Collider2D> enemies = new List<Collider2D>();  
    //     int n = Physics2D.OverlapCircle((Vector2)transform.position, 5, new ContactFilter2D().NoFilter(), enemies);
    //     float minDist = float.PositiveInfinity;
    //     if (n > 0) {
    //         foreach (Collider2D e in enemies) { 
    //             float dist = (e.transform.position-transform.position).magnitude;
    //             if (e.tag == "Enemy" && dist < minDist ) {
    //                 minDist = dist;
    //                 enemy = e.gameObject;
    //             }
    //         }
    //     }
    //     return enemy; 
    // }

    // void TrackEnemy(GameObject e) {
        
    // }

    public override void initialise() { 
        initialised = true; 
    }

    void attackTarget() { 
        if (target == null) 
            shooting = false; 
        else if (shootCD <= 0f) { 
            Vector2 diff = (target.transform.position - transform.position);
            GameObject a = Instantiate(arrow, transform.position + new Vector3(diff.normalized.x, diff.normalized.y, 0), Quaternion.Euler(0, 0, 360f / (2*Mathf.PI) * Mathf.Atan2(-diff.x, diff.y)));
            Debug.Log("shot arrow");
            arrowMovement am = a.GetComponent<arrowMovement>();
            am.damageMod = damageMod; 
            shootCD = arrowCD; 
        }
    }
}
