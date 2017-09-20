﻿using UnityEngine;
using System.Collections;
using InControl;

public class PlayerMovement : MonoBehaviour {

	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;

	public bool isDying = false;
	public bool canMove = true;
	float dyingTimer = 0.7f;

	public float hMovement;
	public float vMovement;
	public bool rollButton;



	RaycastHit hit;
	public float dist = 2f;
	public Vector3 downDir;
	public Vector3 oldPosition;

	public float rollTimer = 0.3f;
	public float rollTimerDef = 0.3f;
	public float rollTimerCool = 1f;
	public float rollTimerCoolDef = 1f;
	public bool isRolling = false;
	public bool isRollCooling = false;
	public Vector3 rollDirection;

	private Vector3 moveDirection = Vector3.zero;
	public CharacterController controller;

	public GameObject matchManager;

	public float stunTimer;

	public InputDevice currentJoystick;

	void Start() {
		rollTimer = rollTimerDef;
		controller = this.GetComponent<CharacterController> ();
		//matchManager = GameObject.Find ("MatchManager(Clone)");

		downDir = new Vector3 (0, -1, 0);

		if (this.gameObject.tag == "Player1") {
			if (InputManager.Devices [0] != null) {
				currentJoystick = InputManager.Devices [0];
			}
		}
		if (this.gameObject.tag == "Player2") {
			if (InputManager.Devices [1] != null) {
				currentJoystick = InputManager.Devices [1];
			}
		}
		if (this.gameObject.tag == "Player3") {
			if (InputManager.Devices [2] != null) {
				currentJoystick = InputManager.Devices [2];
			}
		}
		if (this.gameObject.tag == "Player4") {
			if (InputManager.Devices [3] != null) {
				currentJoystick = InputManager.Devices [3];
			}
		}

	}

	void Update() {



		if (currentJoystick != null) {
			hMovement = currentJoystick.LeftStickX.RawValue;
			vMovement = currentJoystick.LeftStickY.RawValue;
			rollButton = currentJoystick.Action1.WasPressed;
		}



		oldPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);

		if (stunTimer > 0) {
			stunTimer -= Time.deltaTime;

		}

		if (isRolling) {
			//Temp Code
			controller.Move (rollDirection * Time.deltaTime * (speed * 4f));
			rollTimer -= Time.deltaTime;
			if (rollTimer <= 0) {
				isRolling = false;
				isRollCooling = true;
				rollTimer = rollTimerDef;
			}

		}

		if (isRollCooling) {
			rollTimerCool -= Time.deltaTime;
			if (rollTimerCool <= 0) {
				isRollCooling = false;
				rollTimerCool = rollTimerCoolDef;
			}

		}


		if (stunTimer <= 0) {
			canMove = true;

		}



		if (isDying == true) {
			dyingTimer -= Time.deltaTime;
			if (dyingTimer <= 0) {
				Die ();
			}
		}

	

		if (isDying == false && canMove == true) {
			
		
			//if (controller.isGrounded) {
			if (rollButton && !isRolling && !isRollCooling) {
					isRolling = true;
				rollDirection = this.transform.Find ("RotationPoint").forward;
				}
				if (isRolling == false) {
					moveDirection = new Vector3 (hMovement, 0, vMovement);
					moveDirection = transform.TransformDirection (moveDirection);
					moveDirection *= speed;
					moveDirection.y -= gravity * Time.deltaTime;
					controller.Move (moveDirection * Time.deltaTime);
				}
			//}

		}



		if (Physics.Raycast (transform.position, downDir, out hit, dist)) {
			if (hit.collider.tag == "Ground") {
				
			}
		} else {
			this.transform.position = new Vector3(oldPosition.x, oldPosition.y, oldPosition.z);
		}



	}

	void OnCollisionEnter(Collision col){

		if (col.gameObject.tag == "InstantDeath") {
			isDying = true;
		}
		if (col.gameObject.tag == "Projectile") {

		}

	}

	public void Stun(float stunTime){
		stunTimer = stunTime;
		canMove = false;

	}

	public void Die(){
		Destroy (this.gameObject);
		if (matchManager != null) {
			//matchManager.GetComponent<MatchManager> ().AddDeath (this.gameObject);
		} else {
			Debug.Log ("MatchManager was null");
		}
	}
}
