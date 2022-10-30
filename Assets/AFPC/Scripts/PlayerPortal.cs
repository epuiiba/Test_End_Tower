using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPortal
{
    private LayerMask mask;
    PortalScript[] Portal;
    public PortalScript Portal1;
    public PortalScript Portal2;


    public void Initialize()
    {
        Portal = new PortalScript[2];
        Debug.Log("InitShoot");
        Portal[0] = Portal1;
        Portal[1] = Portal2;
        mask = LayerMask.GetMask("Wall"); 
    }
    void Update()
    {
        
    }
    public void shootPortal(int p)
    {
        
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, 1 << 9))
        {
            //Debug.Log("Hit " + p);
            //Debug.Log(Portal[p].transform.forward + "  PF");
            if(hit.normal.z == - Portal[p].transform.forward.z && hit.normal.z != 0f) 
            {
                Portal[p].transform.Rotate(0f,0f,180f);
                //Debug.Log("True");
            }
                
            Portal[p].transform.rotation = Quaternion.FromToRotation(Portal[p].transform.forward, hit.normal) * Portal[p].transform.rotation;
            //Debug.Log(hit.normal + "  HN");
            if(hit.normal.y <= 0.001f)
            {
               Portal[p].transform.localEulerAngles = new Vector3(0f, Portal[p].transform.localEulerAngles.y, 0f);
               
            }
            //Debug.Log("TrueH " + hit.normal.y); 
            Portal[p].transform.position = hit.point + Portal[p].transform.forward * 0.001f;
            Portal[p].terrainBehind = hit.collider.GetComponent<MeshCollider>();
            
        }
    }

}
