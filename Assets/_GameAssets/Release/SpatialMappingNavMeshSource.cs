// ============================================================
// SpatialMappingNavMeshSource.cs
//
// Reference: 
//   HoloLens の空間マップで NavMesh を使ってみる
//   https://tarukosu.hatenablog.com/entry/2017/04/23/183546
// ============================================================

using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;

[RequireComponent(typeof(SpatialMappingObserver))]
public class SpatialMappingNavMeshSource : MonoBehaviour
{
	private SpatialMappingObserver surfaceObserver;

	void Start()
	{
		surfaceObserver = gameObject.GetComponent<SpatialMappingObserver>();
		surfaceObserver.SurfaceAdded += SurfaceObserver_SurfaceAdded;
	}

	private void SurfaceObserver_SurfaceAdded(object sender, DataEventArgs<SpatialMappingSource.SurfaceObject> e)
	{
		AddNavMeshSourceTag(e.Data.Object);
	}

	private void SurfaceObserver_SurfaceUpdated(object sender, DataEventArgs<SpatialMappingSource.SurfaceUpdate> e)
	{
		AddNavMeshSourceTag(e.Data.New.Object);
	}

	private void AddNavMeshSourceTag(GameObject surfaceObject)
	{
		if (surfaceObject.GetComponent<NavMeshSourceTag>() == null)
		{
			surfaceObject.AddComponent<NavMeshSourceTag>();
		}
	}
}
