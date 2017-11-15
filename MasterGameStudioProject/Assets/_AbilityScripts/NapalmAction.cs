﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NapalmAction : MonoBehaviour {

	public Rigidbody thisRigid;
	RaycastHit hit;
	public float dist = 2f;
	public Vector3 downDir;
	// Use this for initialization
	void Start () {
		downDir = Vector3.down;
		thisRigid = this.GetComponent<Rigidbody> ();

	}
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerStay(Collider col){

		if (col.gameObject.tag == "Player1" || col.gameObject.tag == "Player2" || col.gameObject.tag == "Player3" || col.gameObject.tag == "Player4" ){
			if (this.GetComponent<AttackAction>().teamNum != col.gameObject.GetComponent<PlayerState>().teamNum && !col.gameObject.GetComponent<PlayerMovement>().isRolling) {

				col.gameObject.GetComponent<PlayerHealth> ().GetHit (this.GetComponent<AttackAction>().damage);
				//Destroy (this.gameObject);
			}
		}
	}
}
