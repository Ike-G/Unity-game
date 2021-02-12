using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowMovement : MonoBehaviour
{
    [SerializeField] float baseDamage = 10; 
    [SerializeField] float speed; 
    Rigidbody2D rb;
    public float damageMod { get; set; } = 1; 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up*speed; 
    }

    // Update is called once per frame

    void OnCollisionEnter2D(Collision2D collider) { 
        if (!collider.gameObject.CompareTag("Friendly")) {
            Shootable s = collider.gameObject.GetComponent<Shootable>(); 
            if (s != null) 
                s.takeDamage(baseDamage*damageMod);
            Destroy(gameObject, 0); 
        }
    }
}
