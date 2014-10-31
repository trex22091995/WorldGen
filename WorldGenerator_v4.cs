﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator_v4 : MonoBehaviour {
	
	public int ChunkSize = 20;
	public GameObject prefab;
	
	private List<Block_v4> World = new List<Block_v4>();
	private int currentID = 0;
	
	private bool rauf = true;

	private Mesh mesh;
	
	
	
	public int Ebene = 50;
	public int Wechsel = 10;
	
	
	
	
	void Start() {
		gameObject.AddComponent("MeshFilter");
		gameObject.AddComponent("MeshRenderer");
		mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear();

		Generate ();
		//ListOutput ();


		Vector3[] vert = new Vector3[currentID];

		Vector2[] uvs = new Vector2[currentID];

		List<int> Dreieck = new List<int> ();

		int count = 0;




		uvs[0] = new Vector2(0,0);
		uvs[1] = new Vector2(0,256);
		uvs[2] = new Vector2(256,256);
		uvs[3] = new Vector2(256,0);


		foreach (Block_v4 temp in World) {
			vert[count] = temp.pos - transform.position;
			if(count >= 2){
				Dreieck.Add(count);
				Dreieck.Add(count-1);
				Dreieck.Add(count-2);
			}else{
				Dreieck.Add(0);
				Dreieck.Add(1);
				Dreieck.Add(2);
			}


			count ++;
		}

		int[] triangless = new int[Dreieck.Count];
		for(int i = 0; i<Dreieck.Count;i++){
			triangless[i] = Dreieck[i];

		} 

		mesh.vertices = vert;
		mesh.triangles = triangless;
		mesh.uv = uvs;
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();


	}
	
	void Generate() {
		for (int i = 0; i < ChunkSize; i++) {
			int x = i ,z = 0;
			for (int j = i + 1; j > 0; j--) {
				CreateCube(x,z,i,j);
				x--;
				z++;
			}
		}
	}
	
	void CreateCube(int x, int z, int i, int j) {
		Block_v4 tempBlock;
		float tempHeight;
		
		if (currentID == 0) {
			tempHeight = GenerateHeight(i,j,3);
		} else {
			if (x == 0) {
				tempHeight = GenerateHeight (i, j, 0);
			} 
			else if (z == 0) {
				tempHeight = GenerateHeight (i, j, 1);
			} 
			else {
				tempHeight = GenerateHeight (i, j, 2);
			}
		}
		
		
		tempBlock = new Block_v4(Instantiate (prefab, new Vector3(x, tempHeight , z), new Quaternion(0,0,0,Quaternion.identity.w)), tempHeight, currentID, new Vector3(x, tempHeight , z));
		currentID++;
		World.Add (tempBlock);
	}
	
	float GenerateHeight(int i, int j, int opID) {
		//opID: 0: x=0 ; 1: z=0 ; 2: alle anderen
		float outHeight = 0f;
		Block_v4 a, b;
		switch (opID) {
		case 0:		//x = 0
			a = World[currentID - i - 1];
			if(rauf){
				outHeight=a.height + 1f;
			}else{
				outHeight=a.height - 1f;
				
			}
			
			int rande = Random.Range(1,100);
			if (rande > Ebene) {
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
			
			int randos = Random.Range(1,100);
			if (randos > Ebene) {
				outHeight = a.height;
				
			}
			break;
			
		case 2:
			a = World[currentID - i - 1];			//out of range am Anfang
			b = World[currentID - i];		//same here
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
				
				int rand = Random.Range(1,100);
				if (rand > Ebene) {
					outHeight = a.height;
					
				}
			}
			int rando = Random.Range(0,100);
			if (rando <= Wechsel)
				rauf = !rauf;
			break;
		case 3://der aller erste Block!
			
			outHeight = 0.0f;
			
			break;
			
		}
		
		
		return outHeight;
	}
	
	void ListOutput() {
		Debug.Log (World.Count);
	}
}

public class Block_v4 {
	public float height;
	public Object obj;
	public Vector3 pos;
	public int BlockID;
	
	public Block_v4(Object listObject, float inputHeight, int currentID, Vector3 Position) {
		obj = listObject;
		height = inputHeight;
		BlockID = currentID;
		obj.name = currentID.ToString();
		pos = Position;
		
	}
}
