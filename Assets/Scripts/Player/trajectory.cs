using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trajectory : MonoBehaviour
{
    [SerializeField] float speed; 
    [SerializeField] float activeTime; 
    [SerializeField] GameObject explosion; 
    [SerializeField] float baseDamage = 50; 
    public float damageMod { get; set; } = 1; 
    private float explosionDamageMod; 
    private Rigidbody2D rb; 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up*speed; 
    }

    void FixedUpdate()
    {
        activeTime -= Time.fixedDeltaTime; 
        if (activeTime <= 0) { 
            Explode();
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        Shootable s = other.gameObject.GetComponent<Shootable>();
        if (s != null) 
            s.takeDamage(baseDamage*damageMod);
        Explode();
    }

    private void Explode() {
        Destroy(gameObject, 0);
        GameObject e = Instantiate(explosion, transform.position, Quaternion.identity);
        e.GetComponent<ExplosionDamage>().damageMod = damageMod; 
    }
}
