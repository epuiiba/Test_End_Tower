using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour {

    public float jumpForce = 8;

    public bool lockCursor;
    public float mouseSensitivity = 10;

    public PlayerPortal playerPortal;
    public PlayerInteract playerInteract;


    Camera cam;
    
    public float velocidad;
    private int multVel;
    Rigidbody Rbody;
    float rotX;
    Vector3 moveDirection;

    void Start () {
        playerPortal.Initialize();
        cam = Camera.main;
        if (lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Rbody = GetComponent<Rigidbody>();
        
    }

    void Update () {
        readInput();
        
        //transform.Translate(new Vector3( Input.GetAxis("Horizontal") * Time.deltaTime * multVel * velocidad, 0.0f, Input.GetAxis("Vertical") * Time.deltaTime * multVel * velocidad) );
        
        moveDirection = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        if(moveDirection.x == 0f) Rbody.velocity = new Vector3(0f, Rbody.velocity.y, Rbody.velocity.z);
        if(moveDirection.z == 0f) Rbody.velocity = new Vector3(Rbody.velocity.x, Rbody.velocity.y, 0f);
        float  h = mouseSensitivity * Input.GetAxis("Mouse X") * Time.fixedDeltaTime;
        float  v = mouseSensitivity * Input.GetAxis("Mouse Y") * Time.fixedDeltaTime;
        transform.Rotate(0,h,0);
        
        rotX = (cam.transform.eulerAngles.x > 269) ? cam.transform.eulerAngles.x - 360 : cam.transform.eulerAngles.x;
        if(rotX - v < 90f && rotX - v > -90f)
        {   
            cam.transform.Rotate(-v,0,0);
        }
    }
    void FixedUpdate()
    {
        movePlayer();
    }


    void readInput()
    {
        if(Input.GetKey(KeyCode.LeftShift)) {multVel = 2;} else {multVel = 1;}
        if(Input.GetMouseButtonDown(0)) playerPortal.shootPortal(0);
        if(Input.GetMouseButtonDown(1)) playerPortal.shootPortal(1);
        if(Input.GetKeyDown(KeyCode.E)) playerInteract.tryToInteract();
    }
    void movePlayer()
    {
        //Debug.Log (moveDirection.normalized, Rbody);
        Rbody.AddForce(moveDirection.normalized * velocidad * multVel, ForceMode.VelocityChange );
        
    }

    
    

}