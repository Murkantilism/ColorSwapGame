using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// NOTE: This script must be attached to the Ball0 GameObject
public class PlayerBarbell : MonoBehaviour {
	
	// Reference to the player's balls ;) assigned via inspector
	public GameObject ball0;
	public GameObject ball1;

	// Declare public player variabls for color
    public Color ball0_color = new Color(0, 0, 0);
    public Color ball1_color = new Color(0, 0, 0);
    
    string current_shape = "circle";
    
    // Declare a list of available shapes (always size 2)
	List<string> available_shapes = new List<string>();
	
	public ScoreKeeper sk;
	
	private float fingerStartTime  = 0.0f;
	private Vector2 fingerStartPos = Vector2.zero;
	
	private bool isSwipe = false;
	private float minSwipeDist  = 50.0f;
	private float maxSwipeTime = 0.5f;
	
	float pinchStart_DistThreshMin = 500.0f; // The minimum distance to register the start a pinch event
	float pinchStart_DistThreshMax = 750.0f; // The maximum distance to register the start a pinch event
	float pinchEnd_DistThresh = 250.0f; // The threshold distance to register completion of a pinch event
	
	Ray ray;
	RaycastHit hit;
	Ray ray1;
	RaycastHit hit1;

	string debug0;
	string debug1;
	string debug2;
	string debug3;

	GUIStyle style = new GUIStyle();

	bool swapBalls = true; // Have the balls swapped?
	float ballSwapSpeed = 5.0f;
	bool ballsSwapping = false; // Are the balls in the middle of the swapping animation?
	Vector2 tmp_pos0;
	Vector2 tmp_pos1;

	float degrees0;
	float speed0 = 2.0f;
	float amplitude0 = 0.025f;
	float period0 = 2.0f;
	
	float degrees1;
	float speed1 = 2.0f;
	float amplitude1 = 0.025f;
	float period1 = 2.0f;

    
	// Use this for initialization
	void Start () {
	    // Set initial ball color
	    ball0_color = Color.red;
		ball1_color = Color.blue;
		ball0.GetComponent<Renderer>().material.color = ball0_color;
		ball1.GetComponent<Renderer>().material.color = ball1_color;
		
		available_shapes.Add("triangle");
		available_shapes.Add("square");

		// Temporarily save both ball positions
		tmp_pos0 = ball0.transform.position;
		tmp_pos1 = ball1.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// If any touches are detected, chech if they are in the right area
		if(Input.touchCount > 0){
			ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			Debug.Log(ray.origin);
			debug2 = ray.origin.ToString();
			
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)){
				// Detect if player is touching in the correct area of the screen
				if(hit.collider!=null && hit.collider.name == "BarbellArea"){
					//debug3 = hit.collider.name;
					// If the correct area is being touched, check how many touches.
					// If it's one, check for Tap or Flick.
					// If it's two, check for Pinch.
					if(Input.touchCount == 1){
						Touch touch = Input.touches[0];
						
						switch(touch.phase){
							case TouchPhase.Began :
								// This is a new touch
								isSwipe = true;
								fingerStartTime = Time.time;
								fingerStartPos = touch.position;
								break;
							case TouchPhase.Stationary :
								// This is a tap
								Debug.Log("Tap event occured");
								debug0 = "Tap event occured";

							    // Check if ray is in bounds of ball0's area
								if((ray.origin.x < -1.4f && ray.origin.x > -2.8f) &&
							       (ray.origin.y < -13.5 && ray.origin.y > -14.5f)){
								    //debug3 = "Ball0 tapped";    
								    if(ballsSwapping == false){
									    ballsSwapping = true;
									    StartCoroutine("Tap");
								    }
								// Check if ray is in bounds of ball1's area
							    }else if((ray.origin.x < 2.5f && ray.origin.x > 1.5f) &&
							             (ray.origin.y < -13.5 && ray.origin.y > -14.5f)){
									//debug3 = "Ball1 tapped"; 
								    if(ballsSwapping == false){
									    ballsSwapping = true;
								        StartCoroutine("Tap");
								    }
							    }
								break;
							case TouchPhase.Canceled :
								// The touch is being canceled
								isSwipe = false;
								break;
							case TouchPhase.Ended : 
								float gestureTime = Time.time - fingerStartTime;
								float gestureDist = (touch.position - fingerStartPos).magnitude;
								
								if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist){
									Vector2 direction = touch.position - fingerStartPos;
									Vector2 swipeType = Vector2.zero;
									
									if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
										// the swipe is horizontal:
										swipeType = Vector2.right * Mathf.Sign(direction.x);
									}else{
										// the swipe is vertical:
										swipeType = Vector2.up * Mathf.Sign(direction.y);
									}
									
									if(swipeType.x != 0.0f){
										if(swipeType.x > 0.0f){
											// MOVE RIGHT
											Debug.Log("Swipe R event occurred");
											debug0 = "Swipe R event occured";
										}else{
											// MOVE LEFT
										}
									}
									
									if(swipeType.y != 0.0f ){
										if(swipeType.y > 0.0f){
											// MOVE UP
										}else{
											// MOVE DOWN
										}
									}
								}
							break;
						}
					}else if(Input.touchCount == 2){
						Vector2 touch0, touch1;
						float distance;
						touch0 = Input.GetTouch(0).position;
						touch1 = Input.GetTouch(1).position;
						
						distance = Vector2.Distance(touch0, touch1);
						Debug.Log("Distance: " + distance.ToString());
						debug1 = "Distance: " + distance.ToString();
						
						if(Input.GetTouch(0).phase == TouchPhase.Began){
							if (distance < pinchEnd_DistThresh){
								//FIXME: Pinch event doesn't get triggered
								Debug.Log("Pinch event occurred!");
								debug0 = "Pinch event occurred";
							}

							// First check if two touches are far enough to be a potential pinch
							// to prevent very close taps from being registered as pinches.
							//if(distance > pinchStart_DistThreshMin && distance < pinchStart_DistThreshMax){
								// Then check if the distance has become small enough to
								// be considered a complete pinch event.

							//}
						}
					}
				}
			}
		}
	}
	
	// Swap the positions of both balls
	public IEnumerator Tap(){
		Vector2 ball0_pos = ball0.transform.position;
		Vector2 ball1_pos = ball1.transform.position;

		// Move ball 0 left to right, curving above barbell
		// Move ball 1 right to left, curving below barbell
		if(swapBalls == true){
			// Move each ball along a cosine curve until they reach the right postion
			if(ball0.transform.position.x < tmp_pos1.x){
				ball0_pos.x += Time.deltaTime * speed0;
				
				// Update degrees
				float degreesPerSec = 360.0f / period0;
				degrees0 = Mathf.Repeat(degrees0 + (Time.deltaTime * degreesPerSec), 360.0f);
				float radians = degrees0 * Mathf.Deg2Rad;
				
				// Offset by sin wave
				Vector2 offset = new Vector2(0.0f, amplitude0 * Mathf.Sin(radians));
				ball0.transform.position = new Vector2(ball0_pos.x + offset.x, ball0_pos.y + offset.y);
				
				
				// Move ball 1 along a cosine curve (opposite direction)
				ball1_pos.x -= Time.deltaTime * speed1;
				
				// Update degrees
				float degreesPerSec1 = 360.0f / period1;
				degrees1 = Mathf.Repeat(degrees1 + (Time.deltaTime * degreesPerSec1), 360.0f);
				float radians1 = degrees1 * Mathf.Deg2Rad;
				
				// Offset by sin wave
				Vector2 offset1 = new Vector2(0.0f, amplitude1 * Mathf.Sin(radians1));
				ball1.transform.position = new Vector2(ball1_pos.x + offset1.x, ball1_pos.y - offset1.y);
			}else{
				swapBalls = !swapBalls;
			}

		// Move ball 0 left to right, curving above barbell
		// Move ball 1 right to left, curving below barbell
		}else if(swapBalls == false){
			// Move ball 0 along a cosine curve
			if(ball1.transform.position.x > tmp_pos0.x){
				ball0_pos.x -= Time.deltaTime * speed0;
				
				// Update degrees
				float degreesPerSec = 360.0f / period0;
				degrees0 = Mathf.Repeat(degrees0 + (Time.deltaTime * degreesPerSec), 360.0f);
				float radians = degrees0 * Mathf.Deg2Rad;
				
				// Offset by sin wave
				Vector2 offset = new Vector2(0.0f, amplitude0 * Mathf.Sin(radians));
				ball0.transform.position = new Vector2(ball0_pos.x + offset.x, ball0_pos.y - offset.y);
				
				
				// Move ball 1 along a cosine curve
				ball1_pos.x += Time.deltaTime * speed1;
				
				// Update degrees
				float degreesPerSec1 = 360.0f / period1;
				degrees1 = Mathf.Repeat(degrees1 + (Time.deltaTime * degreesPerSec1), 360.0f);
				float radians1 = degrees1 * Mathf.Deg2Rad;
				
				// Offset by sin wave
				Vector2 offset1 = new Vector2(0.0f, amplitude1 * Mathf.Sin(radians1));
				ball1.transform.position = new Vector2(ball1_pos.x + offset1.x, ball1_pos.y + offset1.y);
			}else{
				swapBalls = !swapBalls;
			}
		}

		// Reset boolean switch
		ballsSwapping = false;

		yield return null;
	}
	
	// Given an RGB value change both ball's color to the given color (occurs durng pinch)
	public void Pinch(Color rgb){
	    ball0.GetComponent<Renderer>().material.color = new Color(rgb.r, rgb.g, rgb.b, rgb.a);
		ball1.GetComponent<Renderer>().material.color = new Color(rgb.r, rgb.g, rgb.b, rgb.a);
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
	
	// When this ball collides with something check what it is.
	void OnTriggerEnter2D(Collider2D col){
	    // When a block collides with this block, check if colors are the same
		if(col.gameObject.name == "Block"){
		    if(col.gameObject.GetComponent<Renderer>().material.color == ball0_color){
		        Debug.Log(gameObject.name + " and Block are same color");
				sk.SetNumLives(-1);
		    }else{
		        Debug.Log(gameObject.name + " and Block are different colors");
				sk.SetScore(1);
		    }
			//Debug.Log("Block hit" + col.gameObject.renderer.material.color);
		}
		
		// When the balls overlap with each other, set ball 0's color to the
		// combined color, and turn of the render for ball 1.
		if(col.gameObject.name == "Ball1"){
		    // Combine colors
		    ball0.GetComponent<Renderer>().material.color = Color.Lerp(ball0.GetComponent<Renderer>().material.color, ball1.GetComponent<Renderer>().material.color, 0.5f);
			// Hide other ball
			ball1.GetComponent<Renderer>().enabled = false;
		}
	}
	
	// When balls no longer overlap, set the colors back to original and re-enable ball 1's renderer
	void OnTriggerExit2D(Collider2D col){
		ball0.GetComponent<Renderer>().material.color = ball0_color;
		ball1.GetComponent<Renderer>().material.color = ball1_color;

		ball1.GetComponent<Renderer>().enabled = true;

		/*
		 * 
		// When the balls no longer overlap with each other, set ball 0's color 
		// to the opposite of ball 1's color, re-enable ball 1's renderer.
		if(col.gameObject.name == "Ball1"){
	        // First calculate the HSV value
	        Color ball1_hsv = RGBToHSV(ball1.GetComponent<Renderer>().material.color);
	        // Then change the hue value to the opposite hue, leave saturation and value (g, b) unchanged
	        Color opp_ball1_hsv = new Color(360 - ball1_hsv.r, ball1_hsv.g, ball1_hsv.b); // NOTE: R is H aka Hue in this case
	        // Convert back to RGB
	        Color opp_ball1_rgb = HSVToRGB(opp_ball1_hsv.r, opp_ball1_hsv.g, opp_ball1_hsv.b);
	        
	        // Set ball 0's color to the newly computed opposite color
	        ball0.GetComponent<Renderer>().material.color = opp_ball1_rgb;
	    }*/
	}

	#region HELPER METHODS
	// Helper method to calculate HSV values (to get opposite colors)
	public Color RGBToHSV(Color RGB){
	    int max = (int)Mathf.Max(RGB.r, Mathf.Max(RGB.g, RGB.b));
	    int min = (int)Mathf.Min(RGB.r, Mathf.Min(RGB.g, RGB.b));
	    
	    float hue = (float)GetHue(RGB.r, RGB.g, RGB.b, max, min);
		float saturation = (float)((max == 0) ? 0 : 1d - (1d * min / max));
		float value = (float)(max / 255d);
	    
	    // Return HSV value
	    return new Color(hue, saturation, value);
	}
	
	// Helper function to calculate hue
	public int GetHue(float red, float green, float blue, int max, int min) {
		float hue = 0f;
		if (max == red) {
			hue = (green - blue) / (max - min);
			
		} else if (max == green) {
			hue = 2f + (blue - red) / (max - min);
			
		} else {
			hue = 4f + (red - green) / (max - min);
		}
		
		hue = hue * 60;
		if (hue < 0) hue = hue + 360;
		
		return (int)Mathf.Round(hue);
	}
	
	// Helper method to calculate RGB values (to get opposite colors)
	public static Color HSVToRGB(float H, float S, float V)
	{
		if (S == 0f){
			return new Color(V,V,V);
		}else if (V == 0f){
			return Color.black;
		}else{
			Color _color = Color.black;
			float Hval = H * 6f;
			int sel = Mathf.FloorToInt(Hval);
			float mod = Hval - sel;
			float v1 = V * (1f - S);
			float v2 = V * (1f - S * mod);
			float v3 = V * (1f - S * (1f - mod));
			switch (sel + 1)
			{
			case 0:
				_color.r = V;
				_color.g = v1;
				_color.b = v2;
				break;
			case 1:
				_color.r = V;
				_color.g = v3;
				_color.b = v1;
				break;
			case 2:
				_color.r = v2;
				_color.g = V;
				_color.b = v1;
				break;
			case 3:
				_color.r = v1;
				_color.g = V;
				_color.b = v3;
				break;
			case 4:
				_color.r = v1;
				_color.g = v2;
				_color.b = V;
				break;
			case 5:
				_color.r = v3;
				_color.g = v1;
				_color.b = V;
				break;
			case 6:
				_color.r = V;
				_color.g = v1;
				_color.b = v2;
				break;
			case 7:
				_color.r = V;
				_color.g = v3;
				_color.b = v1;
				break;
			}
			_color.r = Mathf.Clamp(_color.r, 0f, 1f);
			_color.g = Mathf.Clamp(_color.g, 0f, 1f);
			_color.b = Mathf.Clamp(_color.b, 0f, 1f);
			return _color;
		}
	}
	#endregion

	// GUI used for debugging on mobile
	void OnGUI(){
		style.fontSize = 30;
		GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200, 20), debug0, style);
		GUI.Label(new Rect(Screen.width / 2, (Screen.height / 2) + 40, 200, 20), debug1, style);
		GUI.Label(new Rect(Screen.width / 2, (Screen.height / 2) + 80, 200, 20), debug2, style);
		GUI.Label(new Rect(Screen.width / 2, (Screen.height / 2) + 120, 200, 20), debug3, style);
	}
}