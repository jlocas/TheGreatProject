using UnityEngine;
using System.Collections;

public class HM_Pangea : Heightmap {
	
	protected float featureDistanceSquared;


	public HM_Pangea(int size, int fspow, float scale, float step) : base(size, fspow, scale, step)
	{
		featureDistanceSquared = Mathf.Pow((float)featureSize, 2.0f) ;
		
		//initialisation de la map
		InitializePangea(heightScale);

		//boucle diamond-square
		float sampleSize = (float)featureSize;
		
		while (sampleSize > 1)
		{
			DiamondSquare((int)sampleSize, scale);
			sampleSize /= 2f;
			scale /= 2f;
		}

		
	}

	private void InitializePangea(float scale){
		
		int center = mapSize/2;
		//Debug.Log("center = " + center);
		float rando = 0;

		for( int x = 0; x < mapSize; x += featureSize){
			for (int y = 0; y < mapSize; y += featureSize)
			{
				rando = randomWeighted(x, y, center);
				SetHeight(x, y, rando * scale);
			}
		}
	}
	
	private float randomWeighted(int x, int z, int center){
		
		float distanceSquared = Mathf.Pow((float)(center-x),2.0f) + Mathf.Pow((float)(center-z),2.0f) ;
		
		float rando = 0;
		
		if (x==0 || z==0 || x==(mapSize-1) || z==(mapSize-1)){
			rando = Random.Range(-1.0f,-0.5f);
		}
		else if ( distanceSquared <= featureDistanceSquared ){
			rando = Random.Range(0.5f,1.0f);
		}
		else if ( distanceSquared <= (2*featureDistanceSquared) ){
			rando = Random.Range(0.0f,1.0f);
		}
		else{
			rando = Random.Range(-0.5f,0.5f);
		}
		
		return rando;
		
	}

	private void DiamondSquare(int stepsize, float scale)
	{
		
		int halfstep = stepsize / 2;
		
		float rando = 0;
		
		for (int x = halfstep; x < mapSize + halfstep; x += stepsize)
		{
			for (int z = halfstep; z < mapSize + halfstep; z += stepsize)
			{
				rando = Random.Range(-0.5f,0.5f);
				SampleSquare(x, z, stepsize, rando * scale);
			}
		}
		
		for (int x = 0; x < mapSize; x += stepsize)
		{
			for (int z = 0; z < mapSize; z += stepsize)
			{
				rando = Random.Range(-0.5f,0.5f);
				SampleDiamond(x + halfstep, z, stepsize, rando * scale);
				
				rando = Random.Range(-0.5f,0.5f);
				SampleDiamond(x, z + halfstep, stepsize, rando * scale);
			}
		}
	}

	private void SampleSquare(int x, int y, int size, float value)
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
		
		float somme = ((a + b + c + d) / 4.0f) + value;
		
		SetHeight(x, y, somme);		
	}
	
	private void SampleDiamond(int x, int y, int size, float value)
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
		
		SetHeight(x, y, ((a + b + c + d) / 4.0f) + value);
	}

}
