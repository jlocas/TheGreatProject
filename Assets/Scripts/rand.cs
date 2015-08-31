using UnityEngine;
using System.Collections;

public class rand {

	public float max(int times){

		if(times == null){
			times = 2;
		}

		float[] values = new float[times];

		for(int i = 0; i < times; i++){
			float rand = Random.value;
			values[i] = Random.value;
		}

		return Mathf.Max(values);
	}

	public float min(int times){
		
		if(times == null){
			times = 2;
		}
		
		float[] values = new float[times];
		
		for(int i = 0; i < times; i++){
			float rand = Random.value;
			values[i] = Random.value;
		}
		
		return Mathf.Min(values);
	}

	public float tri(int times){
		
		if(times == null){
			times = 2;
		}
		
		float[] values = new float[times];
		float avg = 0f;

		for(int i = 0; i < times; i++){
			float rand = Random.value;
			values[i] = Random.value;
		}

		for(int i = 0; i < times; i++){
			avg += values[i];
		}
		avg /= times;
		return avg;
	}

	public float exp(float lambda, int dir){
		//lambda: raideur de la pente
		//dir: 0 favorise les petites valeurs, 1 les grandes
		float value = Mathf.Log10(Random.value) / lambda;

		if(dir == 1){
			return 1f - value;
		} else if (dir == 0) {
			return value;
		} else {
			Debug.Log("rand.exp: direction argument needs EITHER 0 or 1, please fix");
			return 0f;
		}
	}

}
