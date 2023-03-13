using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnviromentScanner : MonoBehaviour
{
    [SerializeField] Vector3 forwardRayOffset = new Vector3(0, 0.25f, 0);
    [SerializeField] float forwardRayLength = 0.8f;
    [SerializeField] float heightRayLength = 5;
    [SerializeField] float ledgeRayLength = 10;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float ledgeHeightThreshold = 0.75f;

    public ObstacleHitData ObstacleCheck()
    {
        var hitData = new ObstacleHitData();

        var forwardOrigin = transform.position + forwardRayOffset;
        hitData.forwardHitFound = Physics.Raycast(forwardOrigin, transform.forward,
            out hitData.forwardHit, forwardRayLength, obstacleLayer);

        // Debug.DrawRay(forwardOrigin, transform.forward * forwardRayLength,
        //     (hitData.forwardHitFound) ? Color.red : Color.white);

        if (hitData.forwardHitFound == true)
        {
            var heightOrigin = hitData.forwardHit.point + Vector3.up * heightRayLength;
            hitData.heightHitFound = Physics.Raycast(heightOrigin, Vector3.down,
                out hitData.heightHit, heightRayLength, obstacleLayer);

            Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength,
                (hitData.heightHitFound) ? Color.red : Color.white);
        }

        return hitData;
    }

    public bool LedgeCheck(Vector3 moveDir, out LedgeData ledgeData)
    {
        ledgeData = new LedgeData();

        if (moveDir == Vector3.zero)
            return false;

        float originOffset = .5f;
        var origin = transform.position + moveDir * originOffset + Vector3.up;

        if (PhysicsUtil.ThreeRaycasts(origin, Vector3.down, 0.25f, transform,
            out List<RaycastHit> hits, ledgeRayLength, obstacleLayer, true))
        {
            var validHits = hits.Where(hit => transform.position.y - hit.point.y > ledgeHeightThreshold).ToList();

            if (validHits.Count > 0)
            {
                var surfaceRayOrigin = validHits[0].point;
                surfaceRayOrigin.y = transform.position.y  -.1f;

                if (Physics.Raycast(surfaceRayOrigin, transform.position - surfaceRayOrigin, out RaycastHit surfaceHit, 2, obstacleLayer))
                {
                    Debug.DrawLine(surfaceRayOrigin, transform.position, Color.cyan);

                    float heigth = transform.position.y - validHits[0].point.y;

                    ledgeData.angle = Vector3.Angle(transform.forward, surfaceHit.normal);
                    ledgeData.heigth = heigth;
                    ledgeData.surfaceHit = surfaceHit;

                    return true;
                }
            }
        }

        return false;
    }
}


public struct ObstacleHitData
{
    public bool forwardHitFound;
    public bool heightHitFound;
    public RaycastHit forwardHit;
    public RaycastHit heightHit;
}

public struct LedgeData
{
    public float heigth;
    public float angle;
    public RaycastHit surfaceHit;
}
