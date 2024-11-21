using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoLauncher : MonoBehaviour
{
public Transform launchPoint;
    public GameObject projectile;
    public float launchSpeed = 10f;
 
    public LineRenderer lineRenderer;
    public int linePoints = 175;
    public float timeIntervalInPoints = 0.01f;
 
    void Update()
    {
        if(lineRenderer != null)
        {
            if(Input.GetMouseButton(1))
            {
                lineRenderer.enabled = true;
            }
            else
                lineRenderer.enabled = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            var _projectile = Instantiate(projectile, launchPoint.position, launchPoint.rotation);
            _projectile.GetComponent<Rigidbody>().velocity = launchSpeed * launchPoint.up;
        }
    }
 
}
