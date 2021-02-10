using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class Shootable : MonoBehaviour 
{
    // This class should be inherited by any member that can take damage. All specifics should be handled within the damaging object, this merely acts as an interface. 
    protected float maxHealth; 
    protected float health; 
    protected GameObject damageText; // THIS MUST BE INCLUDED AS A SERIALIZED FIELD IN EVERY SHOOTABLE
    // protected float speed; 
    
    public float getHealth { 
        get { return health; }
    }
    public void takeDamage(float d) { 
        health -= d;
        // float r1 = Random.Range(0, 2*Mathf.PI); 
        // float r2 = Random.Range(0.5f, 1.5f); 
        // Vector3 v = this.transform.position + 2*new Vector3(Mathf.Cos(r1), Mathf.Sin(r1), 0);
        // GameObject t = Instantiate(damageText, v, Quaternion.identity); 
        // t.GetComponent<DamageNumbers>().setText(d.ToString()); 
        Debug.Log($"{d} to {this.tag}"); 
        if (health <= 0) 
            Death(); 
    }
    protected abstract void Death();
}
