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

    public float launchInterval = 1f; // Interval between launches in seconds
    private float launchTimer = 0f;

    public AudioClip launchSound; // Assign the sound in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = Input.GetMouseButton(1);
        }

        // Countdown the timer
        launchTimer -= Time.deltaTime;
        if (launchTimer <= 0f)
        {
            LaunchTomato();
            launchTimer = launchInterval; // Reset the timer
        }
    }

    void LaunchTomato()
    {
        // Get the main camera's position
        Transform mainCamera = Camera.main.transform;

        // Calculate the direction from the launchPoint to the main camera
        Vector3 direction = (mainCamera.position - launchPoint.position).normalized;

        // Instantiate and launch the projectile
        var _projectile = Instantiate(projectile, launchPoint.position, Quaternion.LookRotation(direction));
        _projectile.GetComponent<Rigidbody>().velocity = direction * launchSpeed;

        // Play the launch sound
        if (launchSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(launchSound);
        }
    }
}
