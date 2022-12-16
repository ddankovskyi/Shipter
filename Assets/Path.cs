using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Vector3> wayPoints;
    void Awake()
    {
        foreach (Transform point in transform)
        {
            wayPoints.Add(point.position);
        }
    }

    public int GetNearestWayPointId(Vector3 pos)
    {
        float minDistance = Vector3.Distance(pos, wayPoints[0]);
        int nearestWpId = 0;
        int i = 0;
        foreach (var wayPoint in wayPoints)
        {
            var distance = Vector3.Distance(pos, wayPoint);
            if ( distance < minDistance)
            {
                nearestWpId = i;
                minDistance = distance;
            }

            i++;
        }
        Debug.Log("pos " + pos);
        Debug.Log("nearestWpId " + nearestWpId);
        return nearestWpId;
    }

    [ExecuteAlways]
    [ContextMenu(nameof(RenamePoints))]
    void RenamePoints()
    {
        wayPoints.Clear();
        foreach (Transform point in transform)
        {
            wayPoints.Add(point.position);
            point.gameObject.name = wayPoints.Count + " " + gameObject.name;
        }
    }
}
