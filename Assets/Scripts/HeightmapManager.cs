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

			switch(heightmapsParameters[i].GetType())
			{
			case HeightmapType.NONE:
				heightmapsParameters[i].SetHeightParams();
				heightmaps[i] = new Heightmap(heightmapsParameters[i].GetHeightParams());
				break;
			case HeightmapType.PANGEA:
				heightmapsParameters[i].SetHeightParams();
				heightmaps[i] = new HM_Pangea(heightmapsParameters[i].GetHeightParams());
				break;
			}
		}
		heightmap = heightmaps[0].GetFloatMap();
	}

	private void CalculateHeightmap(){
		float[] map = new float[size];
	}


	public float GetScalesSum(){

		float scaleSum = 0f;

		for(int i = 0; i < heightmaps.Length; i++){
			scaleSum += heightmaps[i].GetHeightScale();
		}

		return scaleSum;
	}

	public float[] GetHeightmap(){
		return heightmap;
	}
}
