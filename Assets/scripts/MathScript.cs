using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathScript : MonoBehaviour {


    public int numberOfSides;
    public float polygonRadius;
    public Vector2 polygonCenter;

    Vector3 startPos;

    public float drawTimer;
    float drawTick;
    public float ex;
    public float ey;

   
    private void Start()
    {
        startPos = transform.position;
        ex = startPos.x;
        ey = startPos.y;

        for (int i =0; i < 20; i++)
        {
            var theta = 2 * Mathf.PI * i / 20;
            Vector2 tempV = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * 3;
            
            //Debug.DrawLine(new Vector2(0, 0), tempV, Color.red, 15);
        }


    }
    // Update is called once per frame
    void Update () {
        /*
        ex += 2.3f * Time.deltaTime;
        ey = 3 * Mathf.Sin((Time.time - 2) / 3);
        transform.position = new Vector3(ex, ey, 0);

         */
        if (drawTick < drawTimer)
        {
            drawTick += Time.deltaTime;
        }else
        {
            for (int i = 0; i < 20; i++)
            {
               
                var theta = 2 * Mathf.PI * i / 20;
                Vector3 tempV = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0) * 3;
                tempV += transform.position;
               
                Debug.DrawLine(startPos, tempV, Color.red, 15);
            }

            drawTick = 0;
        }
   




	}

    void DebugDrawPolygon(Vector2 center, float radius, int numSides)
    {
        // The corner that is used to start the polygon (parallel to the X axis).
        Vector2 startCorner = new Vector2(radius, 0) + polygonCenter;

        // The "previous" corner point, initialised to the starting corner.
        Vector2 previousCorner = startCorner;

        // For each corner after the starting corner...
        for (int i = 1; i < numSides; i++)
        {
            // Calculate the angle of the corner in radians.
            float cornerAngle = 2f * Mathf.PI / (float)numSides * i;

            // Get the X and Y coordinates of the corner point.
            Vector2 currentCorner = new Vector2(Mathf.Cos(cornerAngle) * radius, Mathf.Sin(cornerAngle) * radius) + polygonCenter;

            // Draw a side of the polygon by connecting the current corner to the previous one.
            Debug.DrawLine(currentCorner, previousCorner);

            // Having used the current corner, it now becomes the previous corner.
            previousCorner = currentCorner;
        }

        // Draw the final side by connecting the last corner to the starting corner.
        Debug.DrawLine(startCorner, previousCorner);
    }
}
