using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Globalization;

public class RTSController : MonoBehaviour
{
    [SerializeField] private GameObject King; 
    [SerializeField] private GameObject pathPointer; 
    [SerializeField] private GameObject aimIndicator;
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject archers;
    [SerializeField] private GameObject giants; 
    [SerializeField] private GameObject block; 
    [SerializeField] private GameObject highlightSquare;
    [SerializeField] private Camera view;
    [SerializeField] private KeyCode[] keyset; 
    [SerializeField] private float fireballReq; 
    [SerializeField] private float archerReq;
    [SerializeField] private float giantReq; 
    [SerializeField] private float blockReq; 
    [SerializeField] private float maxMana; 
    [SerializeField] private float manaGainRate; 
    [SerializeField] private float maxDashCD; 
    
    private Ability[] abilitySet; 
    private Vector3[] abilityPos; 
    private List<GameObject>[] abilityUI;
    private Actions kingMethods; 
    private Rigidbody2D kingRB;
    private float mana; 
    private float dashCD; 
    public int getMana {
        get { return Mathf.FloorToInt(mana); }
    }

    public string getDashCD { 
        get { return Mathf.FloorToInt((maxDashCD - dashCD) / 3).ToString()+"\t" + (dashCD).ToString("F1", CultureInfo.CurrentCulture); }
    }

    public string getHealth {
        get { return kingMethods.getHealth.ToString(); }
    }
    // Start is called before the first frame update
    void Start()
    {
        kingMethods = King.GetComponent<Actions>();
        kingRB = King.GetComponent<Rigidbody2D>();
        abilitySet = new Ability[] { Fireball, Archers, Giants, Block };
        abilityPos = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        abilityUI = new List<GameObject>[] { new List<GameObject>(), new List<GameObject>(), new List<GameObject>(), new List<GameObject>() };
        mana = maxMana; 
        dashCD = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < abilitySet.Length; i++) {
            abilitySet[i].DynamicInvoke(i);
        }

        if (Input.GetMouseButtonDown(1)) {
            Vector2 intent = (Vector2)view.ScreenToWorldPoint(Input.mousePosition); 
            Instantiate(pathPointer, new Vector3(intent.x, intent.y, 0), Quaternion.identity);
            kingMethods.setIntent(intent);
        }

        if (Input.GetKey(KeyCode.Space)) {
            view.transform.Translate(kingRB.position - (Vector2)view.transform.position);
        }

        if (Input.GetKeyDown(KeyCode.F) && dashCD <= maxDashCD * 3 / 4) {
            Vector2 intent = (Vector2)view.ScreenToWorldPoint(Input.mousePosition); 
            kingMethods.setIntent(intent, true);
            dashCD += maxDashCD / 4; 
        }
    }

    void FixedUpdate() {
        mana = Math.Min(mana+Time.fixedDeltaTime*manaGainRate, maxMana);
        dashCD = Math.Max(dashCD-Time.fixedDeltaTime, 0);
    }

    delegate void Ability(int keyInd); 

    void Fireball(int keyInd) {
        if (Input.GetKeyDown(keyset[keyInd])) {
            Vector2 pos = view.ScreenToWorldPoint(Input.mousePosition);
            abilityPos[keyInd] = new Vector3(pos.x, pos.y, 0);
            GameObject tc = Instantiate(aimIndicator, abilityPos[keyInd], Quaternion.identity);
            GameObject ta = Instantiate(arrow, abilityPos[keyInd], Quaternion.identity);
            abilityUI[keyInd] = new List<GameObject>() { tc, ta };
            
        } else if (Input.GetKey(keyset[keyInd])) {
            Vector2 dp = view.ScreenToWorldPoint(Input.mousePosition) - abilityPos[keyInd];
            abilityUI[keyInd][1].transform.rotation = Quaternion.Euler(0, 0, 360f*(float)Math.Atan2(-(double)dp.x, (double)dp.y) / (2*(float)Math.PI));
            if (dp.magnitude > 1.75) {
                abilityUI[keyInd][1].transform.localScale = new Vector3(0.2f + 0.65f * (1 - (float)Math.Exp(-(dp.magnitude-1.8)/10)), 0.582f, 1);
            } else {
                abilityUI[keyInd][1].transform.localScale = new Vector3(0.2f, dp.magnitude / 1.8f * 0.582f, 1);
            }
        }

        if (Input.GetKeyUp(keyset[keyInd])) {
            if (mana >= fireballReq) {
                Vector2 dp = view.ScreenToWorldPoint(Input.mousePosition) - abilityPos[keyInd];
                Instantiate(fireball, abilityPos[keyInd], Quaternion.Euler(0, 0, 360f*(float)Math.Atan2(-(double)dp.x, (double)dp.y) / (2*(float)Math.PI)));
                mana -= fireballReq;
            }
            foreach (GameObject g in abilityUI[keyInd]) {
                Destroy(g);
            }
            
        }
    }

    void unitSpawning(int keyInd, GameObject g, float manaReq) {
        if (Input.GetKeyDown(keyset[keyInd])) {
            abilityPos[keyInd] = view.ScreenToWorldPoint(Input.mousePosition);
            abilityPos[keyInd] = new Vector3((float)Math.Floor(abilityPos[keyInd].x)+0.5f, (float)Math.Floor(abilityPos[keyInd].y)+0.5f, 0);
            GameObject a = Instantiate(g, abilityPos[keyInd], Quaternion.identity);
            GameObject b = Instantiate(highlightSquare, abilityPos[keyInd], Quaternion.identity);
            abilityUI[keyInd] = new List<GameObject>() { a, b };

        } else if (Input.GetKey(keyset[keyInd])) {
            Vector3 pos = view.ScreenToWorldPoint(Input.mousePosition); // Get current mouse position 
            pos = new Vector3((float)Math.Floor(pos.x)+0.5f, (float)Math.Floor(pos.y)+0.5f, 0); 
            if (pos != abilityPos[keyInd]) // Check that the position has changed and it is now only a tile which isn't an obstacle
            {
                Destroy(abilityUI[keyInd][1]);
                abilityUI[keyInd][1] = Instantiate(highlightSquare, pos, Quaternion.identity); // Set current tile to be highlighted
                abilityUI[keyInd][0].transform.position = pos;
                abilityPos[keyInd] = pos; 
            }
        }

        if (Input.GetKeyUp(keyset[keyInd]))
        {
            if (mana >= manaReq) {
                Vector3 pos = view.ScreenToWorldPoint(Input.mousePosition); // Get current mouse position 
                pos = new Vector3((float)Math.Floor(pos.x)+0.5f, (float)Math.Floor(pos.y)+0.5f, 0); 
                Unit s = abilityUI[keyInd][0].GetComponent<Unit>();
                s.initialise();
                mana -= manaReq;
            } else {
                Destroy(abilityUI[keyInd][0]);
            }
            Destroy(abilityUI[keyInd][1]);
        }
    }
    void Archers(int keyInd) {
        unitSpawning(keyInd, archers, archerReq);
    }

    void Giants(int keyInd) {
        unitSpawning(keyInd, giants, giantReq);
    }

    void Block(int keyInd) {
        if (Input.GetKey(keyset[keyInd]) && mana >= blockReq) {
            Vector3 pos = view.ScreenToWorldPoint(Input.mousePosition);
            pos = new Vector3((float)Math.Floor(pos.x)+0.5f, (float)Math.Floor(pos.y)+0.5f, 0); 
            List<Collider2D> c = new List<Collider2D>();
            int n = Physics2D.OverlapCircle(pos, 0.2f, new ContactFilter2D().NoFilter(), c);
            if (n == 0 || !c[0].gameObject.GetComponent<Rigidbody2D>().isKinematic) {
                Instantiate(block, pos, Quaternion.identity);
                mana -= blockReq; 
            }
        }
    }
}