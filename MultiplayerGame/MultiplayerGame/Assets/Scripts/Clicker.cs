using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            Debug.Log("cliiiick");
            Clicked();
        }
    }

    private void Clicked()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(ray, out hit))
        {
            Debug.Log("ray hit: " + hit.collider.gameObject);
            Debug.Log("ray hit: " + hit.point);

            var clickMove = hit.collider.gameObject.GetComponent<ClickMove>();

            clickMove.OnClick(hit.point);

        }

        
    }
}
