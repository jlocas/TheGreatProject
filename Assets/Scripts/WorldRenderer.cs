using UnityEngine;
using System.Collections;

public class WorldRenderer : MonoBehaviour {

	public WorldData world;
	
	public bool showGrid;

	public bool hardEdges;
	private bool isHardEdges;
	
	Mesh[] smoothMesh;
	Mesh[] hardMesh;

	private GameObject[] worldMeshes;
	private int size;
	
	[Space(20)]
	public Material material;
	public HeightmapDrawer heightmapDrawer;



	// Use this for initialization
	public void Render () {

		heightmapDrawer.Draw();

		size = world.GetWorldSize();



		if (hardEdges){ 

			isHardEdges = true;
			GenerateHardMesh();
		}
		else
		{
			isHardEdges = false;
			GenerateSmoothMesh();

		}

		for(int i = 0; i < worldMeshes.Length; i++)
		{
			worldMeshes[i].GetComponent<MeshRenderer>().material.mainTexture = heightmapDrawer.GetTexture();
		}


		//DrawWater();
	
	}
	
	// Update is called once per frame
	void Update () {


		if( hardEdges && !isHardEdges ){
			if(smoothMesh != null){
				for (int i = 0; i < smoothMesh.Length; i++){
					Destroy (smoothMesh[i]);
				}
			}

			GenerateHardMesh();
			isHardEdges = true;
		}
		else if( !hardEdges && isHardEdges ){

			if(hardMesh != null){
				for (int i = 0; i < hardMesh.Length; i++){
					Destroy (hardMesh[i]);
				}
			}

			GenerateSmoothMesh();
			isHardEdges = false;
		}
	}









	void GenerateSmoothMesh(){


		int numVertsCorners = (size+1) * (size+1);
		int numVertsMiddleX = (size+1) * size;
		int numVertsMiddleZ = size * (size+1);
		int numVertsCenter = size * size;

		int numVertTotal = numVertsCorners + numVertsMiddleX + numVertsMiddleZ + numVertsCenter;
		Debug.Log("Nombre total de vertices  " + numVertTotal);

		Vector3[] verticesCorners = new Vector3[numVertsCorners];
		Vector3[] verticesMiddleX = new Vector3[numVertsMiddleX];
		Vector3[] verticesMiddleZ = new Vector3[numVertsMiddleZ];
		Vector3[] verticesCenter = new Vector3[numVertsCenter];

		Vector3[] vertices = new Vector3[numVertTotal];
		Vector3[] normals = new Vector3[numVertTotal];
		Vector2[] uv = new Vector2[numVertTotal];

		int numTris = size * size * 8;
		int[] triangles = new int[ numTris * 3 ];

		// FYI
		//
		//  ------->  x axis
		//  |
		//  |
		//  v
		//
		//  z axis
		//

		// Filling center vertices 
		//
		//    o--o--o
		//    |     |
		//    o  x  o
		//    |     |
		//    o--o--o
		//
		for(int z=0; z < size; z++) {
			for(int x=0; x < size; x++) {
				float elevationTemp = world.getElevation(x,z);
				verticesCenter[z * size + x] = new Vector3( x, elevationTemp, z);
			}
		}

		// Filling corner vertices 
		//
		//    x--o--x
		//    |     |
		//    o  o  o
		//    |     |
		//    x--o--x
		//
		for(int z=0; z < (size+1); z++) {
			for(int x=0; x < (size+1); x++) {

				float elevationTemp = (world.getElevation(x,z) + world.getElevation(x-1,z) + world.getElevation(x,z-1) + world.getElevation(x-1,z-1)) / 4.0f ;
				verticesCorners[ z * (size+1) + x ] = new Vector3( x-0.5f, elevationTemp, z-0.5f);
			}
		}

		// Filling the middle Z vertices 
		//
		//    o--o--o
		//    |     |
		//    x  o  x
		//    |     |
		//    o--o--o
		//
		for(int z=0; z < size; z++) {
			for(int x=0; x < (size+1); x++) {
				float elevationTemp = (world.getElevation(x,z) + world.getElevation(x-1,z)) / 2.0f ;
				verticesMiddleZ[ z * (size+1) + x ] = new Vector3( x-0.5f, elevationTemp, z);
			}
		}

		// Filling the middle X vertices
		//
		//    o--x--o
		//    |     |
		//    o  o  o
		//    |     |
		//    o--x--o
		//
		for(int z=0; z < (size+1); z++) {
			for(int x=0; x < size; x++) {
				float elevationTemp = (world.getElevation(x,z) + world.getElevation(x,z-1)) / 2.0f ;
				verticesMiddleX[ z * size + x ] = new Vector3( x, elevationTemp, z-0.5f);
			}
		}
		 
		int vertTest = 0;

		// Mesh vertices
		for(int z=0; z < size; z++) {
			for(int x=0; x < size; x++) {

				int sizeVert = size * 2 + 1;

				vertices[ ( 2 * x + 0 ) + ( (2 * z + 0) * sizeVert ) ] = verticesCorners[z * (size+1) + x];
				vertices[ ( 2 * x + 1 ) + ( (2 * z + 0) * sizeVert ) ] = verticesMiddleX[z * size + x];
				vertices[ ( 2 * x + 0 ) + ( (2 * z + 1) * sizeVert ) ] = verticesMiddleZ[z * (size+1) + x];
				vertices[ ( 2 * x + 1 ) + ( (2 * z + 1) * sizeVert ) ] = verticesCenter[z * size + x];

				vertTest += 4;

				normals[ ( 2 * x + 0 ) + ( (2 * z + 0) * sizeVert ) ] = Vector3.up;
				normals[ ( 2 * x + 1 ) + ( (2 * z + 0) * sizeVert ) ] = Vector3.up;
				normals[ ( 2 * x + 0 ) + ( (2 * z + 1) * sizeVert ) ] = Vector3.up;
				normals[ ( 2 * x + 1 ) + ( (2 * z + 1) * sizeVert ) ] = Vector3.up;

				uv[ ( 2 * x + 0 ) + ( (2 * z + 0) * sizeVert ) ] = new Vector2( (float)( 2 * x + 0 ) * sizeVert / size, (float)( (2 * z + 0) * sizeVert  ) / size );
				uv[ ( 2 * x + 1 ) + ( (2 * z + 0) * sizeVert ) ] = new Vector2( (float)( 2 * x + 1 ) * sizeVert / size, (float)( (2 * z + 0) * sizeVert  ) / size );
				uv[ ( 2 * x + 0 ) + ( (2 * z + 1) * sizeVert ) ] = new Vector2( (float)( 2 * x + 0 ) * sizeVert / size, (float)( (2 * z + 1) * sizeVert  ) / size );
				uv[ ( 2 * x + 1 ) + ( (2 * z + 1) * sizeVert ) ] = new Vector2( (float)( 2 * x + 1 ) * sizeVert / size, (float)( (2 * z + 1) * sizeVert  ) / size );

				if (x == size-1){
					vertices[ ( 2 * x + 2 ) + ( (2 * z + 0) * sizeVert ) ] = verticesCorners[z * (size+1) + (x+1)];
					vertices[ ( 2 * x + 2 ) + ( (2 * z + 1) * sizeVert ) ] = verticesMiddleZ[z * (size+1) + (x+1)];

					normals[ ( 2 * x + 2 ) + ( (2 * z + 0) * sizeVert ) ] = Vector3.up;
					normals[ ( 2 * x + 2 ) + ( (2 * z + 1) * sizeVert ) ] = Vector3.up;

					uv[ ( 2 * x + 2 ) + ( (2 * z + 0) * sizeVert ) ] = new Vector2( (float)( 2 * x + 2 ) * sizeVert / size, (float)( (2 * z + 0) * sizeVert ) / size );
					uv[ ( 2 * x + 2 ) + ( (2 * z + 1) * sizeVert ) ] = new Vector2( (float)( 2 * x + 2 ) * sizeVert / size, (float)( (2 * z + 1) * sizeVert ) / size );

					vertTest += 2;

				}

				if (z == size-1){
					vertices[ ( 2 * x + 0 ) + ( (2 * z + 2) * sizeVert ) ] = verticesCorners[(z+1) * (size+1) + x];
					vertices[ ( 2 * x + 1 ) + ( (2 * z + 2) * sizeVert ) ] = verticesMiddleX[(z+1) * size + x];

					normals[ ( 2 * x + 0 ) + ( (2 * z + 2) * sizeVert ) ] = Vector3.up;
					normals[ ( 2 * x + 1 ) + ( (2 * z + 2) * sizeVert ) ] = Vector3.up;

					uv[ ( 2 * x + 0 ) + ( (2 * z + 2) * sizeVert ) ] = new Vector2( (float)( 2 * x + 0 ) * sizeVert / size, (float)( (2 * z + 2) * sizeVert  ) / size );
					uv[ ( 2 * x + 1 ) + ( (2 * z + 2) * sizeVert ) ] = new Vector2( (float)( 2 * x + 1 ) * sizeVert / size, (float)( (2 * z + 2) * sizeVert  ) / size );

					vertTest += 2;
				}

				if (x == size-1 && z == size-1){
					vertices[ ( 2 * x + 2 ) + ( (2 * z + 2) * sizeVert ) ] = verticesCorners[(z+1) * (size+1) + (x+1)];
					normals[ ( 2 * x + 2 ) + ( (2 * z + 2) * sizeVert ) ] = Vector3.up;
					uv[ ( 2 * x + 2 ) + ( (2 * z + 2) * sizeVert ) ] = new Vector2( (float)( 2 * x + 2 ) * sizeVert / size, (float)( (2 * z + 2) * sizeVert ) / size );

					vertTest++;
				}
			}
		}

		Debug.Log ("Smooth vertices done! Total vertices: " + vertTest);
	

		int trigCount = 0;

		// Triangles
		//
		//    1--2--3
		//    |     |
		//    4  5  6
		//    |     |
		//    7--8--9
		//
		for(int x=0; x < size; x++) {
			for(int z=0; z < size; z++) {

				int sizeVert = 2 * size + 1; 

				int squareIndex = z * size + x;
				int triOffset = squareIndex * 3 * 8;

				int V1 =  (2 * x + 0 ) + ( (2 * z + 0) * sizeVert );
				int V2 =  (2 * x + 1 ) + ( (2 * z + 0) * sizeVert );
				int V3 =  (2 * x + 2 ) + ( (2 * z + 0) * sizeVert );
				int V4 =  (2 * x + 0 ) + ( (2 * z + 1) * sizeVert );
				int V5 =  (2 * x + 1 ) + ( (2 * z + 1) * sizeVert );
				int V6 =  (2 * x + 2 ) + ( (2 * z + 1) * sizeVert );
				int V7 =  (2 * x + 0 ) + ( (2 * z + 2) * sizeVert );
				int V8 =  (2 * x + 1 ) + ( (2 * z + 2) * sizeVert );
				int V9 =  (2 * x + 2 ) + ( (2 * z + 2) * sizeVert );

				triangles[triOffset +  0] = V5;
				triangles[triOffset +  1] = V2;
				triangles[triOffset +  2] = V1;
				
				triangles[triOffset +  3] = V5;
				triangles[triOffset +  4] = V3;
				triangles[triOffset +  5] = V2;

				triangles[triOffset +  6] = V5;
				triangles[triOffset +  7] = V6;
				triangles[triOffset +  8] = V3;

				triangles[triOffset +  9] = V5;
				triangles[triOffset + 10] = V9;
				triangles[triOffset + 11] = V6;

				triangles[triOffset + 12] = V5;
				triangles[triOffset + 13] = V8;
				triangles[triOffset + 14] = V9;

				triangles[triOffset + 15] = V5;
				triangles[triOffset + 16] = V7;
				triangles[triOffset + 17] = V8;

				triangles[triOffset + 18] = V5;
				triangles[triOffset + 19] = V4;
				triangles[triOffset + 20] = V7;

				triangles[triOffset + 21] = V5;
				triangles[triOffset + 22] = V1;
				triangles[triOffset + 23] = V4;

				trigCount += 24;
			}
		}
		
		Debug.Log ("Smooth triangles done! Total triangles: " + trigCount);




		// Dividing the mesh into smaller meshes for rendering
		//
		//

		int vertsPerCubeLine = 2 * (2 * size + 1);
		int trigsPerCubeLine = 3 * 8 * size;

		int maxLines = (int)((64998 - (2 * size + 1)) / vertsPerCubeLine);
		int maxVerts = maxLines * vertsPerCubeLine + (2 * size + 1);
		int maxTrigs = maxLines * trigsPerCubeLine;

		int numMeshes = (numVertTotal/maxVerts) + 1;

		smoothMesh = new Mesh[numMeshes];
		worldMeshes = new GameObject[numMeshes];

		for( int i = 0; i < numMeshes; i++ ){

			int vertIndex = (maxVerts - (2 * size + 1)) * i;
			int trigIndex = maxTrigs * i;

			Vector3[] verticesTemp;
			int[] trianglesTemp;

			//Last Mesh
			if( i == (numMeshes - 1) ){

				verticesTemp = new Vector3[numVertTotal - vertIndex];
				trianglesTemp = new int[(numTris * 3) - trigIndex];

				for( int v = 0; v < numVertTotal - vertIndex; v++ ){
					verticesTemp[v] = vertices[vertIndex + v];
				}

				for( int t = 0; t < (numTris * 3) - trigIndex; t++ ){
					trianglesTemp[t] = triangles[trigIndex + t] - vertIndex ;
				}
			}

			else{

				verticesTemp = new Vector3[maxVerts];
				trianglesTemp = new int[maxTrigs];

				//Debug.Log ("vertIndex = " + vertIndex);
				//Debug.Log ("numVertTotal = " + numVertTotal);

				for( int v = 0; v < maxVerts; v++){
					verticesTemp[v] = vertices[vertIndex + v];
				}

				//Debug.Log ("trigIndex = " + vertIndex);
				//Debug.Log ("numTris * 3 = " + (numTris * 3));

				for( int t = 0; t < maxTrigs; t++ ){
					trianglesTemp[t] = triangles[trigIndex + t] - vertIndex;
				}
			}

			//Debug.Log ("Mesh number " + i);

			smoothMesh[i] = new Mesh();
			worldMeshes[i] = new GameObject( "World Mesh " + i );
			worldMeshes[i].transform.parent = gameObject.transform;

			worldMeshes[i].AddComponent<MeshFilter>();
			worldMeshes[i].AddComponent<MeshRenderer>();
			worldMeshes[i].AddComponent<MeshCollider>();


			smoothMesh[i].vertices = verticesTemp;
			smoothMesh[i].triangles = trianglesTemp;

			//System.Array.Clear(verticesTemp, 0, verticesTemp.Length);
			//System.Array.Clear(trianglesTemp, 0, trianglesTemp.Length);


			//smoothMesh[i].uv = uv;
			//smoothMesh[i].normals = normals;
			smoothMesh[i].RecalculateNormals();

			worldMeshes[i].GetComponent<MeshFilter>().mesh = smoothMesh[i];
			worldMeshes[i].GetComponent<MeshCollider>().sharedMesh = smoothMesh[i];
			worldMeshes[i].GetComponent<MeshRenderer>().material = material;


		}
		Debug.Log ("Done Mesh!");

	}







	void GenerateHardMesh(){
			
		
		int numVertsCorners = (size+1) * (size+1);
		int numVertsMiddleX = (size+1) * size;
		int numVertsMiddleZ = size * (size+1);
		int numVertsCenter = size * size;

		Vector3[] verticesCorners = new Vector3[numVertsCorners];
		Vector3[] verticesMiddleX = new Vector3[numVertsMiddleX];
		Vector3[] verticesMiddleZ = new Vector3[numVertsMiddleZ];
		Vector3[] verticesCenter = new Vector3[numVertsCenter];

		int numTris = size * size * 8;
		int numVertTotal = numTris * 3;

		Debug.Log("Nombre total de vertices  " + numVertTotal);

		int[] triangles = new int[ numVertTotal ];
		Vector3[] vertices = new Vector3[ numVertTotal ];
		Vector3[] normals = new Vector3[ numVertTotal ];
		Vector2[] uv = new Vector2[ numVertTotal ];

		
		// FYI
		//
		//  ------->  x axis
		//  |
		//  |
		//  v
		//
		//  z axis
		//
		
		// Filling center vertices 
		//
		//    o--o--o
		//    |     |
		//    o  x  o
		//    |     |
		//    o--o--o
		//
		for(int z=0; z < size; z++) {
			for(int x=0; x < size; x++) {
				float elevationTemp = world.getElevation(x,z);
				verticesCenter[z * size + x] = new Vector3( x, elevationTemp, z);
			}
		}
		
		// Filling corner vertices 
		//
		//    x--o--x
		//    |     |
		//    o  o  o
		//    |     |
		//    x--o--x
		//
		for(int z=0; z < (size+1); z++) {
			for(int x=0; x < (size+1); x++) {
				
				float elevationTemp = (world.getElevation(x,z) + world.getElevation(x-1,z) + world.getElevation(x,z-1) + world.getElevation(x-1,z-1)) / 4.0f ;
				verticesCorners[ z * (size+1) + x ] = new Vector3( x-0.5f, elevationTemp, z-0.5f);
			}
		}
		
		// Filling the middle Z vertices 
		//
		//    o--o--o
		//    |     |
		//    x  o  x
		//    |     |
		//    o--o--o
		//
		for(int z=0; z < size; z++) {
			for(int x=0; x < (size+1); x++) {
				float elevationTemp = (world.getElevation(x,z) + world.getElevation(x-1,z)) / 2.0f ;
				verticesMiddleZ[ z * (size+1) + x ] = new Vector3( x-0.5f, elevationTemp, z);
			}
		}
		
		// Filling the middle X vertices
		//
		//    o--x--o
		//    |     |
		//    o  o  o
		//    |     |
		//    o--x--o
		//
		for(int z=0; z < (size+1); z++) {
			for(int x=0; x < size; x++) {
				float elevationTemp = (world.getElevation(x,z) + world.getElevation(x,z-1)) / 2.0f ;
				verticesMiddleX[ z * size + x ] = new Vector3( x, elevationTemp, z-0.5f);
			}
		}
		
		int vertTest = 0;
		
		
		// Mesh vertices
		//
		//    1--2--3
		//    |     |
		//    4  5  6
		//    |     |
		//    7--8--9
		//
		for(int z=0; z < size; z++) {
			for(int x=0; x < size; x++) {
				
				//int index = 0;
				int squareIndex = z * size + x;
				int triOffset = squareIndex * 3 * 8;

				Vector3 V1 = verticesCorners[z * (size+1) + x];
				Vector3 V2 = verticesMiddleX[z * size + x];
				Vector3 V3 = verticesCorners[z * (size+1) + (x+1)];
				Vector3 V4 = verticesMiddleZ[z * (size+1) + x];
				Vector3 V5 = verticesCenter[z * size + x];
				Vector3 V6 = verticesMiddleZ[z * (size+1) + (x+1)];
				Vector3 V7 = verticesCorners[(z+1) * (size+1) + x];
				Vector3 V8 = verticesMiddleX[(z+1) * size + x];
				Vector3 V9 = verticesCorners[(z+1) * (size+1) + (x+1)];


				vertices[ triOffset +  0 ] = V5;
				vertices[ triOffset +  1 ] = V1;
				vertices[ triOffset +  2 ] = V4;

				uv[ triOffset +  0 ] = new Vector2( V5.x / size, V5.z / size );
				uv[ triOffset +  1 ] = new Vector2( V1.x / size, V1.z / size );
				uv[ triOffset +  2 ] = new Vector2( V4.x / size, V4.z / size );

				vertices[ triOffset +  3 ] = V5;
				vertices[ triOffset +  4 ] = V4;
				vertices[ triOffset +  5 ] = V7;

				uv[ triOffset +  3 ] = new Vector2( V5.x / size, V5.z / size );
				uv[ triOffset +  4 ] = new Vector2( V4.x / size, V4.z / size );
				uv[ triOffset +  5 ] = new Vector2( V7.x / size, V7.z / size );

				vertices[ triOffset +  6 ] = V5;
				vertices[ triOffset +  7 ] = V7;
				vertices[ triOffset +  8 ] = V8;

				uv[ triOffset +  6 ] = new Vector2( V5.x / size, V5.z / size );
				uv[ triOffset +  7 ] = new Vector2( V7.x / size, V7.z / size );
				uv[ triOffset +  8 ] = new Vector2( V8.x / size, V8.z / size );

				vertices[ triOffset +  9 ] = V5;
				vertices[ triOffset + 10 ] = V8;
				vertices[ triOffset + 11 ] = V9;

				uv[ triOffset +  9 ] = new Vector2( V5.x / size, V5.z / size );
				uv[ triOffset + 10 ] = new Vector2( V8.x / size, V8.z / size );
				uv[ triOffset + 11 ] = new Vector2( V9.x / size, V9.z / size );

				vertices[ triOffset + 12 ] = V5;
				vertices[ triOffset + 13 ] = V9;
				vertices[ triOffset + 14 ] = V6;

				uv[ triOffset + 12 ] = new Vector2( V5.x / size, V5.z / size );
				uv[ triOffset + 13 ] = new Vector2( V9.x / size, V9.z / size );
				uv[ triOffset + 14 ] = new Vector2( V6.x / size, V6.z / size );

				vertices[ triOffset + 15 ] = V5;
				vertices[ triOffset + 16 ] = V6;
				vertices[ triOffset + 17 ] = V3;

				uv[ triOffset + 15 ] = new Vector2( V5.x / size, V5.z / size );
				uv[ triOffset + 16 ] = new Vector2( V6.x / size, V6.z / size );
				uv[ triOffset + 17 ] = new Vector2( V3.x / size, V3.z / size );

				vertices[ triOffset + 18 ] = V5;
				vertices[ triOffset + 19 ] = V3;
				vertices[ triOffset + 20 ] = V2;

				uv[ triOffset + 18 ] = new Vector2( V5.x / size, V5.z / size );
				uv[ triOffset + 19 ] = new Vector2( V3.x / size, V3.z / size );
				uv[ triOffset + 20 ] = new Vector2( V2.x / size, V2.z / size );

				vertices[ triOffset + 21 ] = V5;
				vertices[ triOffset + 22 ] = V2;
				vertices[ triOffset + 23 ] = V1;

				uv[ triOffset + 21 ] = new Vector2( V5.x / size, V5.z / size );
				uv[ triOffset + 22 ] = new Vector2( V2.x / size, V2.z / size );
				uv[ triOffset + 23 ] = new Vector2( V1.x / size, V1.z / size );


			}
		}
		
		Debug.Log ("Done Verts! Verts tota: " + vertTest);
		
		
		
		// Triangles
		//
		//    1--2--3
		//    |     |
		//    4  5  6
		//    |     |
		//    7--8--9
		//
		for(int x=0; x < size; x++) {
			for(int z=0; z < size; z++) {

				int squareIndex = z * size + x;
				int triOffset = squareIndex * 3 * 8;

				
				triangles[triOffset +  0] = triOffset +  0;
				triangles[triOffset +  1] = triOffset +  1;
				triangles[triOffset +  2] = triOffset +  2;
				
				triangles[triOffset +  3] = triOffset +  3;
				triangles[triOffset +  4] = triOffset +  4;
				triangles[triOffset +  5] = triOffset +  5;
				
				triangles[triOffset +  6] = triOffset +  6;
				triangles[triOffset +  7] = triOffset +  7;
				triangles[triOffset +  8] = triOffset +  8;
				
				triangles[triOffset +  9] = triOffset +  9;
				triangles[triOffset + 10] = triOffset + 10;
				triangles[triOffset + 11] = triOffset + 11;
				
				triangles[triOffset + 12] = triOffset + 12;
				triangles[triOffset + 13] = triOffset + 13;
				triangles[triOffset + 14] = triOffset + 14;
				
				triangles[triOffset + 15] = triOffset + 15;
				triangles[triOffset + 16] = triOffset + 16;
				triangles[triOffset + 17] = triOffset + 17;
				
				triangles[triOffset + 18] = triOffset + 18;
				triangles[triOffset + 19] = triOffset + 19;
				triangles[triOffset + 20] = triOffset + 20;
				
				triangles[triOffset + 21] = triOffset + 21;
				triangles[triOffset + 22] = triOffset + 22;
				triangles[triOffset + 23] = triOffset + 23;
			}
		}
		
		Debug.Log ("Done Triangles!");



		// Dividing the mesh into smaller meshes for rendering
		//
		//
		
		int vertsPerCubeLine = 24 * size;
		int trigsPerCubeLine = 24 * size;
		
		//Debug.Log ("vertsPerCubeLine = " + vertsPerCubeLine);
		//Debug.Log ("trigsPerCubeLine = " + trigsPerCubeLine);
		
		int maxLines = (int)(64998 / vertsPerCubeLine);
		int maxVerts = maxLines * vertsPerCubeLine;
		int maxTrigs = maxLines * trigsPerCubeLine;
		
		//Debug.Log ("maxLines = " + maxLines);
		//Debug.Log ("maxVerts = " + maxVerts);
		//Debug.Log ("maxTrigs = " + maxTrigs);
		
		int numMeshes = (numVertTotal/maxVerts) + 1;
		
		//Debug.Log ("numMeshes = " + numMeshes);
		
		hardMesh = new Mesh[numMeshes];
		worldMeshes = new GameObject[numMeshes];
		
		for( int i = 0; i < numMeshes; i++ ){
			
			int vertIndex = maxVerts * i;
			int trigIndex = maxTrigs * i;
			
			Vector3[] verticesTemp;
			int[] trianglesTemp;
			Vector2[] uvTemp;
			
			//Debug.Log ("i = " + i);
			
			//Last Mesh
			if( i == (numMeshes - 1) ){
				
				verticesTemp = new Vector3[numVertTotal - vertIndex];
				trianglesTemp = new int[(numTris * 3) - trigIndex];
				uvTemp = new Vector2[numVertTotal - vertIndex];
				
				//Debug.Log ("Triangles temp lenght " + trianglesTemp.Length);
				
				//Debug.Log ("vertIndex = " + vertIndex);
				//Debug.Log ("numVertTotal = " + numVertTotal);
				
				for( int v = 0; v < numVertTotal - vertIndex; v++ ){
					verticesTemp[v] = vertices[vertIndex + v];
					/*if (v < 5){
						Debug.Log ("verticesTemp[v] = "+ verticesTemp[v]);
					}*/
				}

				for( int v = 0; v < numVertTotal - vertIndex; v++ ){
					uvTemp[v] = uv[vertIndex + v];
					/*if (v < 5){
						Debug.Log ("verticesTemp[v] = "+ verticesTemp[v]);
					}*/
				}
				
				//Debug.Log ("trigIndex = " + trigIndex);
				//Debug.Log ("numTris * 3 = " + (numTris * 3));
				
				for( int t = 0; t < (numTris * 3) - trigIndex; t++ ){
					trianglesTemp[t] = triangles[trigIndex + t] - vertIndex ;
					/*if (t < 5){
						Debug.Log ("trianglesTemp[t] = "+ trianglesTemp[t]);
					}*/
				}
			}
			
			else{
				
				verticesTemp = new Vector3[maxVerts];
				trianglesTemp = new int[maxTrigs];
				uvTemp = new Vector2[maxVerts];
				
				//Debug.Log ("vertIndex = " + vertIndex);
				//Debug.Log ("numVertTotal = " + numVertTotal);
				
				for( int v = 0; v < maxVerts; v++){
					verticesTemp[v] = vertices[vertIndex + v];
				}

				for( int v = 0; v < maxVerts; v++){
					uvTemp[v] = uv[vertIndex + v];
				}

				
				//Debug.Log ("trigIndex = " + vertIndex);
				//Debug.Log ("numTris * 3 = " + (numTris * 3));
				
				for( int t = 0; t < maxTrigs; t++ ){
					trianglesTemp[t] = triangles[trigIndex + t] - vertIndex;
				}
			}
			
			//Debug.Log ("Mesh number " + i);
			
			hardMesh[i] = new Mesh();
			worldMeshes[i] = new GameObject( "World Mesh " + i );
			worldMeshes[i].transform.parent = gameObject.transform;
			
			worldMeshes[i].AddComponent<MeshFilter>();
			worldMeshes[i].AddComponent<MeshRenderer>();
			worldMeshes[i].AddComponent<MeshCollider>();
			
			
			hardMesh[i].vertices = verticesTemp;
			hardMesh[i].uv = uvTemp;
			hardMesh[i].triangles = trianglesTemp;
			
			//System.Array.Clear(verticesTemp, 0, verticesTemp.Length);
			//System.Array.Clear(trianglesTemp, 0, trianglesTemp.Length);
			
			
			//smoothMesh[i].uv = uv;
			//smoothMesh[i].normals = normals;
			hardMesh[i].RecalculateNormals();
			
			worldMeshes[i].GetComponent<MeshFilter>().mesh = hardMesh[i];
			worldMeshes[i].GetComponent<MeshCollider>().sharedMesh = hardMesh[i];
			worldMeshes[i].GetComponent<MeshRenderer>().material = material;

		}


	}


		/*
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


	private void DrawWater(){

		//Vector3 waterPos = new Vector3((float)size/2, 0.0f, (float)size/2);
		
		//GameObject waterPlane = Instantiate(waterSurface, waterPos, Quaternion.identity) as GameObject;
		
		//waterPlane.transform.localScale = new Vector3((float)size/10, 1.0f, (float)size/10);
	}

	public void SetTexture(Texture2D tex){
		GetComponent<Renderer>().material.mainTexture = tex;
	}
	
}
