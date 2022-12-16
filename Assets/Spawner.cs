using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] Ship shipPrefab;
    [SerializeField] Path path;
    [SerializeField] float spawnCooldownSec = 10f;
    Camera _camera;
    void Start()
    {
        _camera = Camera.main;
        StartCoroutine(SpawnLoop());
    }

    [ContextMenu(nameof(Spawn))]
    public bool Spawn()
    {
        if (IsTargetVisible(_camera, gameObject)) return false;

        var ship = Instantiate(shipPrefab, transform);
        int wpId = path.GetNearestWayPointId(transform.position);
        ship.currentDestinationId = wpId;
        ship.transform.LookAt(path.wayPoints[wpId]);
        ship.path = path;
        return true;
    }

    bool IsTargetVisible(Camera c,GameObject go)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(c);
        var point = go.transform.position;
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
                return false;
        }
        return true;
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if(ShipsManager.Instance.RequestSpawnApproval())
                Spawn();
            yield return new WaitForSeconds(spawnCooldownSec);
        }
    }
}
