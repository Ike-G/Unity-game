using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DamageNumbers : MonoBehaviour
{
    private Text number; 
    int frame = 0; 
    [SerializeField] int frames; 
    public int testPublic { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        number = gameObject.GetComponent<Text>(); 
        testPublic = 1; 
    }

    // Update is called once per frame
    void Update()
    {
        float factor = Mathf.Exp(-Mathf.Pow(frame*4/frames-2, 2)) + 1;  
        transform.localScale = new Vector3(factor, factor, 1);
        if (frame == frames) { 
            Destroy(gameObject, 0);
        } 
    }

    public void setText(string s) { 
        number.text = s; 
    }
}
