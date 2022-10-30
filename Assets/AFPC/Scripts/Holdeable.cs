using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdeable : Interactable
{
    // Start is called before the first frame update
    [SerializeField] Transform holdArea;
    bool isHolded;
    Rigidbody rb;

    private float pickUpForce = 150.0f;

    void Start()
    {
        holdArea = GameObject.Find("holdObj").transform;
        isHolded = false;
        rb = GetComponent<Rigidbody>();
    }

    public override void Interact()
    {
        if(isHolded) stopHolding();
        else startHolding();
    }
    public void stopHolding()
    {
        isHolded = false;
        rb.useGravity = true;
        rb.drag = 1;
        rb.constraints = RigidbodyConstraints.None;

        transform.parent = null;
    }
    public void startHolding()
    {
        isHolded = true;
        rb.useGravity = false;
        rb.drag = 10;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        transform.parent = holdArea;

    }
    void Update()
    {
        if(isHolded) holdItem();
    }

    void holdItem()
    {
        if(Vector3.Distance(transform.position, holdArea.position) > 0.1f)
        {
            Vector3 mDic = (holdArea.position - transform.position);
            rb.AddForce(mDic * pickUpForce);
        }
    }
}
