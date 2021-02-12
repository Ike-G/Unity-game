using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeStats : MonoBehaviour
{
    [SerializeField] string statBuff; 
    [SerializeField] float buffValue;
    [SerializeField] float downPeriod; 
    float cooldown = 0;  
    SpriteRenderer sr; 
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>(); 
        switch (statBuff) {
            case "Mana" : 
                sr.color = new Color(0, 0, 1, 0.8f); 
                break; 
            case "Health" : 
                sr.color = new Color(0, 1, 0, 0.8f); 
                break; 
            case "Damage" : 
                sr.color = new Color(1, 0, 0, 0.8f); 
                break; 
            case "Heal" : 
                sr.color = new Color(1, 0, 1, 0.8f); 
                break; 
            case "Speed" : 
                sr.color = new Color(0, 1, 1, 0.8f);
                break; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        cooldown = Mathf.Max(cooldown - Time.deltaTime, 0); 
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("Player") && cooldown == 0) {
            switch (statBuff) {
                case "Mana" :
                    collider.gameObject.GetComponent<Actions>().buffMana(buffValue);  
                    break;
                case "Health" : 
                    collider.gameObject.GetComponent<Actions>().buffHealth(buffValue); 
                    break; 
                case "Damage" : 
                    collider.gameObject.GetComponent<Actions>().buffDamage(buffValue); 
                    break; 
                case "Heal" : 
                    collider.gameObject.GetComponent<Actions>().Heal(buffValue); 
                    break; 
                case "Speed" : 
                    collider.gameObject.GetComponent<Actions>().buffSpeed(buffValue); 
                    break;
            }
            cooldown = downPeriod; 
            sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0.3f); 

        }
    }
}
