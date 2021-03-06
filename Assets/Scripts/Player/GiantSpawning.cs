﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; 

public class GiantSpawning : Unit
{
    SpriteRenderer sr; 
    PolygonCollider2D pc;
    bool inBlock = false;  
     
    [SerializeField] string ptargetTag; 
    [SerializeField] float pviewRange = 10; 
    [SerializeField] float pspeed = 10; 
    [SerializeField] float pradius; 
    [SerializeField] float attackRange; 
    [SerializeField] float pMaxHealth; 
    [SerializeField] float attackDamage; 
    float attackCD = 0; 
    
    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        pc = gameObject.GetComponent<PolygonCollider2D>();
        pc.isTrigger = true;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f); 
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();  
        targetTag = ptargetTag; 
        viewRange = pviewRange;
        speed = pspeed;
        radius = pradius; 
        maxHealth = pMaxHealth; 
    }

    protected override void Death() {
        Destroy(gameObject, 0); 
    }

    void Update() { 
        pathing(); 
        attackCD = Mathf.Max(0, attackCD - Time.deltaTime); 
        if (attackCD <= 0) {
            attackTarget();
            attackCD = 1; 
        }
    }

    public override void initialise() {
        if (!inBlock) {
            pc.isTrigger = false; 
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
            initialised = true; 
        } else {
            Destroy(gameObject, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.GetComponent<Rigidbody2D>()?.bodyType == RigidbodyType2D.Kinematic) {
            inBlock = true; 
        }
    }

    void OnTriggerEnter2D() {
        inBlock = false; 
    }

    void attackTarget() { 
        if (Vector2.Distance(target.transform.position, transform.position) < attackRange) {
            target.GetComponent<Shootable>().takeDamage(attackDamage*damageMod); 
        }
    }
}
