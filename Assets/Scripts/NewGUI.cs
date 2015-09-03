using UnityEngine;
using System.Collections;
using UnityEditor;

public class NewGUI : MonoBehaviour {
	public float min, max;
	//public const float minLimit, maxLimit;
	
	void OnGUI () {
		EditorGUILayout.MinMaxSlider (ref min, ref max, -10f, 10f);
		
		//min and max will now store the returning value of the use
	}
	
}

