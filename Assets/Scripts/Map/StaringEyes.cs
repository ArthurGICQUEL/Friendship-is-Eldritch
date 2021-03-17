using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaringEyes : MonoBehaviour
{
    [SerializeField] GameObject pupil;
    [SerializeField] float eyeRadius;

    private void FixedUpdate()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0, 0, -10);
        Vector3 relative = mouseWorldPosition - transform.position;

        if (relative.magnitude < eyeRadius) pupil.transform.position = mouseWorldPosition;

        else pupil.transform.localPosition = relative.normalized * eyeRadius;
    }

}
