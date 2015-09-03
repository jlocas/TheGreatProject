using UnityEngine;
using System.Collections;

public enum HeightmapType{
	NONE,
	PANGEA
}



[System.Serializable]
public class Heightmap {
	
	protected float[] heightmap;
	protected int mapSize;
	
	public HeightmapType heightmapType;
	protected int featureSize;
	[Range(1, 10)]
	public int featureSizePower;
	public float heightScale;
	public float heightStep;


	public Heightmap(int size, int fspow, float scale, float step){
		mapSize = size;
		featureSizePower = fspow;
		heightScale = scale;
		heightStep = step;

		heightmap = new float[mapSize*mapSize];	
		featureSize = (int)Mathf.Pow(2.0f, (float)featureSizePower);

	}

	public float GetHeight(int x, int y){
		return heightmap[(x & (mapSize - 1)) + ((y & (mapSize - 1)) * mapSize)];
	}
	
	protected void SetHeight(int x, int y, float height){
		heightmap[(x & (mapSize - 1)) + ((y & (mapSize - 1)) * mapSize)] = height;
	}

	public float[] GetHeightmap(){
		return heightmap;
	}
	
	public HeightmapType GetType(){
		return heightmapType;
	}


	public int GetMapSize(){
		return mapSize;
	}
	
	public int GetFeatureSizePower(){
		return featureSizePower;
	}
	
	public float GetHeightScale(){
		return heightScale;
	}

	public float GetHeightStep(){
		return heightStep;
	}

	/*public void ApplyHeightStep(){
		float heightStepInverse = 1/heightStep;
		for(int x = 0; x < mapSize; x++){
			for(int y = 0; y < mapSize; y++){
				float newHeight = Mathf.Round( GetHeight(x, y))
				SetHeight(x, y, )
			}
		}
	}*/



}
