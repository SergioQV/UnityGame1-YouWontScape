using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparo : MonoBehaviour {

    Rigidbody2D rb2d;
    public float direccion;
    public float speed;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        
            rb2d.velocity = direccion*transform.right * speed ;
        
	}
}
