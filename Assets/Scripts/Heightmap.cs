using UnityEngine;
using System.Collections;

public enum HeightmapType{
	NONE,
	PANGEA
}

public enum Form
{
	Lin,
	Exp,
	Pow

}

public struct HeightmapParams{
	public Form form;
	public float formArg;
	public int mapSize;
	public int featureSizePower;
	public float heightScale;
	public bool heightStepped;
	public float heightStep;
	public MinMaxRange diamondRange;
	public MinMaxRange squareRange;
	public bool diamondSquared;
}

[System.Serializable]
public class Heightmap {

	protected WorldData worldData;
	protected int mapSize;
	protected float dsScale;

	
	public HeightmapType heightmapType;
	public Form form;
	public float formArg;
	protected int featureSize;
	[Range(1, 10)]
	public int featureSizePower;
	public float heightScale;
	[Space(20)]
	public bool heightStepped;
	[Range(0.1f, 10f)]
	public float heightStep;
	[Space(20)]
	public bool diamondSquared;

	[MinMaxRange(-2f, 2f)]
	public MinMaxRange diamondRange;
	[MinMaxRange(-2f, 2f)]
	public MinMaxRange squareRange;

	protected float[] floatMap;
	protected HeightmapParams heightParams;
	
	public Heightmap(HeightmapParams vars){
		worldData = GameObject.Find("World").GetComponent<WorldData>();

		form = vars.form;
		formArg = vars.formArg;
		mapSize = worldData.GetWorldSize();
		featureSizePower = vars.featureSizePower;
		heightScale = vars.heightScale;
		heightStepped = vars.heightStepped;
		heightStep = Mathf.Max(vars.heightStep, 0.00001f);

		floatMap = new float[mapSize*mapSize];	
		featureSize = (int)Mathf.Pow(2.0f, (float)featureSizePower);

		diamondSquared = vars.diamondSquared;
		diamondRange = vars.diamondRange;
		squareRange = vars.squareRange;
	}

	public void SetHeightParams(){
		heightParams = new HeightmapParams();

		heightParams.form = form;
		heightParams.formArg = formArg;
		heightParams.mapSize = mapSize;
		heightParams.featureSizePower = featureSizePower;
		heightParams.heightScale = heightScale;
		heightParams.heightStepped = heightStepped;
		heightParams.heightStep = heightStep;
		heightParams.diamondSquared = diamondSquared;
		heightParams.diamondRange = diamondRange;
		heightParams.squareRange = squareRange;
	}

	public HeightmapParams GetHeightParams(){
		return heightParams;
	}

	public float GetHeight(int x, int y){
		return floatMap[(x & (mapSize - 1)) + ((y & (mapSize - 1)) * mapSize)];
	}
	
	public void SetHeight(int x, int y, float height){

		int i = (x & (mapSize - 1)) + ((y & (mapSize - 1)) * mapSize);
		floatMap[i] = height;


	}

	public float[] GetFloatMap(){
		return floatMap;
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

	public int GetFeatureSize(){
		return featureSize;
	}
	
	public float GetHeightScale(){
		return heightScale;
	}

	public float GetHeightStep(){
		return heightStep;
	}

	protected void ApplyForm(){

		for(int x = 0; x < mapSize; x++){
			for(int y = 0; y < mapSize; y++){
				int i = (x & (mapSize - 1)) + ((y & (mapSize - 1)) * mapSize);
				switch(form)
				{
				case Form.Lin:
					break;
				case Form.Exp:
					floatMap[i] = Mathf.Pow(formArg, floatMap[i]);
					break;
				case Form.Pow:
					floatMap[i] = Mathf.Pow(floatMap[i], formArg);
					break;
				}

			}
		}
	}

	protected void ApplyScale(){
		for(int x = 0; x < mapSize; x++){
			for(int y = 0; y < mapSize; y++){
				float newHeight = GetHeight(x, y) * heightScale;
				SetHeight(x, y, newHeight);
			}
		}
	}

	protected void ApplyHeightStep(){
		if(heightStepped){
			float heightStepInverse = 1/heightStep;
			
			for(int x = 0; x < mapSize; x++){
				for(int y = 0; y < mapSize; y++){
					float newHeight = Mathf.Round( GetHeight(x, y) * heightStep) * heightStepInverse;
					SetHeight(x, y, newHeight);
				}
			}
		}
	}

	protected void ApplyDiamondSquare(){

		dsScale = 1f;

		if(diamondSquared)
		{

			//boucle diamond-square
			float sampleSize = (float)featureSize;
			
			while (sampleSize > 1)
			{
				DiamondSquare((int)sampleSize);
				sampleSize /= 2f;
				dsScale /= 2f;
			}
		}
	}



	protected void DiamondSquare(int stepsize)
	{
		
		int halfstep = stepsize / 2;
		
		float rando = 0;
		
		for (int x = halfstep; x < mapSize + halfstep; x += stepsize)
		{
			for (int z = halfstep; z < mapSize + halfstep; z += stepsize)
			{
				rando = Random.Range(squareRange.min, squareRange.max);
				SampleSquare(x, z, stepsize, rando);
			}
		}
		
		for (int x = 0; x < mapSize; x += stepsize)
		{
			for (int z = 0; z < mapSize; z += stepsize)
			{
				rando = Random.Range(diamondRange.min, diamondRange.max);
				SampleDiamond(x + halfstep, z, stepsize, rando);
				
				rando = Random.Range(diamondRange.min, diamondRange.max);
				SampleDiamond(x, z + halfstep, stepsize, rando);
			}
		}
	}
	
	protected void SampleSquare(int x, int y, int size, float value)
	{
		int hs = size / 2;
		
		// a     b 
		//
		//    x
		//
		// c     d
		
		float a = GetHeight(x - hs, y - hs);
		float b = GetHeight(x + hs, y - hs);
		float c = GetHeight(x - hs, y + hs);
		float d = GetHeight(x + hs, y + hs);
		
		float sum = ((a + b + c + d) / 4.0f) + value * dsScale;
		
		SetHeight(x, y, sum);		
	}
	
	protected void SampleDiamond(int x, int y, int size, float value)
	{
		int hs = size / 2;
		
		//Debug.Log ("Diamond "+x+","+y);
		//   c
		//
		//a  x  b
		//
		//   d
		
		float a = GetHeight(x - hs, y);
		float b = GetHeight(x + hs, y);
		float c = GetHeight(x, y - hs);
		float d = GetHeight(x, y + hs);
		
		float sum = ((a + b + c + d) / 4.0f) + value * dsScale;
		
		SetHeight(x, y, sum);
	}
}
