using UnityEngine;
using System.Collections;

public class JTree : MonoBehaviour {

	public float yOffset;

	public float growthScale;
	public float growthRate;
	public float maxGrowth;

	public Tree tree;
	public AnimationCurve curve;


	//0 = seedling
	//1 = juvenile
	//2 = mature
	public int stage;

	// Use this for initialization
	void Start () {
		Plant ();
	
	}
	
	// Update is called once per frame
	void Update () {
		Grow();
	
	}

	void Plant(){
		RaycastHit hitInfo;
		Physics.Raycast(gameObject.transform.position, Vector3.down, out hitInfo);

		gameObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + yOffset * growthScale, hitInfo.point.z);
	}

	void Grow(){
		growthScale += growthRate;

		gameObject.transform.localScale = new Vector3(growthScale, growthScale, growthScale);
	}
}
