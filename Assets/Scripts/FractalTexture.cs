using UnityEngine;
using System.Collections;

public enum NoiseTyp{
	Noise,
	Fractal,
	Marble
}

public class FractalTexture : MonoBehaviour {

	public NoiseTyp type;

	public int sizeX, sizeZ;

	private Texture2D texture;

	private float perlin;

	public Color color1, color2;

	public float xPeriod;
	public float zPeriod;

	public float turbPower;
	public float turbSize;




	// Use this for initialization
	void Start () {
		texture = new Texture2D(sizeX, sizeZ, TextureFormat.RGB24, false);
		GetComponent<Renderer>().material.mainTexture = texture;

		GenerateTexture ();
	
	}
	
	// Update is called once per frame
	void Update () {
		GenerateTexture ();
	
	}

	void GenerateTexture(){
		for(int x = 0; x < sizeX; x++){
			for(int z = 0; z < sizeZ; z++){

				float value = x * xPeriod / (float)sizeX + z * zPeriod / (float)sizeZ + turbPower * Turbulence(x, z, turbSize) / 256f;
				float sineValue = 256f * Mathf.Abs(Mathf.Sin(value * Mathf.PI));

				Color col = new Color(0,sineValue,0);
			

				texture.SetPixel(x, z, col);
			}
		}

		texture.Apply();
	}

	float Turbulence(float x, float z, float size){
		float value = 0.0f, initialSize = size;

		while(size >= 1){
			value += Mathf.PerlinNoise(x/size, z/size) * size;
			size *= 0.5f;
		}

		return 128f * value / initialSize;
	}
}
