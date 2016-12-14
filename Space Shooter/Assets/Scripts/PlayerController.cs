using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin;
    public float xMax;
    public float zMin;
    public float zMax;
}

public class PlayerController : MonoBehaviour {

    public float speed;
    public Boundary boundary;
    public float tilt;
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public SimpleTouchPad touchPad;
    public SimpleTouchAreaButton areaButton;

    private Rigidbody rb;
    private float myTime = 0.0f;
    private float nextFire = 0.5f;
    private AudioSource audioSource;
    private Quaternion calibrationQuaternion;




    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        CalibrateAccelerometer();

    }

    void Update()
    {

        

        myTime = myTime + Time.deltaTime;

#if UNITY_IOS
        if (areaButton.CanFire() && myTime > nextFire)
#else
        if (Input.GetButton("Fire1") && myTime > nextFire)
#endif
        
        {
            nextFire = myTime + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);


            audioSource.Play();                 

            nextFire = nextFire - myTime;
            myTime = 0.0F;
        }
    }

    void FixedUpdate()
    {

#if UNITY_IOS
        Vector2 direction = touchPad.GetDirection();
        Vector3 movement = new Vector3(direction.x, 0.0f, direction.y);
#else
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
#endif


        
        
        rb.velocity = movement * speed;

        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    void CalibrateAccelerometer ()
    {
        Vector3 accelerationSnapshot = Input.acceleration;
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
        calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);
    }

    Vector3 FixAcceleration (Vector3 acceleration)
    {
        Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
        return fixedAcceleration;
    }


}
