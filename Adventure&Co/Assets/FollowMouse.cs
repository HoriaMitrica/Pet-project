using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Debug.Log(Input.mousePosition);
        Debug.Log(worldPosition);
        transform.position = worldPosition;
                   //-6.20     -5.75     -5.35     -4.90     -4.45
        
              
              
              
              
        //0.35  

        
        //-0.05 
        
        
        //-4.45 BOTTOM RIGHT
        //-1.80
    }
}
