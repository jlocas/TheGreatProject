using UnityEngine;
using System.Collections;

public class SmoothMesher : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*
	
	
	
	
	
	
	
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
		
	}*/
	

}
