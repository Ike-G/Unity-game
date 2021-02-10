using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class Shootable : MonoBehaviour 
{
    // This class should be inherited by any member that can take damage. All specifics should be handled within the damaging object, this merely acts as an interface. 
    protected float maxHealth; 
    protected float health; 
    // protected float speed; 
    
    public float getHealth { 
        get { return health; }
    }
    public void takeDamage(float d) { 
        health -= d;
        Debug.Log(health); 
        if (health <= 0) 
            Death(); 
    }
    protected abstract void Death();
}
