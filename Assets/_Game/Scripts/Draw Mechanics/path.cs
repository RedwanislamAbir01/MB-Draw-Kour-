using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LineRenderer))]
public class path : MonoBehaviour
{


    public MonoBehaviour behaviour;
    [HideInInspector]
    public LineRenderer lineRender;
    public Action<IEnumerable<Vector3>> onnewpathcreat = delegate { };

    [HideInInspector]public bool drawLine = false;

  
    public List<Vector3> pionts = new List<Vector3>();
    LineFollower lineFollower = null;
    private void Awake()
    {
        lineRender = GetComponent<LineRenderer>();
        lineRender.enabled = false;
     
    }
    void Start()
    {
        lineFollower = behaviour.GetComponent<LineFollower>();
    }

    // Update is called once per frame
    void Update()
    {
      
      
        if(drawLine)
        {
            if (!lineFollower.endPointReached)
            {
               
            }
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            int layerMask = ~LayerMask.GetMask("Enemy");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.tag == "Player")
                {
                    drawLine = true;
                    lineRender.enabled = true;
                   
                }


                if (Distancetolastpiont(hit.point) > .5f && drawLine)
                {

                    Vector3 adjustedPoint = new Vector3(hit.point.x, 0.025f, hit.point.z); // Set y to 0.025f
                    pionts.Add(adjustedPoint);
                    lineRender.positionCount = pionts.Count;
                    lineRender.SetPositions(pionts.ToArray());
                    LineSmoother.SmoothLine(pionts.ToArray(), .1f);
                }
            }


        }
        else

        {
            onnewpathcreat(pionts);
            if (lineRender.enabled)
              behaviour.enabled = true;
           else
              behaviour.enabled = false;

        }


    }

    public float Distancetolastpiont(Vector3 pioint)
    {

        if (!pionts.Any())
            return Mathf.Infinity;
        return Vector3.Distance(pionts.Last(), pioint);


    }
    public void ResetPath()
    {
        pionts.Clear();
        lineRender.positionCount = 0;
        drawLine = false;
        lineRender.enabled = false;
    }

}
