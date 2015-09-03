using UnityEngine;
using System.Collections;

[System.Serializable]
public class WorldGenerator : MonoBehaviour {

	//Terrain shape variable
	private float[] elevationMap;
	
	//private bool[] debugMap;
	private int worldSize;
	private float featureDistanceSquared;

	public WorldData data;



	public WorldGenerator(){
		
	}


	/*public void Generate(){

		worldSize = data.getWorldSize();
				
		if(data.landmass == HeightmapType.PANGEA){
			int featureSize = 5;
			HM_Pangea pangea = new HM_Pangea(worldSize, featureSize, data.heightScale);
			elevationMap = pangea.getElevations();
		}


	}
	
	
	public float getElevation(int x, int z){
		return elevationMap[(x & (worldSize - 1)) + ((z & (worldSize - 1)) * worldSize)];
	}
	
	private void setElevation(int x, int z, float elev){
		elevationMap[(x & (worldSize - 1)) + ((z & (worldSize - 1)) * worldSize)] = elev;
	}*/
	
}
