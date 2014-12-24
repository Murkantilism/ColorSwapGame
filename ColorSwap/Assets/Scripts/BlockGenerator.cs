using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Generates batches of colored blocks
public class BlockGenerator : MonoBehaviour{
    
	static int rowsPerBatch = 6; // Number of rows per batch
    
    // Declare an array of rows, an example structue of this array will look like:
	// [[largeBlock], [smallBlock0, smallBlock1], [smallBlock0, smallBlock1], [largeBlock], [smallBlock0, smallBlock1], [smallBlock0, smallBlock1]]
	Block[][] _rows = new Block[rowsPerBatch][];
    
    int numRowsGenerated = 0;
    
	int randy;
	
	// Preset spawn positions assigned via inspector
	public GameObject Center_pos_preset;
	public GameObject Left_pos_preset;
	public GameObject Right_pos_preset;
	
	Block[] lb = new Block[1]; // Declare array of size 1
	Block[] twoSmallBlocks = new Block[2]; // Declare array of size 2
	
	public void Start(){
		randy = Random.Range(0, 4);
	}
	
	public void Update(){
		GenerateBatch();
	}
    
    // Randomly decide the color scheme for the next batch of rows
    // 0 = red-blue
    // 1 = yellow-green
    // 2 = red-white
    public void GenerateBatch(){
        // If we haven't finished generating this batch, don't
        // change the color scheme and call GenerateRow.
		if(numRowsGenerated < rowsPerBatch){
		    GenerateRow(randy);
		// If we have finished this batch, randomly pick
		// another color scheme and call GenerateRow.
		}else{
			randy = Random.Range(0, 4);
			numRowsGenerated = 0; // Reset number of rows generated
			GenerateRow(randy);
		}
    }
    
    // Given a color scheme, randomly generate the next row of blocks
    public void GenerateRow(int colorScheme){
		
        // Randomly decide if this row will be 1 large block or 
        // 2 small blocks and add them to this row array.
        if(Random.Range(0, 2) == 0){
			// Assign associated values to this large block
			lb[0] = new Block();
			lb[0].blockPos = Center_pos_preset.transform.position;
			lb[0].size = 8;

            // Assign the block's color value based on color scheme,
            // then add it to the list of blocks.
            if(colorScheme == 0){
				lb[0].blockColor = Color.Lerp(Color.red, Color.blue, 0.5f); // Purple
            }else if(colorScheme == 1){
				lb[0].blockColor = Color.Lerp(Color.yellow, Color.green, 0.5f); // Light green
            }else{
				lb[0].blockColor = Color.Lerp(Color.red, Color.white, 0.5f); // Pink
            }
            
			_rows[numRowsGenerated] = lb; // Add this block to list of blocks
			numRowsGenerated += 1; // Increment counter

        }else{
			// Declare 2 small blocks with associated values.
			Block sb0 = new Block();
			sb0.blockPos = Left_pos_preset.transform.position;
			sb0.size = 4;
			
			Block sb1 = new Block();
			sb1.blockPos = Right_pos_preset.transform.position;
			sb1.size = 4;
			
			// Randomly assign opposite block colors to each small block based
			// on the color scheme, then add them both to the list of blocks.
			if(colorScheme == 0){
			    if(Random.Range(0, 2) == 0){
			        sb0.blockColor = Color.red;
			        sb1.blockColor = Color.blue;
			    }else{
					sb0.blockColor = Color.blue;
					sb1.blockColor = Color.red;
			    }
			}else if(colorScheme == 1){
				if(Random.Range(0, 2) == 0){
					sb0.blockColor = Color.yellow;
					sb1.blockColor = Color.green;
				}else{
					sb0.blockColor = Color.green;
					sb1.blockColor = Color.yellow;
				}
			}else{
				if(Random.Range(0, 2) == 0){
					sb0.blockColor = Color.red;
					sb1.blockColor = Color.white;
				}else{
					sb0.blockColor = Color.white;
					sb1.blockColor = Color.red;
				}
			}
			// Add these 2 small blocks to list of small blocks
			twoSmallBlocks[0] = sb0;
			twoSmallBlocks[1] = sb1;
			_rows[numRowsGenerated] = twoSmallBlocks;
			numRowsGenerated += 1; // Increment counter
        }
        
        
        foreach(Block[] r in _rows){
            if(r.Length == 1){
                Debug.Log("LARGE BLOCK: " + r[0].blockColor + " | " + r[0].blockPos + " | " + r[0].size);
			}else if(r.Length == 2){
				Debug.Log("SMALL BLOCK 0: " + r[0].blockColor + " | " + r[0].blockPos + " | " + r[0].size);
				Debug.Log("SMALL BLOCK 1: " + r[1].blockColor + " | " + r[1].blockPos + " | " + r[1].size);
			}
        }
    }
}
