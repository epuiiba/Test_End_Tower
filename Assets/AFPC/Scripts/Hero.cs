using UnityEngine;
using AFPC;

/// <summary>
/// Example of setup AFPC with Lifecycle, Movement and Overview classes.
/// </summary>
public class Hero : MonoBehaviour {

    /* UI Reference */
    public HUD HUD;

    /* Lifecycle class. Damage, Heal, Death, Respawn... */
    public Lifecycle lifecycle;
    public PlayerPortal playerPortal; 
    /* Movement class. Move, Jump, Run... */
    public Movement movement;
    public Camera cam;
    public float horCamSpeed;
    public float verCamSpeed;
    /* Overview class. Look, Aim, Shake... */
    //public Overview overview;

    /* Optional assign the HUD */
    private void Awake () {
        if (HUD) {
            HUD.hero = this;
        }
    }

    
    /* Some classes need to initizlize */
    private void Start () {

        /* a few apllication settings for more smooth. This is Optional. */
        QualitySettings.vSyncCount = 0;
        Cursor.lockState = CursorLockMode.Locked;

        /* Initialize lifecycle and add Damage FX */
        lifecycle.Initialize();
        lifecycle.AssignDamageAction (DamageFX);

        /* Initialize movement and add camera shake when landing */
        movement.Initialize();
        //movement.AssignLandingAction (()=> overview.Shake(0.5f));
    }

    private void Update () {

        /* Read player input before check availability */
        ReadInput();

        /* Block controller when unavailable */
        if (!lifecycle.Availability()) return;

        /* Mouse look state */
        //overview.Looking();

        /* Change camera FOV state */
        //overview.Aiming();

        /* Shake camera state. Required "physical camera" mode on */
        //overview.Shaking();

        /* Control the speed */
        movement.Running();

        /* Control the jumping, ground search... */
        movement.Jumping();

        /* Control the health and shield recovery */
        lifecycle.Runtime();
    }

    private void FixedUpdate () {

        /* Block controller when unavailable */
        if (!lifecycle.Availability()) return;

        /* Physical movement */
        movement.Accelerate();

        /* Physical rotation with camera */
        //overview.RotateRigigbodyToLookDirection (movement.rb);
        /*float h = horCamSpeed * Input.GetAxis("Mouse X") * Time.fixedDeltaTime;
        float v = verCamSpeed * Input.GetAxis("Mouse Y") * Time.fixedDeltaTime;

        transform.Rotate(0,h,0);
        cam.transform.Rotate(-v,0,0);*/
    }

    private void LateUpdate () {

        /* Block controller when unavailable */
        if (!lifecycle.Availability()) return;

        /* Camera following */
        //overview.Follow (transform.position);
    }

    private void ReadInput () {
        if (Input.GetKeyDown (KeyCode.R)) lifecycle.Damage(50);
        if (Input.GetKeyDown (KeyCode.H)) lifecycle.Heal(50);
        if (Input.GetKeyDown (KeyCode.T)) lifecycle.Respawn();
        if (Input.GetMouseButtonDown(0)) playerPortal.shootPortal(0);
        if (Input.GetMouseButtonDown(1)) playerPortal.shootPortal(1);
        /*overview.lookingInputValues.x = Input.GetAxis("Mouse X");
        overview.lookingInputValues.y = Input.GetAxis("Mouse Y");
        overview.aimingInputValue = Input.GetKey(KeyCode.LeftControl);*/
        movement.movementInputValues.x = Input.GetAxis("Horizontal");
        movement.movementInputValues.y = Input.GetAxis("Vertical");
        movement.jumpingInputValue = Input.GetButtonDown("Jump");
        movement.runningInputValue = Input.GetKey(KeyCode.LeftShift);
    }

    private void DamageFX () {
        if (HUD) HUD.DamageFX();
        //overview.Shake(0.75f);
    }
}
