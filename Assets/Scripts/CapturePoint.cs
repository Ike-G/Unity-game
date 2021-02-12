using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : Interactable
{
    public bool isCaptured = false;
    public float timeToCap = 10;

    void Update()
    {
        if (playerInRange && timeToCap > 0)
        {
            timeToCap -= Time.deltaTime;
        }
        if (!playerInRange)
        {
            timeToCap = 10;
        }

        if (timeToCap <= 0)
        {
            isCaptured = true;
            GetComponent<PolygonCollider2D>().enabled = false;
        }


    }

}
