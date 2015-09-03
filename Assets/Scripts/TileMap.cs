using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMap {

	public Tile[,] tiles;
	public float seed;

	private int sizeX, sizeZ;
	private HM_Perlin heightmap;

	public TileMap (int w, int h, HM_Perlin hmap){
		seed = Random.value * 1000;
		sizeX = w;
		sizeZ = h;

		heightmap = hmap;
		tiles = new Tile[sizeX, sizeZ];

		for(int z=0; z < sizeZ; z++){
			for (int x=0; x < sizeX; x++){
				//initialisation par défaut: grass
				tiles[x,z] = new Tile(SURFACE.GRASS, x, z);
			}
		}

		GenerateHeightMap();
		GenerateRiver();
	}

	public void GenerateMap(string biome){

		switch(biome)
		{
		case "Forest": case "forest":
			Debug.Log ("Creating forest");
			break;
		default:
			Debug.Log ("Unable to generate map.");
			break;

		}
	}

	void GenerateHeightMap(){
		for(int x = 0; x < sizeX; x++){
			for(int z = 0; z < sizeZ; z++){
				tiles[x,z].elevation = heightmap.Generate(x,z);
			}
		}
	}



	void GenerateRiver(){
		float a = Random.value - 0.5f;
		float b = Random.Range(3, 7) * 0.1f * sizeZ;
		float[] periods = new float[2]{Random.Range(0.1f, 0.2f), Random.Range(0.2f, 0.4f)};

		// Maximum riverWidth = 5;
		int riverWidth = 5;
		float riverDepth = 2;

		for(int x = 0; x < sizeX; x++){
			int z = (int)(Mathf.Sin(x * periods[0]) * 5f + Mathf.Sin(x * periods[1]) * 2.5f + a * x + b);
			if(z >= 0 && z < sizeZ){

				// Diggin'
				tiles[x,z].surface = SURFACE.DIRT;
				DigRiver(tiles[x,z], riverWidth, riverDepth);

				// Fillin'
				if(x == 0 || x == sizeX-1){
					FillRiverSource(tiles[x,z], riverWidth);
				}
				else{
					FillRiver(tiles[x,z], riverWidth);
				}
			}
		}
	}

	void DigRiver(Tile centerTile, int width, float depth){

		for (int i=-width; i<=width; i++){

			if(centerTile.posZ+i >=0 && centerTile.posZ+i < sizeZ){
				float digDepth = depth * Mathf.Exp(-Mathf.Pow (i,2)/width);
				tiles[centerTile.posX, centerTile.posZ + i].elevation = tiles[centerTile.posX, centerTile.posZ + i].elevation - digDepth;
				tiles[centerTile.posX, centerTile.posZ + i].surface = SURFACE.DIRT;
			}
		}
	}

	void FillRiverSource(Tile centerTile, int width){

		//initialisation for the center tile
		if(centerTile.posZ >=0 && centerTile.posZ < sizeZ){

			tiles[centerTile.posX, centerTile.posZ].hasWater = true;
			tiles[centerTile.posX, centerTile.posZ].isSource = true;
			tiles[centerTile.posX, centerTile.posZ].waterDepth 
				= tiles[centerTile.posX, centerTile.posZ + width-2].elevation - tiles[centerTile.posX, centerTile.posZ].elevation;
			tiles[centerTile.posX, centerTile.posZ].sourceDepth = tiles[centerTile.posX, centerTile.posZ].waterDepth;
		}

		//initiation for adjacent tiles
		for (int i=-width; i<=width; i++){
			if(i !=0 && centerTile.posZ+i >=0 && centerTile.posZ+i < sizeZ){

				float depth = tiles[centerTile.posX, centerTile.posZ].getWaterSurface() - tiles[centerTile.posX, centerTile.posZ + i].elevation;

				if (depth > 0){
					tiles[centerTile.posX, centerTile.posZ + i].hasWater = true;
					tiles[centerTile.posX, centerTile.posZ + i].isSource = true;
					tiles[centerTile.posX, centerTile.posZ + i].waterDepth = depth;
					tiles[centerTile.posX, centerTile.posZ + i].sourceDepth = depth;
					tiles[centerTile.posX, centerTile.posZ + i].waterVelocity = 0;
				}

			}
		}
	}

	void FillRiver(Tile centerTile, int width){

		if(centerTile.posZ >=0 && centerTile.posZ < sizeZ){

			tiles[centerTile.posX, centerTile.posZ].hasWater = true;
			tiles[centerTile.posX, centerTile.posZ].waterDepth 
				= tiles[centerTile.posX, centerTile.posZ + width-2].elevation - tiles[centerTile.posX, centerTile.posZ].elevation;
		}
		
		for (int i=-width; i<=width; i++){
			if(i !=0 && centerTile.posZ+i >=0 && centerTile.posZ+i < sizeZ){
				
				float depth = tiles[centerTile.posX, centerTile.posZ].getWaterSurface() - tiles[centerTile.posX, centerTile.posZ + i].elevation;
				
				if (depth > 0){
					tiles[centerTile.posX, centerTile.posZ + i].hasWater = true;
					tiles[centerTile.posX, centerTile.posZ + i].waterDepth = depth;
				}
			}
		}
	}



	public void RefreshWater(float time, float deltaTime){

		//Debug.Log ("inside Refresh Water");

		float[,] newWaterSurface = new float[sizeX, sizeZ];
		float dt = deltaTime;
		
		for (int x = 0; x < sizeX; x++){
			for (int z = 0; z < sizeZ; z++){
				float force = getWaterForce(x,z);
				tiles[x,z].waterVelocity += (force * dt);
				newWaterSurface[x,z] = tiles[x,z].getWaterSurface() + (force * dt);
			}
		}

		for (int x = 0; x < sizeX; x++){
			for (int z = 0; z < sizeZ; z++){

				//on essaie de faire des vagues dans la source
				if(tiles[x,z].isSource){
					//float surf = 0.25f*Mathf.Cos(time)+tiles[x,z].sourceDepth+tiles[x,z].elevation;
					//tiles[x,z].setWaterSurface(surf);
					tiles[x,z].setWaterSurface(newWaterSurface[x,z]);
				}
				//si ce n'est pas une source, on assigne sa nouvelle hauteur calculée
				else{
					if((newWaterSurface[x,z]-tiles[x,z].elevation) >= 0 ){
						tiles[x,z].hasWater = true;
						tiles[x,z].setWaterSurface(newWaterSurface[x,z]);
					}
					else{
						tiles[x,z].hasWater = false;
					}

				}
			}
		}

	}

	private float getWaterForce(int x, int z){

		int multfactor = 0;
		float force = 0;

		if (x+1 >= 0 && x+1 < sizeX && tiles[x+1,z].hasWater){
			multfactor++;
			force += tiles[x+1,z].getWaterSurface();
		}
		if (x-1 >= 0 && x-1 < sizeX && tiles[x-1,z].hasWater){
			multfactor++;
			force += tiles[x-1,z].getWaterSurface();
		}
		if (z+1 >= 0 && z+1 < sizeZ && tiles[x,z+1].hasWater){
			multfactor++;
			force += tiles[x,z+1].getWaterSurface();
		}
		if (z-1 >= 0 && z-1 < sizeZ && tiles[x,z-1].hasWater){
			multfactor++;
			force += tiles[x,z-1].getWaterSurface();
		}

		force -= (multfactor * tiles[x,z].getWaterSurface());

		//Debug.Log(force);

		force /= multfactor;

		force *= 10;

		return force;
	}


	float Gauss(float x, float mu, float sigma){

		float a = 1 / (sigma * 2 * Mathf.PI);
		float b = mu;
		float c = sigma;

		float fx = a * Mathf.Exp ( -(Mathf.Pow(x-b,2)/(2*Mathf.Pow (c,2))));

		return fx;
	}

	List<Tile> GetNeighbors(Tile tile){
		List<Tile> neighbors = new List<Tile>();

		for(int x = -1; x <= 1; x++){
			for(int z = -1; z <= 1; z++){

				if( !(x == 0 && z == 0) ){
					if(tile.posX + x < sizeX && tile.posX + x >= 0
					   && tile.posZ + z < sizeZ && tile.posZ + z >= 0){
						
						neighbors.Add(tiles[tile.posX + x, tile.posZ + z]);
					}
				}
			}
		}
		return neighbors;
	}

	Tile GetLowestTile(){
		Tile lowestTile = new Tile();
		lowestTile.elevation = 99999999999999999;

		for(int x=0; x < sizeX; x++){
			for (int z=0; z < sizeZ; z++){
				if (tiles[x,z].elevation < lowestTile.elevation){
					lowestTile = tiles[x,z];
				}
			}
		}

		return lowestTile;
	}


}
