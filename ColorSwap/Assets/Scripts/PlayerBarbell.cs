using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBarbell : MonoBehaviour {
	
	// Reference to the player's balls ;) assigned via inspector
	public GameObject ball0;
	public GameObject ball1;

	// Declare public player variabls for color, position
    public Color ball0_color = new Color(0, 0, 0);
    public Color ball1_color = new Color(0, 0, 0);
    
    string current_shape = "circle";
    
    // Declare a list of available shapes (always size 2)
	List<string> available_shapes = new List<string>();
    
	// Use this for initialization
	void Start () {
		available_shapes.Add("triangle");
		available_shapes.Add("square");
		Flick();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// Swap the positions of both balls
	public void Tap(){
	    // Temporarily save ball 0's position
	    Vector2 tmp_pos = ball0.transform.position;
	    // Move ball 0 to ball 1's position
	    ball0.transform.position = ball1.transform.position;
	    // Move ball 1 to temp position
	    ball1.transform.position = tmp_pos;
	}
	
	// Given an RGB value change both ball's color to the given color (occurs durng pinch)
	public void Pinch(Color rgb){
	    ball0.renderer.material.color = new Color(rgb.r, rgb.g, rgb.b, rgb.a);
		ball1.renderer.material.color = new Color(rgb.r, rgb.g, rgb.b, rgb.a);
	}
	
	// When the player flicks, assign a new shape and color to barbell
	public void Flick(){
		AssignNewShape();
		InstantiateNewShape();
		DeleteOldShape();
	}
	
	// Randomly select one of the two elements in the list of available shapes.
	// Remove the newly selected shape & add the current shape to the list.
	void AssignNewShape(){
	    string newShape;
	    	    
	    if (Random.Range(0, 1) == 0){
	        newShape = available_shapes[0];
	        available_shapes.Remove(newShape);
	        available_shapes.Add(current_shape);
	        current_shape = newShape;
	    }else{
			newShape = available_shapes[1];
			available_shapes.Remove(newShape);
			available_shapes.Add(current_shape);
			current_shape = newShape;
	    }
	}
	
	// Instantiate the newly selected shape
	void InstantiateNewShape(){
	    // TODO: Instantiate shape from Resources folder
	}
	
	void DeleteOldShape(){
	   // TODO: Delete the previous shape's gameobjects
	}
}
