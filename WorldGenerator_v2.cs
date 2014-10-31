using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator_v2 : MonoBehaviour {
	
	
	public GameObject obj;
	
	//public Transform PlayerPosition;
	public float PlayerChunkLoading;
	public int ChunkSize = 9;
	public float VerticalNoise = 20.0f;
	public float HorizontalNoise = 20.0f;
	
	public float CubeSize = 1.0f;
	
	private bool first = true;
	
	private bool randomHoch = true;
	
	
	private List<cubes_v2> world;


	private int rich = 0;


	private int kreis = 0;//(2n+1)*(2n+1)
	private int seite = 0;


	
	// Update is called once per frame
	void Update () {
		world = new List<cubes_v2> ();
		if(first){
			GenWorld(0, 0, 0);
			first = false;
		}
	}
	
	void GenWorld(int posiX, int posiY, int ID){
		//posX und posY geben den Mittelpunkt des würfels an!


		//system: von dem mittelpunkt kreise drumherumziehen!
		//chance von 10%, dass der block bei der eigenschaft, dass der block hinter ihm und der links von ihm die gleiche höhe haben.




		cubes_v2 temp = new cubes_v2();

		temp.ID = ID;
		
		//if ((ID - ChunkSize) < 0) {
		if (ID == 0) {
			//Instantiate (obj,new Vector3((float)posX, (float)genHeight(i,j),(float)posY), Quaternion.identity);
			temp.posX = 0.0f;
			temp.posY = 0.0f;
			temp.HeightDavor = 0.0f;
			temp.Height = 0.0f;
			temp.Cube = Instantiate (obj, new Vector3 ((float)0.0f, 0.0f, (float)posiY), Quaternion.identity);	

		} else {
			Vector2 neuePos = new Vector2 ();
			neuePos = neuerWürfelXZ (ID, new Vector2(posiX,posiY));
			float x = neuePos.x;
			float y = neuePos.y;
			temp.posX = x;
			temp.posY = y;
			temp.HeightDavor = world[ID-1].Height;
			cubes_v2 derNebenMir;
			int IDdesNachbarn = 0;
			if(ID > 7){
				IDdesNachbarn = 2;
			}else if(ID > 10){
				IDdesNachbarn = 3;
			} 

			temp.Height = genHeight(temp.HeightDavor,world[IDdesNachbarn].Height);
			temp.Cube = Instantiate (obj, new Vector3 ((float)x, 0.0f, (float)y), Quaternion.identity);

		}
		
		
		world.Add (temp);
		if(ID < (ChunkSize*ChunkSize)-1){
			if(posiX+1 >= ChunkSize){
				//Debug.Log ("neue Reihe");
				
				GenWorld (0,posiY + 1,ID + 1); 
			}else{
				//Debug.Log ("alte Reihe");
				
				GenWorld(posiX + 1,posiY,ID+1);
			}
			
		}
		
		
	}


	#region
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
	#endregion

	private bool grade(int Zahl){
		//ist Zahl grade?
		if (Zahl % 2 == 0) {
			return true;
		} else {
			return false;
		}

	}

	private Vector2 neuerWürfelXZ(int ID, Vector2 vorherigerCubePos){
		Vector2 pos;
		float posX = vorherigerCubePos.x;
		float posY = vorherigerCubePos.y;
		if(grade (ID) == true){

			Debug.Log ("richtungswechsel!");
			rich ++;
			if(rich >= 4){
				rich = 0;
			}
		}

		switch (rich) {
				
		case 0: 
			posY = posY - 1.0f;
			pos = new Vector2 (posX, posY);
			return pos;
			break;

		case 1:
			posX = posX - 1.0f;
			pos = new Vector2 (posX, posY);
			return pos;
			break;

		case 2: 
			posY = posY + 1.0f;
			pos = new Vector2 (posX, posY);
			return pos;
			break;

		case 3:
			posX = posX + 1.0f;
			pos = new Vector2 (posX, posY);
			return pos;
			break;

		default:
			Debug.LogError("rich war zu hoch!!!");

			break;
		}


		pos = new Vector2 (posX, posY);
		return pos;



	}
}

public class cubes_v2{
	public float posX;
	public float posY;
	public float Height;
	public int ID;
	public float HeightDavor;
	public object Cube;
}


