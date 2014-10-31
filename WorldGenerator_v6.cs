using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator_v6 : MonoBehaviour {

	public int ChunkSize = 20;
	public GameObject prefab;

	private List<Block> World = new List<Block>();
	private int currentID = 0;

	private bool rauf = true;




	public int Ebene = 50;
	public int Wechsel = 10;

	private int wegweiser = 0; //eine Variable die alle reihen mitzählt
	private bool wechselreihe = true;//die variable ist für die erste Schlussreihe zuständig


	void Start() {
				Generate ();
		ListOutput ();
		}

	void Generate() {
		for (int i = 0; i < ChunkSize; i++) {
			int x = i ,z = 0;
			for (int j = i + 1; j > 0; j--) {
				CreateCube(x,z,i,j,false);
				x--;
				z++;

			}
			wegweiser = i;
		}

		int firstX = ChunkSize - 1;
		int firstZ = 1;

		wegweiser--;

		for (int i = ChunkSize; i > 0; i--) {
			wegweiser ++;
			int x = firstX; 
			int z = firstZ;

			for (int j = i + 1; j > 2; j--) {
				CreateCube(x,z,i,j,true);
				x--;
				z++;
			}
			wechselreihe = false;
			//firstX --;
			firstZ ++;
		}


	}

	void CreateCube(int x, int z, int i, int j, bool schluss) {
		Block tempBlock;
		float tempHeight;

		if (schluss == false) {
			if (currentID == 0) {
				tempHeight = GenerateHeight (i, j, 3);
			} else {
				if (x == 0) {
					tempHeight = GenerateHeight (i, j, 0);
				} else if (z == 0) {
					tempHeight = GenerateHeight (i, j, 1);
				} else {
					tempHeight = GenerateHeight (i, j, 2);
				}
			}
		} else {
			tempHeight = GenerateHeight(i,j,4);
		}



		tempBlock = new Block(Instantiate (prefab, new Vector3(x, tempHeight , z), new Quaternion(0,0,0,Quaternion.identity.w)), tempHeight, currentID);
		currentID++;
		World.Add (tempBlock);
	}

	float GenerateHeight(int i, int j, int opID) {
		//opID: 0: x=0 ; 1: z=0 ; 2: alle anderen; 3: der aller erste Block; 4: schluss!
		int rand = 0;
		float outHeight = 0f;
		Block a, b;
		switch		 (opID) {
		case 0:		//x = 0
			a = World[currentID - i - 1];
			if(rauf){
				outHeight=a.height + 1f;
			}else{
				outHeight=a.height - 1f;
				
			}
			
			rand = Random.Range(1,100);
			if (rand > Ebene) {
				outHeight = a.height;
				
			}

			break;

		case 1:		//z = 0
			a = World[currentID - i];
			if(rauf){
				outHeight=a.height + 1f;
			}else{
				outHeight=a.height - 1f;
				
			}
			
			rand = Random.Range(1,100);
			if (rand > Ebene) {
				outHeight = a.height;
				
			}
			break;

		case 2:
			a = World[currentID - i - 1];			//out of range am Anfang
			b = World[currentID - i];				//same here
			if (a.height != b.height) {
				if(Mathf.Abs(a.height - b.height) > 1){
					outHeight = Mathf.Max(a.height,b.height) - 1f;
				}else{

					if(rauf){
						outHeight=Mathf.Max(a.height,b.height);
					}else{
						outHeight=Mathf.Min(a.height,b.height);

					}



				}
			}
			else {

				if(rauf){
					outHeight=a.height + 1f;
				}else{
					outHeight=a.height - 1f;
					
				}

				rand = Random.Range(1,100);
				if (rand > Ebene) {
					outHeight = a.height;

				}
			}
			rand = Random.Range(0,100);
			if (rand <= Wechsel)
				rauf = !rauf;
			break;
		case 3://der aller erste Block!

			outHeight = 0.0f;

			break;

		case 4://der schluss
			if(wechselreihe){
				a = World[currentID - i -2];			//out of range am Anfang
				b = World[currentID - i -1];
			}else{
				a = World[currentID - i -1-2];			//out of range am Anfang
				b = World[currentID - i -2];
			}


			Debug.Log ("MyID: " + currentID + " Chunksize: " + ChunkSize + " und das i: " + i);
			if (a.height != b.height) {
				if(Mathf.Abs(a.height - b.height) > 1){
					outHeight = Mathf.Max(a.height,b.height) - 1f;
				}else{
					
					if(rauf){
						outHeight=Mathf.Max(a.height,b.height);
					}else{
						outHeight=Mathf.Min(a.height,b.height);
						
					}
					
					
					
				}
			}
			else {
				
				if(rauf){
					outHeight=a.height + 1f;
				}else{
					outHeight=a.height - 1f;
					
				}
				
				rand = Random.Range(1,100);
				if (rand > Ebene) {
					outHeight = a.height;
					
				}
			}
			rand = Random.Range(0,100);
			if (rand <= Wechsel)
				rauf = !rauf;
			
			break;
			
		}


		return outHeight;
	}

	void ListOutput() {
		Debug.Log (World.Count);
	}
}

public class Block {
	public float height;
	public Object obj;
	public int BlockID;

	public Block(Object listObject, float inputHeight, int currentID) {
		obj = listObject;
		height = inputHeight;
		BlockID = currentID;
		obj.name = currentID.ToString();

	}
}
