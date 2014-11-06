using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator_v6 : MonoBehaviour
{

		/// <summary>
		/// The size of the chunk.
		/// </summary>
		public int ChunkSize = 20;
		/// <summary>
		/// The prefab.
		/// </summary>
		public GameObject prefab;
		private List<Block> World = new List<Block> ();
		private int currentID = 0;
		private bool rauf = true;
		/// <summary>
		/// Umso kleiner die Variable ist, um so größer ist die Chance auf eine Ebene.
		/// </summary>
		public int Ebene = 50;
		/// <summary>
		/// Umso größer die Variable ist, um so größer ist die Chance auf "Hoch-Runter"-Wechsel.
		/// </summary>
		public int Wechsel = 10;
		private int wegweiser = 0; //eine Variable die alle reihen mitzählt
		private bool wechselreihe = true;//die variable ist für die erste Schlussreihe zuständig
		/// <summary>
		/// The player um festzustellen, ob ein neuer Chunk geladen werden muss.
		/// </summary>s
		public Transform Player;
		/// <summary>
		/// The load chunk in distance.
		/// </summary>
		public float loadChunkInDistance = 5f;

		void Start ()
		{
				Generate ();
				ListOutput ();
		}

		void Update ()
		{
			//wenn der Player an den Rand des Chunks kommt, soll ein neuer Chunk erstellt werden. 
			//dann muss die funktion in sofern verändert werden, dass ein chunk nicht nur von rechts oben nach links unten erstellt wird, 
			//sonder auch je nach dem, wo der Chunk hinzugefügt wird, sinnvoll editiert wird.


		}

		void Generate ()
		{
				for (int i = 0; i < ChunkSize; i++) {
						int x = i, z = 0;
						for (int j = i + 1; j > 0; j--) {
								CreateCube (x, z, i, j, false);
								x--;
								z++;
				
						}
						wegweiser = i;
				}
		
				int firstX = ChunkSize - 1;
				int firstZ = 1;
		
				wegweiser = -1;
		
				for (int i = ChunkSize; i > 0; i--) {
						wegweiser ++;

						int x = firstX; 
						int z = firstZ;
			
						for (int j = i + 1; j > 2; j--) {
								//Debug.Log ("CurrentID: " + currentID);
								CreateCube (x, z, wegweiser, j, true);
								x--;
								z++;
						}
						wechselreihe = false;
						//firstX --;
						firstZ ++;
		
				}


		}

		void CreateCube (int x, int z, int i, int j, bool schluss)
		{
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
						//Debug.Log ("Fall vier");
						tempHeight = GenerateHeight (i, j, 4);
				}


				tempBlock = new Block (Instantiate (prefab, new Vector3 (x, tempHeight, z), new Quaternion (0, 0, 0, Quaternion.identity.w)), tempHeight, currentID);
				currentID++;
				World.Add (tempBlock);
		}

		float GenerateHeight (int i, int j, int opID)
		{
				//opID: 0: x=0 ; 1: z=0 ; 2: alle anderen; 3: der aller erste Block; 4: schluss!
				int rand = 0;
				float outHeight = 0f;
				Block a, b;
				switch (opID) {
				case 0:		//x = 0
						a = World [currentID - i - 1];
						if (rauf) {
								outHeight = a.height + 1f;
						} else {
								outHeight = a.height - 1f;
				
						}
			
						rand = Random.Range (1, 100);
						if (rand > Ebene) {
								outHeight = a.height;
				
						}

						break;

				case 1:		//z = 0
						a = World [currentID - i];
						if (rauf) {
								outHeight = a.height + 1f;
						} else {
								outHeight = a.height - 1f;
				
						}
			
						rand = Random.Range (1, 100);
						if (rand > Ebene) {
								outHeight = a.height;
				
						}
						break;

				case 2:
						a = World [currentID - i - 1];			//out of range am Anfang
						b = World [currentID - i];				//same here
						if (a.height != b.height) {
								if (Mathf.Abs (a.height - b.height) > 1) {
										outHeight = Mathf.Max (a.height, b.height) - 1f;
								} else {

										if (rauf) {
												outHeight = Mathf.Max (a.height, b.height);
										} else {
												outHeight = Mathf.Min (a.height, b.height);

										}



								}
						} else {

								if (rauf) {
										outHeight = a.height + 1f;
								} else {
										outHeight = a.height - 1f;
					
								}

								rand = Random.Range (1, 100);
								if (rand > Ebene) {
										outHeight = a.height;

								}
						}
						rand = Random.Range (0, 100);
						if (rand <= Wechsel)
								rauf = !rauf;
						break;
				case 3://der aller erste Block!

						outHeight = 0.0f;

						break;

				case 4://der schluss


			//Dieser scheiß stimmt nicht und ich weiß nicht genau wieso.
			/*
			 meine vermutung ist dass ich iwas mit der bestimmung von den nachbarn falsch mache. 
			 zudem ist mir aufgefallen dass das script sehr komsiche sachen anstellt, wenn ich die höhe des ERSTEN elements
			 der list benutze. 
			 ich werde hier den fehler in der listenordnung finden und fixen! */
			//Debug.Log ("Meine ID: " + currentID + " i: " + i + " a-ID: " + (currentID - ChunkSize + (i) * 2 - i));

						if (wechselreihe) {
								//a = World[currentID - i -2];			//out of range am Anfang


								a = World [currentID - ChunkSize + (i) * 2 - i];
								b = World [currentID - ChunkSize + (i) * 2 + 1 - i];
								//b = World[currentID - i -1];
						} else {
								a = World [currentID - ChunkSize + (i) * 2 - i];
								b = World [currentID - ChunkSize + (i) * 2 + 1 - i];
						}

						if (a.height != b.height) {
								if (Mathf.Abs (a.height - b.height) > 1) {
										outHeight = Mathf.Max (a.height, b.height) - 1f;
								} else {
					
										if (rauf) {
												outHeight = Mathf.Max (a.height, b.height);
										} else {
												outHeight = Mathf.Min (a.height, b.height);
						
										}
					
					
					
								}
						} else {
				
								if (rauf) {
										outHeight = a.height + 1f;
								} else {
										outHeight = a.height - 1f;
					
								}
				
								rand = Random.Range (1, 100);
								if (rand > Ebene) {
										outHeight = a.height;
					
								}
						}
						rand = Random.Range (0, 100);
						if (rand <= Wechsel)
								rauf = !rauf;
						break;
			
				}


				return outHeight;
		}

		void ListOutput ()
		{
				//Debug.Log (World.Count);
		}
}

public class Block
{
		/// <summary>
		/// The height.
		/// </summary>
		public float height;
		/// <summary>
		/// The object.
		/// </summary>
		public Object obj;
		/// <summary>
		/// The block I.
		/// </summary>
		public int BlockID;

		/// <summary>
		/// Initializes a new instance of the <see cref="Block"/> class.
		/// </summary>
		/// <param name="listObject">List object.</param>
		/// <param name="inputHeight">Input height.</param>
		/// <param name="currentID">Current I.</param>
		public Block (Object listObject, float inputHeight, int currentID)
		{
				obj = listObject;
				height = inputHeight;
				BlockID = currentID;
				obj.name = currentID.ToString ();

		}
}

public class Chunk{
	List<Block> blocks = new List<Block>();
	List<int> nord = new List<int> ();
	List<int> ost = new List<int> ();
	List<int> west = new List<int> ();
	List<int> sued = new List<int> ();

}
