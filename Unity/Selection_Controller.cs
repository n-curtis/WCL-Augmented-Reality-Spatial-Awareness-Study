/* Copyright 2016 Nicholas Curtis

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License. */
   
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Selection_Controller : MonoBehaviour
{
	public string state; 			// 'H', 'Vx', 'Vy'
	private bool active; 			// flag for drawing the selector
	private bool shuffle; 			// flags for shuffling the cube
	private bool animating; 		// flag for animating a rotation
	private float remaining;			//
	private float rotation;				//
	private float animation_step;			//	animation and rotation values
	private Vector3 point;				//
	private Vector3 axis;				//
	public Text debugText; 			// debug UI element for testing
	private bool debug; 			// flag to enable display of debug text

	public List<GameObject> cubes; 		// vector of cubes to be rotated/manipulated
	
	void Start () // runs on initialization
	{
		debug = true; 		active = true;
		animating = false; 	shuffle = false;
		state = "H";		disable ();
	}

	// called once per frame
	void Update ()
	{
		if (shuffle) {shuffleCube ();}
		if (!(animating)) {
			if (Input.GetButtonDown ("1")) {
				// toggle the selector on or off
				// the selector is rotated one orientation back so that it remains correct
				if (!(active)) {
					if 		(state.Equals  ("H")) {state = "Vy";}
					else if (state.Equals ("Vx")) {state = "H" ;}
					else if (state.Equals ("Vy")) {state = "Vx";}
					drawSelector ();
					active = true;	output ("selector activated");
				} else {disable ();	output ("selector disabled" );}
			}
			if (active) {
				if (Input.GetButtonDown ("2")) {drawSelector ();}
				if (Input.GetButtonDown ("Vertical")) {vertical ();}
				if (Input.GetButtonDown ("Horizontal")) {horizontal ();}
			} else{
				if (Input.GetButtonDown ("3")) {shuffle = true;}
				if (Input.GetButtonDown ("4")) {Application.LoadLevel (0);}
				if (Input.GetButtonDown ("Vertical")) {cube_vertical ();}
				if (Input.GetButtonDown ("Horizontal")) {cube_horizontal ();}
			}
		} else {
			// rotates each cube selected in the slab gradually so that it appears animated
			foreach (GameObject cube in cubes) {
				cube.transform.RotateAround (point, axis, (rotation * Time.deltaTime));
			}
			remaining -= (rotation * Time.deltaTime); output (remaining.ToString());
			if (remaining <= 0.4) {	// when the difference is small enough, the slab is snapped into place
				foreach (GameObject cube in cubes) {cube.transform.RotateAround (point, axis, (remaining));}
				remaining = 0.0f; animating = false; output ("end rotation");
				cubes.Clear (); // list of cubes requiring rotation is cleared
			}
		}
	}

	void horizontal ()
	{
		if (state.Equals ("Vy")) {
			float step = Input.GetAxis ("Horizontal");
			if (step == 0) {step = Random.Range (-1, 1);}
			if (step  < 0) {step = -1;}
			if (step  > 0) {step = 1;}
			output ("input: " + step.ToString ());

			step = transform.localPosition.x + step;
			if (step > 1)  {step = 1;}
			if (step < -1) {step = -1;}
			
			Vector3 movement = new Vector3 (step, transform.localPosition.y, transform.localPosition.z);
			gameObject.transform.localPosition = movement;
		}

		// rotate the slab if the state is 'H' or 'Vx'
		if (state.Equals ("H")) {
			animation_step = Input.GetAxis ("Horizontal");
			if (animation_step == 0) {animation_step = Random.Range (-1, 1);}
			if (animation_step  < 0) {animation_step = -1;}
			if (animation_step  > 0) {animation_step = 1;}
			
			foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube")) {
				if (isApproximately (cube.gameObject.transform.position.y, gameObject.transform.position.y)) {
					cubes.Add (cube);
				}
			}
			animating = true;
			point = Vector3.zero;
			axis = new Vector3 (0, ((90 * animation_step) * -1), 0);
			rotation = 90;		remaining = rotation;
			
		}
		
		if (state.Equals ("Vx")) {
			animation_step = Input.GetAxis ("Horizontal");
			if (animation_step == 0) {animation_step = Random.Range (-1, 1);}
			if (animation_step  < 0) {animation_step = -1;}
			if (animation_step  > 0) {animation_step = 1;}
			
			foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube")) {
				if (isApproximately (cube.gameObject.transform.position.z, gameObject.transform.position.z)) {
					cubes.Add (cube);
				}
			}
			animating = true;
			point = new Vector3 (0, 2, 0);
			axis = new Vector3 (0, 0, ((90 * animation_step) * -1));
			rotation = 90;	remaining = rotation;
		}
	}

	void vertical ()
	{
		if (state.Equals ("H")) {
			float step = Input.GetAxis ("Vertical");
			if (step == 0) {step = Random.Range (-1, 1);}
			if (step  < 0) {step = -1;}
			if (step  > 0) {step = 1;}
			output ("input: " + step.ToString ());
			
			step = transform.localPosition.y + step;
			if (step > 3) {step = 3;}
			if (step < 1) {step = 1;}
			
			Vector3 movement = new Vector3 (transform.localPosition.x, step, transform.localPosition.z);
			gameObject.transform.localPosition = movement;
		} else if (state.Equals ("Vx")) {
			float step = Input.GetAxis ("Vertical");
			if (step == 0) {step = Random.Range (-1, 1);}
			if (step  < 0) {step = -1;}
			if (step  > 0) {step = 1;}
			output ("input: " + step.ToString ());
			
			step = transform.localPosition.z + step;
			if (step > 1) {step = 1;}
			if (step < -1) {step = -1;}
			
			Vector3 movement = new Vector3 (transform.localPosition.x, transform.localPosition.y, step);
			gameObject.transform.localPosition = movement;
		}
		// rotate the slab if the state is 'Vy'
		if (state.Equals ("Vy")) {
			animation_step = Input.GetAxis ("Vertical");
			if (animation_step == 0) {animation_step = Random.Range (-1, 1);}
			if (animation_step  < 0) {animation_step = -1;}
			if (animation_step  > 0) {animation_step = 1;}
			
			foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube")) {
				if (isApproximately (cube.gameObject.transform.position.x, gameObject.transform.position.x)) {
					cubes.Add (cube);
				}
			}
			animating = true;
			point = new Vector3 (0, 2, 0);
			axis = new Vector3 (((90 * animation_step)), 0, 0);
			rotation = 90;	remaining = rotation;
		}
	}

	void cube_horizontal ()
	{
		animation_step = Input.GetAxis ("Horizontal");
		if (animation_step < 0) {animation_step = -1;}
		if (animation_step > 0) {animation_step = 1;}
		foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube")) {cubes.Add (cube);}
		animating = true;
		point = new Vector3 (0, 2, 0);
		axis = new Vector3 (0, 0, ((90 * animation_step) * -1));
		rotation = 90;	remaining = rotation;
	}

	void cube_vertical ()
	{
		animation_step = Input.GetAxis ("Vertical");
		if (animation_step < 0) {animation_step = -1;}
		if (animation_step > 0) {animation_step = 1;}
		foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube")) {cubes.Add (cube);}
		
		animating = true;
		point = new Vector3 (0, 2, 0);
		axis = new Vector3 (((90 * animation_step)), 0, 0);
		rotation = 90;	remaining = rotation;
	}

	void shuffleCube ()
	{
		int input = 0;
		int turns = 0;
		int choice = 0;
		int shuffle_moves;

		shuffle_moves = Random.Range (140, 200);

		// store the current state and orientation of the selector
		Vector3 position = gameObject.transform.localPosition;
		Vector3 scale = gameObject.transform.localScale;
		string state_pre = state;

		while (shuffle_moves > 0) {
			if (turns == 0) {
				turns = Random.Range (3, 16);
				input = Random.Range (0, 3);
				shuffle_moves--;
			}
			if (input == 0) {input = Random.Range (1, 5); drawSelector ();}
			else {
				choice = Random.Range (1, 3);
				if (choice == 1) {horizontal ();}	else {vertical ();}
				turns--;
			}
			// rotate the random slab without animation
			foreach (GameObject cube in cubes) {cube.transform.RotateAround (point, axis, remaining);}
			remaining = 0.0f;
			cubes.Clear ();
		}
		animating = false;	shuffle = false;

		// return the selector to where it was before
		gameObject.transform.localPosition = position;
		gameObject.transform.localScale = scale;
		state = state_pre;

		output ("shuffle completed");
	}

	void drawSelector ()
	{
		Vector3 transform = new Vector3 (0.0f, 0.0f, 0.0f);
		
		if 		(state.Equals  ("H")) 	{transform = new Vector3 (3.1f, 3.1f, 1.1f); 	state = "Vx";}
		else if (state.Equals ("Vx")) 	{transform = new Vector3 (1.1f, 3.1f, 3.1f); 	state = "Vy";}
		else if (state.Equals ("Vy")) 	{transform = new Vector3 (3.1f, 1.1f, 3.1f);	state = "H" ;}
		
		gameObject.transform.localPosition = new Vector3 (0.0f, 2, 0.0f);
		gameObject.transform.localScale = transform;
		output ("State changed to " + state);
	}

	// disable the selector while allowing it to still take input
	void disable ()	{active = false; gameObject.transform.localScale = new Vector3 (0, 0, 0);}

	// for some reason, Mathf.Approximately is useless for this simulation
	bool isApproximately (float a, float b) {if (a - b < 0.4 && b - a < 0.4) {return true;}	else {return false;}}

	// provides debug text on the GUI
	void output (string x) {if (debug) {debugText.text = x;}}
}
