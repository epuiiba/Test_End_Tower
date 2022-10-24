using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInteract
{   
    public float distanceInteract = 3f;
    private LayerMask mask;
    void Start()
    {
        mask = LayerMask.GetMask("Interactable"); 
    }
    // Start is called before the first frame update
    public void tryToInteract()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distanceInteract, 1 << 11))
        {
            Interactable inter = hit.collider.GetComponent<Interactable>();
            if(inter != null)
            {
                inter.Interact();
            }
        }
    }

    void Update()
    {
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.white, 3f);
    }
}
