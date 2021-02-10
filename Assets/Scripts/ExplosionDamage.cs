using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] private float baseDamage = 5; 
    public float damageMod { get; set; } = 1; 
    // void FixedUpdate()
    // {
    //     for (int i = 0; i < precision; i++) {
    //         float rad = 2*Mathf.PI*i / precision;
    //         RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)), col.radius);
    //         //Debug.Log($"Raycast from {transform.position} to {new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * col.radius + (Vector2)transform.position}");
    //         // Debug.DrawLine(transform.position, (new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f))*col.radius + transform.position, Color.green);
    //         Shootable s = hit.transform.gameObject.GetComponent<Shootable>();
    //         Debug.Log(hit.transform); 
    //         if (s != null) {
    //             s.takeDamage(10f); 
    //         }
    //     }
    // }

    void OnTriggerEnter2D(Collider2D col) {
        Shootable s = col.gameObject.GetComponent<Shootable>(); 
        if (s != null) 
            s.takeDamage(baseDamage*damageMod); 
    }
}
