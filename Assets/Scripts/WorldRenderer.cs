using UnityEngine;
using System.Collections;

[System.Serializable]
public class WorldRenderer : MonoBehaviour {

	[Range(3, 8)]
	public int nthPower;

	[Range(1, 8)]
	public int featureSizePower;

	[Range(1, 20)]
	public float heightScale;

	[Range(1f, 10.0f)]
	public float heightStep;


	public GameObject waterSurface;
	public GameObject worldBlock;


	private WorldGenerator world;
	private GameObject[,] worldBlocks;
	private int size;


	// Use this for initialization
	void Start () {

		world = new WorldGenerator(nthPower, featureSizePower, heightScale, heightStep);

		size = world.getWorldSize();

		worldBlocks = new GameObject[size, size];



		firstDrawWorld();
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//drawWorld();

	}
/*
	void GenerateGridMesh(){
		
		// Tiles (quads) and triangles
		int numTiles = size * size;
		int numTris = numTiles * 2;
		
		// Vertices array
		int vsize_x = width + 1;
		int vsize_y = height + 1;
		int numVerts = vsize_x * vsize_y;
		
		// Generate the mesh data
		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		
		int[] triangles = new int[ numTris * 3 ];
		
		int x, y;
		
		for(y=0; y < vsize_y; y++) {
			for(x=0; x < vsize_x; x++) {
				
				if (x == width && y == height){
					Debug.Log ("tiles["+x+","+y+"].elevation = "+theGrid.tiles[x-1,y-1].elevation + " special round");
					vertices[ y * vsize_x + x ] = new Vector3( x*tileSize, y*tileSize, -theGrid.tiles[x-1,y-1].elevation * slope);
				}
				
				else if(x == width)
				{
					Debug.Log ("tiles["+x+","+y+"].elevation = "+theGrid.tiles[x-1,y].elevation + " special round");
					vertices[ y * vsize_x + x ] = new Vector3( x*tileSize, y*tileSize, -theGrid.tiles[x-1,y].elevation * slope);
				}
				else if(y == height)
				{
					Debug.Log ("tiles["+x+","+y+"].elevation = "+theGrid.tiles[x,y-1].elevation + " special round");
					vertices[ y * vsize_x + x ] = new Vector3( x*tileSize, y*tileSize, -theGrid.tiles[x,y-1].elevation * slope);
				}
				else{
					Debug.Log ("tiles["+x+","+y+"].elevation = "+theGrid.tiles[x,y].elevation);
					vertices[ y * vsize_x + x ] = new Vector3( x*tileSize, y*tileSize, -theGrid.tiles[x,y].elevation * slope);
				}
				
				
				normals[ y * vsize_x + x ] = Vector3.back;
				//uv[ y * vsize_x + x ] = new Vector2( (float)x / width, 1f - (float)y / height );
				uv[ y * vsize_x + x ] = new Vector2( (float)x / width, (float)y / height );
			}
		}
		
		Debug.Log ("Done Verts!");
		
		for(y=0; y < height; y++) {
			for(x=0; x < width; x++) {
				int squareIndex = y * width + x;
				int triOffset = squareIndex * 6;
				triangles[triOffset + 0] = y * vsize_x + x + 		   0;
				triangles[triOffset + 1] = y * vsize_x + x + vsize_x + 0;
				triangles[triOffset + 2] = y * vsize_x + x + vsize_x + 1;
				
				triangles[triOffset + 3] = y * vsize_x + x + 		   0;
				triangles[triOffset + 4] = y * vsize_x + x + vsize_x + 1;
				triangles[triOffset + 5] = y * vsize_x + x + 		   1;
			}
		}
		
		Debug.Log ("Done Triangles!");
		
		// Create a new Mesh and populate with the data
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		
		// Assign our mesh to our filter/renderer/collider
		MeshFilter mesh_filter = GetComponent<MeshFilter>();
		MeshCollider mesh_collider = GetComponent<MeshCollider>();
		
		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;
		Debug.Log ("Done Mesh!");
	}

	
	void GenerateTexture(){
		
		int textureWidth = width * tileResolution;
		int textureHeight = height * tileResolution;
		Texture2D texture = new Texture2D(textureWidth, textureHeight);
		
		//Color[][] tiles = ChopUpTiles();
		
		Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Terrain/Sprites_Terrain");
		
		Color[][] colors = new Color[sprites.Length][];
		//Debug.Log ("Colors lenght*** **** ****" +sprites.Length);
		
		for (int i = 0; i<sprites.Length; i++){
			int x = (int)sprites[i].textureRect.x;
			int y = (int)sprites[i].textureRect.y;
			int dx = (int)sprites[i].textureRect.width;
			int dy = (int)sprites[i].textureRect.height;
			
			colors[i] = sprites[i].texture.GetPixels( x, y, dx, dy );
			Debug.Log ("colors lenght" +colors[i].Length);
		}
		
		
		
		
		for(int y=0; y < height; y++) {
			for(int x=0; x < width; x++) {
				if(theGrid.tiles[x,y].surface == SURFACE.GRASS){
					//texture.SetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution, colors[0]);
					texture.SetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution, colors[0]);
				}
				else if(theGrid.tiles[x,y].surface == SURFACE.CLIFF){
					texture.SetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution, colors[1]);
				}
			}
			
		}
		
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		
		mesh_renderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
		
		mesh_renderer.sharedMaterials[0].mainTexture = texture;
		
		
		Debug.Log ("Done Texture!");
	}
*/
	private void firstDrawWorld(){

		GameObject worldBlocksParent = new GameObject();
		worldBlocksParent.name = "World";
		worldBlocksParent.transform.parent = gameObject.transform;


		for(int x = 0; x < size; x++){
			for(int z = 0; z < size; z++){

				Vector3 pos = new Vector3(x, world.getElevation(x,z), z);
				GameObject blockInstance = Instantiate(worldBlock, pos, Quaternion.identity) as GameObject;
				blockInstance.transform.parent = worldBlocksParent.transform;
				blockInstance.transform.localScale = new Vector3(1.0f, world.getElevation(x,z)+10.0f, 1.0f);
				
				worldBlocks[x,z] = blockInstance;

			}
		}

		Vector3 waterPos = new Vector3((float)size/2, 0.0f, (float)size/2);

		GameObject waterPlane = Instantiate(waterSurface, waterPos, Quaternion.identity) as GameObject;

		waterPlane.transform.localScale = new Vector3((float)size/10, 1.0f, (float)size/10);


	}
}
