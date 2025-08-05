using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CameraContoller : MonoBehaviour
{
    public Transform target;
    public Transform farBackground, middleBackground;
    
    //varijabla za max visinu kamere
    public float minHeight, maxHeight;

    //private float lastXPos;
    private Vector2 lastPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //lastXPos = transform.position.x;
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /* transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

         float clampedY = Mathf.Clamp(transform.position.y, minHeight, maxHeight);
         transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);       */

        //new organized line, ogranicavanje min,max razdaljine kamere (target za pomeranje, transform za zadrzavanje)
        transform.position = new Vector3(target.position.x, Mathf.Clamp(target.position.y, minHeight, maxHeight), transform.position.z);

        //float amountToMoveX = transform.position.x - lastXPos;
        Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);

        farBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f);
        middleBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f) * .5f;

        //lastXPos = transform.position.x;
        lastPos = transform.position;
    }
}
