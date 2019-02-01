using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshVisualizer : MonoBehaviour
{
	public Material NavMeshMaterial;

	private MeshFilter filter;

	void Start()
	{
		filter = gameObject.GetComponent<MeshFilter>();
		if (filter==null)
		{
			filter = gameObject.AddComponent<MeshFilter>();
		}

		MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
		if(renderer==null)
		{
			renderer = gameObject.AddComponent<MeshRenderer>();
		}
		renderer.material = NavMeshMaterial;
	}

	void Update()
	{
		NavMeshTriangulation triangles = NavMesh.CalculateTriangulation();
		Mesh mesh = new Mesh();
		mesh.vertices = triangles.vertices;
		mesh.triangles = triangles.indices;
        filter.mesh = mesh;
	}
}
