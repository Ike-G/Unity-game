using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updateMana : MonoBehaviour
{
    RTSController playerScript; 
    Text manaText; 
    // Start is called before the first frame update
    void Start()
    {
        playerScript = transform.parent.parent.parent.GetComponentInChildren<RTSController>();
        manaText = gameObject.GetComponent<Text>(); 
    }

    // Update is called once per frame
    void Update()
    {
        manaText.text = playerScript.getMana.ToString(); 
    }
}
