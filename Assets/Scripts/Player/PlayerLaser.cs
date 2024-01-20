using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    [SerializeField] float longitudLaser;
    LineRenderer lineRenderer;
    [SerializeField] Transform transPadre;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer=GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 endpos = transform.position + (transform.right * longitudLaser);

        if (transPadre.localScale.x < 0)
        {
            endpos = transform.position - transform.right * longitudLaser;
        }

        lineRenderer.SetPositions(new Vector3[] { transform.position, endpos });
    }
}
