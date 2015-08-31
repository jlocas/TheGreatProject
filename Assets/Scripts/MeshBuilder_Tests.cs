using UnityEngine;
using System.Collections;

public class MeshBuilder_Tests : MonoBehaviour {

	MeshBuilder meshBuilder = new MeshBuilder();
	public float m_Length = 1f;
	public float m_Width = 1f;
	public Vector3 offset;
	// Use this for initialization
	void Start () {
		/*//Set up the vertices and triangles:
		meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, 0.0f));
		meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, m_Length));
		meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.Vertices.Add(new Vector3(m_Width, 0.0f, m_Length));
		meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.Vertices.Add(new Vector3(m_Width, 0.0f, 0.0f));
		meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.AddTriangle(0, 1, 2);
		meshBuilder.AddTriangle(0, 2, 3);
		
		//Create the mesh:
		MeshFilter filter = GetComponent<MeshFilter>();
		
		if (filter != null)
		{
			filter.sharedMesh = meshBuilder.CreateMesh();
		}*/


		BuildQuad(meshBuilder, offset);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void BuildQuad(MeshBuilder meshBuilder, Vector3 offset)
	{
		meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, 0.0f) + offset);
		meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, m_Length) + offset);
		meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.Vertices.Add(new Vector3(m_Width, 0.0f, m_Length) + offset);
		meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.Vertices.Add(new Vector3(m_Width, 0.0f, 0.0f) + offset);
		meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		int baseIndex = meshBuilder.Vertices.Count - 4;
		
		meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
		meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
	}
}


