using UnityEngine;
using System.Collections;

public class Tile {

	// Tile type info
	public int posX;
	public int posZ;
	public float elevation;
	public SURFACE surface;

	// Water info
	public bool hasWater;
	public bool isSource;
	public float sourceDepth;
	public float waterDepth;
	public float waterVelocity;



	public Tile(SURFACE s, int x, int z){
		surface = s;
		posX = x;
		posZ = z;
		elevation = 0f;
		hasWater = false;
		isSource = false;
		waterDepth = 0;
		waterVelocity = 0;
	}

	public Tile(){
		surface = SURFACE.GRASS;
		posX = 0;
		posZ = 0;
		elevation = 0f;
		hasWater = false;
		isSource = false;
		waterDepth = 0;
		waterVelocity = 0;
	}


	public float getWaterSurface(){
		return (elevation + waterDepth);
	}
	public void setWaterSurface(float s){
		waterDepth = s - elevation;
	}


}

public enum SURFACE{
	DIRT,
	GRASS,
	TALLGRASS,
	STONE
}


