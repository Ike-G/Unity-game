using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Shootable : MonoBehaviour 
{
    // This class should be inherited by any member that can take damage. All specifics should be handled within the damaging object, this merely acts as an interface. 
    protected float maxHealth; 
    protected float health; 
    public float getHealth { 
        get { return health; }
    }
    public void takeDamage(float d) { 
        health -= d;
        Debug.Log(health); 
        if (health <= 0) 
            Death(); 
    }

    public Vector2[] pathToDest(Vector2 dest) { 
        throw new UnityException(); 
    }

    protected abstract void Death();
}
