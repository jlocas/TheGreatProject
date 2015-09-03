using UnityEngine;
using System.Collections;

[System.Serializable]
public class HeightmapDrawer {

	private int size;
	public Color landColor1, landColor2, waterColor1, waterColor2;

	private Texture2D texture;
	private float[] pixels;

	//private Heightmap heightmap;
	public WorldData world;
	public WorldRenderer worldRenderer;

	// Use this for initialization
	public void Draw () {
		//heightmap = world.heightmaps[0].GetHeightmap();
		size = world.GetWorldSize();
		texture = new Texture2D(size,size, TextureFormat.RGB24, false);
		worldRenderer.SetTexture(texture);
		//GetComponent<Renderer>().material.mainTexture = texture;

		pixels = world.GetHeightmap();

		for(int x = 0; x < size; x++){
			for(int y = 0; y < size; y++){

				Color col = new Color();

				int i = (x & (size - 1)) + ((y & (size - 1)) * size);

				if(pixels[i] >= 0){
					col = Color.Lerp(landColor1, landColor2, pixels[i]);
				} else if(pixels[i] < 0){
					col = Color.Lerp(waterColor1, waterColor2, pixels[i] * -1f);
				} else {
					Debug.Log("Heightmap Drawer: Pixel error plz investigate.");
				}
				//Color col = Color.Lerp(landColor1, landColor2, pixels[x,y]);
				texture.SetPixel(x, y, col);
				//Debug.Log(x + ", " + y + ", " + pixels[x,y]);
			}
		}
		texture.Apply();
	
	}

	public Texture2D GetTexture(){
		return texture;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
