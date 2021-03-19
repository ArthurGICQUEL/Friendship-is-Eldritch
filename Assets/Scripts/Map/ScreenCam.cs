using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCam : MonoBehaviour
{   //[SerializeField]
    //public Camera camera;
    [SerializeField] BoxCollider2D referenceBounds = null;
    [SerializeField] float moveSpeed, zoomSpeed;
    [SerializeField] float closeSize, farSize;
    private float minX, maxX, minY, maxY;
    bool movementIsEnabled = true;
    Camera _cam;

    private void Awake() {
        _cam = GetComponent<Camera>();

        if (referenceBounds == null) {
            minX = -72.6f;
            maxX = 18.4f;
            minY = -3.3f;
            maxY = 0f;
            return;
        }
        Vector3 boundsSize = referenceBounds.size;
        Vector3 boundsOffset = (Vector3)referenceBounds.offset + referenceBounds.transform.position;
        minX = -boundsSize.x * 0.5f + boundsOffset.x;
        maxX = boundsSize.x * 0.5f + boundsOffset.x;
        minY = -boundsSize.y * 0.5f + boundsOffset.y;
        maxY = boundsSize.y * 0.5f + boundsOffset.y;
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A)) {
            movementIsEnabled = !movementIsEnabled;
        }
        if (movementIsEnabled) {
            TranslateCam();
        }
        ZoomClamped();

        // clamp cam pos
        Vector2 camHalfSize = new Vector2(_cam.orthographicSize * _cam.aspect, _cam.orthographicSize);
        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(transform.position.x, minX + camHalfSize.x, maxX - camHalfSize.x);
        clampedPos.y = Mathf.Clamp(transform.position.y, minY + camHalfSize.y, maxY - camHalfSize.y);
        transform.position = clampedPos;
    }

    void ZoomClamped() {
        _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize - Input.mouseScrollDelta.y * zoomSpeed, closeSize, farSize);
    }

    void TranslateCam() {
        Vector3 translation = Vector3.zero;
        if(Input.mousePosition.x >= (Screen.width - 1f)) //verify if mouseX >= to the screen width
        {
            translation = new Vector3(2, 0, 0); //Move the cam right 2 by 2
        } else if(Input.mousePosition.x <= 1f) //verify if mouseX <= to 0 which is the min of screen width
          {
            translation = new Vector3(-2, 0, 0); //Move the cam left 2 by 2
        }
        if(Input.mousePosition.y >= (Screen.height - 1f)) //verify if mouseY >= to the screen width
        {
            translation = new Vector3(0, 2, 0); //Move the cam up 2 by 2
        } else if(Input.mousePosition.y <= 1f) //verify if mouseY <= to 0 which is the min of screen height
          {
            translation = new Vector3(0, -2, 0); //Move the cam down 2 by 2
        }
        if(translation != Vector3.zero) {
            transform.Translate(translation * Time.deltaTime * moveSpeed);

        }
    }
    //Scren Bounds X:18.4; -72.6 Y:0;-3.3
}
