using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSpawning : Unit
{
    SpriteRenderer[] srs;
    PolygonCollider2D[] pcs; 
    archerBehaviour[] scripts;
    [SerializeField] string ptargetTag; 
    [SerializeField] float pviewRange; 
    [SerializeField] float pspeed; 
    [SerializeField] float pradius; 
    // Start is called before the first frame update
    void Start()
    {
        srs = this.transform.GetComponentsInChildren<SpriteRenderer>();
        pcs = this.transform.GetComponentsInChildren<PolygonCollider2D>();
        scripts = GetComponentsInChildren<archerBehaviour>();
        foreach (SpriteRenderer sr in srs) {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f); 
        }
        foreach(PolygonCollider2D pc in pcs) { 
            pc.enabled = false; 
        }
    }

    public override void initialise() {
        foreach (SpriteRenderer sr in srs) {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
        }
        foreach(PolygonCollider2D pc in pcs) { 
            pc.enabled = true; 
        }
        foreach (archerBehaviour s in scripts) {
            s.initialise();
            s.setRadius = pradius; 
            s.setSpeed = pspeed; 
            s.setTargetTag = ptargetTag; 
            s.setViewRange = pviewRange; 
        }
    }
}
