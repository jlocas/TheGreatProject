using UnityEngine;
using System.Collections;

[System.Serializable]
public class HeightmapDrawer {

	private int size;
	public Color landColor1, landColor2, waterColor1, waterColor2;

	private Texture2D texture;
	private float[] pixels;

	//private Heightmap heightmap;
	public WorldData worldData;
	public WorldRenderer worldRenderer;

	// Use this for initialization
	public void Draw () {

		size = worldData.GetWorldSize();
		texture = new Texture2D(size,size, TextureFormat.RGB24, false);
		worldRenderer.SetTexture(texture);

		pixels = worldData.GetHeightmap();

		float scales = worldData.GetHeightmapManager().GetScalesSum();

		for(int x = 0; x < size; x++){
			for(int y = 0; y < size; y++){

				Color col = new Color();

				int i = (x & (size - 1)) + ((y & (size - 1)) * size);
				float height = pixels[i] / scales;

				if(height >= 0){
					col = Color.Lerp(landColor1, landColor2, height);
				} else if(height < 0){
					col = Color.Lerp(waterColor1, waterColor2, height * -1f);
				} else {
					Debug.Log("Heightmap Drawer: Pixel error plz investigate.");
				}
				texture.SetPixel(x, y, col);
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
