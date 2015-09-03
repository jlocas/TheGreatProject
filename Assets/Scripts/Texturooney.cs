using UnityEngine;
using System.Collections;

public enum NoiseType{
	Noise,
	SmoothNoise,
	Turbulence,
	Marble,
	Perlin
}


public class Texturooney : MonoBehaviour {

	public NoiseType type;

	public int sizeX, sizeY;
	public Color color1, color2;
	public float zoom;

	public float turbSize;
	public float turbPower;
	
	private Texture2D texture;
	private float[,] pixels;

	// Use this for initialization
	void Start () {
		texture = new Texture2D(sizeX, sizeY, TextureFormat.RGB24, false);
		GetComponent<Renderer>().material.mainTexture = texture;

		GenerateTexture();
	
	
	}
	
	// Update is called once per frame
	void Update () {
		GenerateTexture();
	
	}

	float[,] Noise(){
		float[,] noise = new float[sizeX, sizeY];

		for(int x = 0; x < sizeX; x++){
			for(int y = 0; y < sizeY; y++){
				float value = Random.value;
				noise[x, y] = value;
			}
		}
		return noise;
	}

	float SmoothNoise(float x, float y){

		//get fractional part of x and y
		float fractX = x - (int)x;
		float fractY = y - (int)y;

		//wrap around
		int x1 = ((int)x + sizeX) % sizeX;
		int y1 = ((int)y + sizeY) % sizeY;

		//neighbor values
		int x2 = (x1 + sizeX - 1) % sizeX;
		int y2 = (y1 + sizeY - 1) % sizeY;

		//smooth the noise with bilinear interpolation
		float value = 0f;
		value += fractX * fractY * pixels[x1, y1];
		value += fractX * (1 - fractY) * pixels[x1, y2];
		value += (1 - fractX) * fractY * pixels[x2, y1];
		value += (1 - fractX) * (1 - fractY) * pixels[x2, y2];

		return value;
	}

	float Turbulence(float x, float y, float size){
		float value = 0.0f, initialSize = size;
		
		while(size >= 1){
			value += SmoothNoise(x / size, y/size) * size;
			size *= 0.5f;
		}
		
		return 128f * value / initialSize;
	}

	void GenerateTexture(){
		switch(type)
		{
		case NoiseType.Noise:
			pixels = Noise();
			break;

		case NoiseType.SmoothNoise:
			pixels = Noise ();
			float[,] tempPix = pixels;

			for(int x = 0; x < sizeX; x++){
				for(int y = 0; y < sizeY; y++){
					//to avoid blurring over blur
					tempPix[x,y] = SmoothNoise(x/zoom, y/zoom);
				}
			}
			pixels = tempPix;
			break;
		case NoiseType.Turbulence:

			break;
		case NoiseType.Marble:
			break;
		}

		for(int x = 0; x < sizeX; x++){
			for(int y = 0; y < sizeY; y++){
				Color col = Color.Lerp(color1, color2, pixels[x,y]);
				texture.SetPixel(x, y, col);
			}
		}
		texture.Apply();
	}
}
