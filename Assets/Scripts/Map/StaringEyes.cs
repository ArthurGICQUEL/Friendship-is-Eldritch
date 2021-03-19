using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaringEyes : MonoBehaviour
{
    [SerializeField] float eyeRadius = 0; 
    GameObject pupil = null;

    private void Awake() {
        pupil = transform.GetChild(0).gameObject;
    }

    private void FixedUpdate()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        Vector3 relative = mouseWorldPosition - transform.position;

        if (relative.magnitude < eyeRadius) pupil.transform.position = mouseWorldPosition;

        else pupil.transform.localPosition = relative.normalized * eyeRadius;
    }

}
