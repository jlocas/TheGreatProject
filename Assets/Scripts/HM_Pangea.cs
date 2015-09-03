using UnityEngine;
using System.Collections;

public class HM_Pangea : Heightmap {
	
	protected float featureDistanceSquared;

	public HM_Pangea(HeightmapParams vars) : base(vars)
	{
		featureDistanceSquared = Mathf.Pow((float)featureSize, 2.0f) ;
		InitializePangea();
		ApplyDiamondSquare(); //absolutely need diamond square for this to work!
		ApplyForm();
		ApplyScale();
		ApplyHeightStep();



		
	}

	private void InitializePangea(){
		
		int center = mapSize/2;
		float rando = 0;

		for( int x = 0; x < mapSize; x += featureSize){
			for (int y = 0; y < mapSize; y += featureSize)
			{
				rando = randomWeighted(x, y, center);
				SetHeight(x, y, rando);
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
}
