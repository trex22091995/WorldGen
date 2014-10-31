using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour {


	public GameObject obj;

	//public Transform PlayerPosition;
	public float PlayerChunkLoading;
	public int ChunkSize = 100;
	public float VerticalNoise = 20.0f;
	public float HorizontalNoise = 20.0f;

	public float CubeSize = 1.0f;

	private bool first = true;

	private bool randomHoch = true;


	private List<cubes> world;

	// Update is called once per frame
	void Update () {
		world = new List<cubes> ();
		if(first){
			GenWorld(0, 0, 0);
			first = false;
		}
	}

	void GenWorld(int posX, int posY, int ID){
		//posX und posY geben den Mittelpunkt des würfels an!
		cubes temp = new cubes();
		temp.posX = posX;
		temp.posY = posY;
		temp.ID = ID;

		//if ((ID - ChunkSize) < 0) {
		if (ID == 0) {
			//Instantiate (obj,new Vector3((float)posX, (float)genHeight(i,j),(float)posY), Quaternion.identity);	
			temp.HeightLinks = 0.0f;
			temp.HeightHinten = 0.0f;
			temp.Height = 0.0f;
			temp.Cube = Instantiate (obj, new Vector3 ((float)0.0f, 0.0f, (float)posY), Quaternion.identity);	

		} else if (ID < ChunkSize) {
			temp.HeightLinks = 0.0f;
			temp.HeightHinten = world [ID - 1].Height;
			temp.Height = genHeight (0.0f, 0.0f);
			//temp.Height = genHeight(world[ID-1].Height);

			temp.Cube = Instantiate (obj, new Vector3 ((float)posX, temp.Height, (float)posY), Quaternion.identity);
		} else {
			temp.HeightLinks = world[ID-ChunkSize].Height;
			temp.HeightHinten = world [ID - 1].Height;
			temp.Height = genHeight (temp.HeightHinten,temp.HeightLinks);
			//temp.Height = genHeight(world[ID-1].Height);
			
			temp.Cube = Instantiate (obj, new Vector3 ((float)posX, temp.Height, (float)posY), Quaternion.identity);
		}


		world.Add (temp);
		if(ID < (ChunkSize*ChunkSize)-1){
			if(posX+1 >= ChunkSize){
				//Debug.Log ("neue Reihe");

				GenWorld (0,posY + 1,ID + 1); 
			}else{
				//Debug.Log ("alte Reihe");

				GenWorld(posX + 1,posY,ID+1);
			}

		}



	}

	float genHeight(float oldHeight,float linksHeight){
		float Height = 0;
		int rand = Random.Range(0,30);
		if((oldHeight-linksHeight) >= 2){
			return oldHeight -linksHeight;
		}
		if((oldHeight-linksHeight) <= -2){
			return linksHeight - oldHeight;
		}
		if(rand == 0){
			randomHoch = !randomHoch;
			Height = oldHeight;
		}else if (rand >= 1 && rand < 25) {
			Height = oldHeight;
		
		}else {
			if (randomHoch) {
				
				Height = oldHeight + 1;
				
			} else {
				Height = oldHeight - 1;	
			}	
		}

		return Height;
	}
}

public class cubes{
	public int posX;
	public int posY;
	public float Height;
	public int ID;
	public float HeightRechts;
	public float HeightLinks;
	public float HeightVorne;
	public float HeightHinten;
	public object Cube;
}

