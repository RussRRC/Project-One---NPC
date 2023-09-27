using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
   public LineRenderer lineRenderer;

    public Transform other;

    public void Update() {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, other.position);
    }
}
