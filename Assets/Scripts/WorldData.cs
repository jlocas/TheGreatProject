using UnityEngine;
using System.Collections;


public class WorldData : MonoBehaviour  {

	private float[] heights;

	[Range(3, 10)]
	public int nthPower;
	private int worldSize;

	[Space(20)]
	public HeightmapManager heightmapManager;
	
	
	// Use this for initialization
	void Awake () {
		worldSize = (int)Mathf.Pow(2.0f, (float)nthPower);

		heightmapManager.Awake();
		heights = heightmapManager.GetHeightmap();
		gameObject.GetComponent<WorldRenderer>().Render();

	}
	
	public int GetWorldSize(){
		return worldSize;
	}

	public HeightmapManager GetHeightmapManager(){
		return heightmapManager;
	}

	public float[] GetHeightmap(){
		return heightmapManager.GetHeightmap();
	}

	public float getElevation(int x, int z){
		return heights[(x & (worldSize - 1)) + ((z & (worldSize - 1)) * worldSize)];
	}

	private void setElevation(int x, int z, float elev){
		heights[(x & (worldSize - 1)) + ((z & (worldSize - 1)) * worldSize)] = elev;
	}
}
