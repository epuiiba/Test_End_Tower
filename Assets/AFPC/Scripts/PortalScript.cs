using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AFPC;

public class PortalScript : MonoBehaviour
{
    public Transform OtherPortal;
    public Camera playerCam;
    public Camera portalCam;
    public Camera OtherPortalCam;
    public GameObject player;
    
    public MeshCollider terrainBehind;
    public CapsuleCollider playerColider;

    private bool isOpen;
    Rigidbody playerRbody;
    // Start is called before the first frame update
    void Awake()
    {
        
    }
    void Start()
    {
        isOpen = false;
        playerRbody = player.GetComponent<Rigidbody>();
        

    }

    // Update is called once per frame
    void Update()
    {

        // Quaternion.Inverse(transform.rotation) *
        Quaternion direccion =  Quaternion.Inverse(transform.rotation) * Camera.main.transform.rotation;
            //Debug.Log (direccion.eulerAngles.x +"  "+ direccion.eulerAngles.y + 180+"  "+ direccion.eulerAngles.z); direccion.eulerAngles.x
        OtherPortalCam.transform.localEulerAngles = new Vector3(direccion.eulerAngles.x, direccion.eulerAngles.y + 180, direccion.eulerAngles.z);
        //OtherPortalCam.transform.LookAt(OtherPortal.position);
        
        Vector3 distancia = transform.InverseTransformPoint(Camera.main.transform.position);  //+ new Vector3(0f,0.6f,-0.5f)
        OtherPortalCam.transform.localPosition = - new Vector3(distancia.x, -distancia.y, distancia.z);
        setNearClipPlane();
        distancePlayer();
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log ("Portal Triggering Mal");
        if (other.tag == "Player")
        {
           // Debug.Log ("Portal Triggering");
            Vector3 PlayerFromPortal = transform.InverseTransformPoint(player.transform.position);
            

            if(PlayerFromPortal.z <= 0.015)
            {
                Vector3 vel = -playerRbody.velocity;
                player.transform.position = OtherPortal.position + new Vector3(-PlayerFromPortal.x,
                                                                                +PlayerFromPortal.y,
                                                                                -PlayerFromPortal.z);
                //Debug.Log ("Portal Triggering Be");
                //Quaternion ttt = Quaternion.Inverse(transform.rotation) * player.transform.rotation;

                
                player.transform.eulerAngles = Vector3.up * (OtherPortal.eulerAngles.y - (transform.eulerAngles.y - player.transform.eulerAngles.y) + 180);
                //Vector3 CamLEA = playerCam.transform.localEulerAngles;
                playerCam.transform.localEulerAngles = Vector3.right * (OtherPortal.eulerAngles.x + Camera.main.transform.localEulerAngles.x);
                
                Vector3 velocidadLocalPlayer = -transform.InverseTransformPoint(playerRbody.velocity);
                //playerRbody.velocity = (OtherPortal.transform.position - OtherPortalCam.transform.position).normalized * vel;
                Quaternion ftoR = Quaternion.FromToRotation(transform.forward, OtherPortal.transform.forward);
                if(ftoR == new Quaternion(1f,0f,0f,0f)) ftoR = new Quaternion(0f,1f,0f,0f);
                playerRbody.velocity = ftoR * vel;
                //Debug.Log (ftoR);
                //playerRbody.velocity = - OtherPortal.transform.forward * playerRbody.velocity.y * 2;
            }
        }
    }

    void distancePlayer()
    {
        float distVec = transform.InverseTransformPoint(player.transform.position).magnitude;
        //Debug.Log (distVec);
        if(distVec < 0.9f) 
        {
            isOpen = true;
            Physics.IgnoreCollision(playerColider, terrainBehind, true);
        }
        else if(isOpen) 
        {
            isOpen = false;
            Physics.IgnoreCollision(playerColider, terrainBehind, false);
        }
    }
    void setNearClipPlane()
    {
        Transform clipPlane = transform;
        int dot = System.Math.Sign(Vector3.Dot(clipPlane.forward, transform.position - portalCam.transform.position));
        Vector3 camSpacePos = portalCam.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
        Vector3 camSpaceNormal = portalCam.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * dot;
        float camSpaceDst = -Vector3.Dot (camSpacePos, camSpaceNormal);
        Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

        portalCam.projectionMatrix = playerCam.CalculateObliqueMatrix(clipPlaneCameraSpace);
    }
}
