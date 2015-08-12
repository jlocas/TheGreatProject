using UnityEngine;
using System.Collections;

public class WorldGenerator {

	//Terrain shape variable
	private float[] elevationMap;
	private bool[] debugMap;

	private int worldSize;

	private float featureDistanceSquared;

	private float heightStep;

	public WorldGenerator(int nthPower, int featureSizePower, float scale, float hStep){
		heightStep = hStep;
		worldSize = (int)Mathf.Pow(2.0f, (float)nthPower);

		elevationMap = new float[worldSize*worldSize];
		debugMap = new bool[worldSize*worldSize];

		int featureSize = (int)Mathf.Pow(2.0f, (float)featureSizePower);

		featureDistanceSquared = Mathf.Pow((float)featureSize,2.0f) ;

		//initialisation de la map
		InitializePangea(featureSize, scale);



		//boucle diamond-square
		float sampleSize = (float)featureSize;

		while (sampleSize > 1)
		{
			DiamondSquare((int)sampleSize, scale);
			sampleSize /= 2f;
			scale /= 2.0f;
		}

		Quantizerooney();

	}


	private void Quantizerooney(){

		Debug.Log ("Inside the rooney");

		float heightStepInverse = 1 / heightStep;

		for(int x = 0; x < worldSize; x++){
			for(int z = 0; z < worldSize; z++){
				float heightTemp = getElevation(x, z);
				float newHeightTemp = Mathf.Round(heightTemp * heightStep) * heightStepInverse;
				setElevation(x, z, newHeightTemp);

				if(x==0 && z==0){
					Debug.Log ("getElevation: "+ heightTemp);
					Debug.Log ("newElevation: "+ newHeightTemp);
				}

			}
		}
	}

	public int getWorldSize(){
		return worldSize;
	}
	
	public float getElevation(int x, int z){
		return elevationMap[(x & (worldSize - 1)) + ((z & (worldSize - 1)) * worldSize)];
	}
	
	private void setElevation(int x, int z, float elev){

		/*if (debugMap[(x & (worldSize - 1)) + ((z & (worldSize - 1)) * worldSize)]){
			Debug.Log ("Elevation déja assignée a la coordonnée: x="+x+", z="+z);
		}
		else{
			elevationMap[(x & (worldSize - 1)) + ((z & (worldSize - 1)) * worldSize)] = elev;
			debugMap[(x & (worldSize - 1)) + ((z & (worldSize - 1)) * worldSize)] = true;
		}*/
		elevationMap[(x & (worldSize - 1)) + ((z & (worldSize - 1)) * worldSize)] = elev;
		
	}





	private float randomWeighted(int x, int z, int center){
		
		float distanceSquared = Mathf.Pow((float)(center-x),2.0f) + Mathf.Pow((float)(center-z),2.0f) ;
		
		float rando = 0;
		
		if (x==0 || z==0 || x==(worldSize-1) || z==(worldSize-1)){
			rando = Random.Range(-1.0f,-0.5f);
		}
		else if ( distanceSquared <= featureDistanceSquared ){
			rando = Random.Range(0.5f,1.0f);
		}
		else if ( distanceSquared <= (2*featureDistanceSquared) ){
			rando = Random.Range(0.0f,0.1f);
		}
		else{
			rando = Random.Range(-0.5f,0.5f);
		}

		return rando;
		
	}




	private void InitializePangea(int featureSize, float scale){

		int center = worldSize/2;
		float rando = 0;

		for( int x = 0; x < worldSize; x += featureSize){
			for (int z = 0; z < worldSize; z += featureSize)
			{
				rando = randomWeighted(x, z, center);
				setElevation(x, z, rando * scale);
			}
		}
	}









	private void DiamondSquare(int stepsize, float scale)
	{
		
		int halfstep = stepsize / 2;

		float rando = 0;
		
		for (int x = halfstep; x < worldSize + halfstep; x += stepsize)
		{
			for (int z = halfstep; z < worldSize + halfstep; z += stepsize)
			{
				rando = Random.Range(-0.5f,0.5f);
				sampleSquare(x, z, stepsize, rando * scale);
			}
		}
		
		for (int x = 0; x < worldSize; x += stepsize)
		{
			for (int z = 0; z < worldSize; z += stepsize)
			{
				rando = Random.Range(-0.5f,0.5f);
				sampleDiamond(x + halfstep, z, stepsize, rando * scale);

				rando = Random.Range(-0.5f,0.5f);
				sampleDiamond(x, z + halfstep, stepsize, rando * scale);
			}
		}
	}




	private void sampleSquare(int x, int y, int size, float value)
	{
		int hs = size / 2;

		// a     b 
		//
		//    x
		//
		// c     d
		
		float a = getElevation(x - hs, y - hs);
		float b = getElevation(x + hs, y - hs);
		float c = getElevation(x - hs, y + hs);
		float d = getElevation(x + hs, y + hs);

		float somme = ((a + b + c + d) / 4.0f) + value;

		setElevation(x, y, somme);

		/*if(x == 3 && y == 3){
			Debug.Log ("Square "+x+","+y);
			Debug.Log ("a = "+a);
			Debug.Log ("b = "+b);
			Debug.Log ("c = "+c);
			Debug.Log ("d = "+d);
			Debug.Log ("Somme / 4 = "+somme);
			float elev = getElevation(x, y);
			Debug.Log ("getElevation: "+elev);
		}*/

	}
	
	private void sampleDiamond(int x, int y, int size, float value)
	{
		int hs = size / 2;

		//Debug.Log ("Diamond "+x+","+y);
		//   c
		//
		//a  x  b
		//
		//   d
		
		float a = getElevation(x - hs, y);
		float b = getElevation(x + hs, y);
		float c = getElevation(x, y - hs);
		float d = getElevation(x, y + hs);

		setElevation(x, y, ((a + b + c + d) / 4.0f) + value);
	}
	
}
