using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ColorPalette : MonoBehaviour {
	
	public Color main;
	private Color oldMain;
	public List<Color> complementary;
	public List<Color> splitComplementary;
	public List<Color> doubleComplementary;
	public List<Color> rainbow;

	public HSBColor mainHSB;



	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
		if(main != oldMain){
			mainHSB = HSBColor.FromColor(main);
			UpdateColors();
			oldMain = main;
		}




	
	}

	public List<Color> GetComplementary(int colorsNum){
		List<Color> colors = new List<Color>();

		for(int i = 0; i < colorsNum; i++){
			HSBColor col = new HSBColor((mainHSB.h + (i * (1/(float)colorsNum))) % 1, mainHSB.s, mainHSB.b, mainHSB.a);
			colors.Add(HSBColor.ToColor(col));
		}

		return colors;


	}

	public void UpdateColors(){
		complementary = GetComplementary(2);
		splitComplementary = GetComplementary(3);
		doubleComplementary = GetComplementary(4);
		rainbow = GetComplementary(6);
	}
}
