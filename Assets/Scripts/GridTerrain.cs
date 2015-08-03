using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class GridTerrain : MonoBehaviour {
	public GameObject grassObject;
	public GameObject grassContainer;
	public GameObject terrainBlock;
	public GameObject waterBlock;
	public TileMap tileMap;

	[Space(20)]
	public int sizeX;
	public int sizeZ;

	[Space(20)]
	public Heightmap heightmap;

	[Space(20)]
	[Range(1, 10)]
	public int grassDensity;
	public GameObject combinedGrass;

	[Space(20)]
	public TerrainAssets assets;

	public GameObject[,] terrainBlocks;
	public GameObject[,] waterBlocks;

	public int grassTriangleCount;
	//max amount of grass in a single mesh
	public int grassMeshMax;

	GameObject waterBlocksParent;



	// Use this for initialization
	void Start () {

		waterBlocksParent = new GameObject();
		waterBlocksParent.name = "Water Blocks";
		waterBlocksParent.transform.parent = gameObject.transform;

		tileMap = new TileMap(sizeX, sizeZ, heightmap);


		terrainBlocks = new GameObject[sizeX,sizeZ];
		waterBlocks = new GameObject[sizeX,sizeZ];

		FirstDrawTerrain();
		FirstDrawWater();

	}


	// Update is called once per frame
	void Update () {


		tileMap.RefreshWater(Time.time, Time.deltaTime);
		RefreshWater();

	}






	void FirstDrawTerrain(){

		GameObject terrainBlocksParent = new GameObject();
		terrainBlocksParent.name = "Terrain Blocks";
		terrainBlocksParent.transform.parent = gameObject.transform;

		for(int x = 0; x < sizeX; x++){
			for(int z = 0; z < sizeZ; z++){
					
				Vector3 pos = new Vector3(x, tileMap.tiles[x,z].elevation, z);
				GameObject blockInstance = Instantiate(terrainBlock, pos, Quaternion.identity) as GameObject;
				blockInstance.transform.parent = terrainBlocksParent.transform;

				terrainBlocks[x,z] = blockInstance;

				switch(tileMap.tiles[x,z].surface)
				{
				case SURFACE.TALLGRASS:
					blockInstance.GetComponentInChildren<MeshRenderer>().material = assets.materials.grass;
					break;

				case SURFACE.GRASS:
					blockInstance.GetComponentInChildren<MeshRenderer>().material = assets.materials.grass;
					combinedGrass = Instantiate<GameObject>(grassContainer);

					Vector3 blockPos = blockInstance.transform.position;
					for(int i = 0; i < grassDensity; i++){
						//position for the new grass object. x and z give a random pos within the top face of the cube, y is elevated to the top of the cube and given a random deviation
						Vector3 grassPos = new Vector3(blockPos.x + Random.value - 0.5f, blockPos.y + 0.15f, blockPos.z + Random.value - 0.5f);
						GameObject newGrass = Instantiate(grassObject, grassPos, Quaternion.identity) as GameObject;
						newGrass.transform.Rotate(new Vector3(0, Random.value*360f, 0));
						newGrass.transform.parent = combinedGrass.transform;
						combinedGrass.transform.parent = blockInstance.transform;
					}
					break;

				case SURFACE.DIRT:
					blockInstance.GetComponentInChildren<MeshRenderer>().material = assets.materials.dirt;
					break;

				default:
					blockInstance.GetComponentInChildren<MeshRenderer>().material = assets.materials.def;
					break;
				}
			}
		}
	}



	void FirstDrawWater(){

		for(int x = 0; x < sizeX; x++){
			for(int z = 0; z < sizeZ; z++){

				if(tileMap.tiles[x,z].hasWater){

					Vector3 pos = new Vector3(x, tileMap.tiles[x,z].getWaterSurface(), z);
					GameObject blockInstance = Instantiate(waterBlock, pos, Quaternion.identity) as GameObject;
					blockInstance.transform.parent = waterBlocksParent.transform;

					waterBlocks[x,z] = blockInstance;

					blockInstance.transform.localScale = new Vector3(1, 0, 1);
					blockInstance.GetComponentInChildren<MeshRenderer>().material = assets.materials.water;
				}
			}
		}
	}

	void RefreshWater(){

		for(int x = 0; x < sizeX; x++){
			for(int z = 0; z < sizeZ; z++){

				if( tileMap.tiles[x,z].hasWater && waterBlocks[x,z] == null ){
					if (tileMap.tiles[x,z].waterDepth > 0.1){
						Vector3 pos = new Vector3(x, tileMap.tiles[x,z].getWaterSurface(), z);
						GameObject blockInstance = Instantiate(waterBlock, pos, Quaternion.identity) as GameObject;
						blockInstance.transform.parent = waterBlocksParent.transform;
						waterBlocks[x,z] = blockInstance;
						blockInstance.transform.localScale = new Vector3(1, 0, 1);
						blockInstance.GetComponentInChildren<MeshRenderer>().material = assets.materials.water;
					}
				}
				else if( !tileMap.tiles[x,z].hasWater && waterBlocks[x,z] != null ){
					Destroy(waterBlocks[x,z]);
				}
				else if( tileMap.tiles[x,z].hasWater && waterBlocks[x,z] != null ){
					Vector3 pos = new Vector3(x, tileMap.tiles[x,z].getWaterSurface(), z);
					waterBlocks[x,z].transform.position = pos;
				}
			}
		}

	}

	int CountTriangles(Mesh mesh){
		return mesh.triangles.Length;
	}


}
