using UnityEngine;
using System.Collections;

[System.Serializable]
public class WorldRenderer : MonoBehaviour {

	[Range(3, 8)]
	public int nthPower;

	[Range(1, 8)]
	public int featureSizePower;

	[Range(1, 20)]
	public float heightScale;

	[Range(1f, 10.0f)]
	public float heightStep;


	public GameObject waterSurface;
	public GameObject worldBlock;


	private WorldGenerator world;
	private GameObject[,] worldBlocks;
	private int size;


	// Use this for initialization
	void Start () {

		world = new WorldGenerator(nthPower, featureSizePower, heightScale, heightStep);

		size = world.getWorldSize();

		worldBlocks = new GameObject[size, size];



		firstDrawWorld();
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//drawWorld();

	}


	private void firstDrawWorld(){

		GameObject worldBlocksParent = new GameObject();
		worldBlocksParent.name = "World";
		worldBlocksParent.transform.parent = gameObject.transform;


		for(int x = 0; x < size; x++){
			for(int z = 0; z < size; z++){

				Vector3 pos = new Vector3(x, world.getElevation(x,z), z);
				GameObject blockInstance = Instantiate(worldBlock, pos, Quaternion.identity) as GameObject;
				blockInstance.transform.parent = worldBlocksParent.transform;
				blockInstance.transform.localScale = new Vector3(1.0f, world.getElevation(x,z)+10.0f, 1.0f);
				
				worldBlocks[x,z] = blockInstance;

			}
		}

		Vector3 waterPos = new Vector3((float)size/2, 0.0f, (float)size/2);

		GameObject waterPlane = Instantiate(waterSurface, waterPos, Quaternion.identity) as GameObject;

		waterPlane.transform.localScale = new Vector3((float)size/10, 1.0f, (float)size/10);


	}
}
