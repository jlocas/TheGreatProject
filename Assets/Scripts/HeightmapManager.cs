using UnityEngine;
using System.Collections;



[System.Serializable]
public class HeightmapManager {

	public WorldData world;
	private float[] heightmap;

	[Space(20)]
	public Heightmap[] heightmapsParameters;
	private Heightmap[] heightmaps;

	private int size;

	// Use this for initialization
	public void Awake () {
		if(world == null){
			Debug.Log("Heightmap has no WorldData component, assigning default world data");
			world = GameObject.Find("World").GetComponent<WorldData>();
		}
		size = world.GetWorldSize();
		Generate();
	
	}

	private void Generate(){

		heightmaps = new Heightmap[heightmapsParameters.Length];

		for(int i = 0 ; i < heightmapsParameters.Length ; i++){

			switch(heightmapsParameters[i].GetType()){
			case HeightmapType.NONE:
				heightmaps[i] = new Heightmap(size, heightmapsParameters[i].GetFeatureSizePower(), heightmapsParameters[i].GetHeightScale(), heightmapsParameters[i].GetHeightStep());
				break;
			case HeightmapType.PANGEA:
				heightmaps[i] = new HM_Pangea(size, heightmapsParameters[i].GetFeatureSizePower(), heightmapsParameters[i].GetHeightScale(), heightmapsParameters[i].GetHeightStep());
				break;
			}
		}
		heightmap = heightmaps[0].GetHeightmap();
		/*if(heightmapType == HeightmapType.PANGEA){
			//featureSize = (int)Mathf.Pow(2.0f, (float)featureSizePower);
			heightmap[0] = new HM_Pangea(world);
			//heightmap = pangea.getElevationMap();
			//heightmap = pangea.GetHeightmap();
		}*/
	}

	private void CalculateHeightmap(){
		float[] map = new float[size];
	}

	public float[] GetHeightmap(){
		return heightmap;
	}
}
