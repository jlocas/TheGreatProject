using UnityEngine;
using System.Collections;

[System.Serializable]
public class Heightmap {

	public float heightScaleX = 10.0f, heightScaleZ = 10.0f, heightMul = 50.0f, heightStep = 5f;
	public float seed = 4f;

	public float Generate(int x, int z){
		float heightStepInverse = 1/heightStep;
		float y = Mathf.Round(Mathf.PerlinNoise(seed + x/heightScaleX, seed + z/heightScaleZ) * heightStep) * heightStepInverse * heightMul;
		return y;
	}
}
