using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerHealth))]

public class PlayerCont : NetworkBehaviour
{
    [HideInInspector]
    public List<GameObject> AmmoStuck = new List<GameObject>();
    WeaponController weaponController;
    CharacterController cCont;
    Vector3 cameraRot;
    PlayerHealth health;
    [SyncVar(hook = "NameChangedRefresh")]
    public string username;
    public delegate void NameChanged(string name);
    public event NameChanged nameChanged;
    public RunSettings running;
    public float jumpSpeed = 3;
    float moveSpeed;
    public float fallSpeed = 3;
    public float topOfPlayer = 1;
    public float widthOfPlayer = 1;
    float yMovement = 0;
    Vector3 max;
    float fall;
    [HideInInspector]
    public Vector3 outSideForce;
    Vector3 velocity;
    [System.Serializable]
    public struct FallDamage
    {
        public AnimationCurve curve;
        public float minFall;
        public float maxFall;
        public int relitiveDamage;
    }
    [System.Serializable]
    public struct RunSettings
    {
        public float walksSpeed;
        public float sprintSpeed;
        public float sprintRampSpeed;
        public float airRampSpeed;
        public float airMoveSpeed;
    }
    public FallDamage fallDamageStats;
    public string playerName
    {
        get
        {
            return username;
        }
        set
        {
            username = value;
            if (nameChanged != null)
                nameChanged(username);
        }
    }
    void NameChangedRefresh(string name)
    {
        playerName = name;
    }
    // Use this for initialization
    void Start()
    {
        GameObject.FindGameObjectWithTag("LeaderBoard").GetComponent<PlayerLeaderBoard>().NewPlayer(gameObject);
        if (isLocalPlayer)
        {
            CmdSetName(GameObject.FindGameObjectWithTag("Manager").GetComponent<UserNames>().userName);
            cCont = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            cameraRot = Camera.main.transform.eulerAngles;
        }
        health = GetComponent<PlayerHealth>();
        weaponController = GetComponentInChildren<WeaponController>();
        if (!isLocalPlayer)
        {
            GetComponentInChildren<Camera>().depth = -100;
            GetComponentInChildren<Camera>().tag = "Untagged";
            Destroy(GetComponentInChildren<AudioListener>());
        }
    }
    public override void OnStartLocalPlayer()
    {
        GetComponentInChildren<CanvasFacingCamera>().gameObject.SetActive(false);
    }
    [Command]
    void CmdFire(NetworkPosition pos)
    {
        GameObject go = weaponController.Fire(pos, true);
        if (go)
            NetworkServer.Spawn(go);
    }
    [Command]
    void CmdSetName(string name)
    {
        playerName = name;
    }
    void Update()
    {
        if (!isLocalPlayer)
            return;

        CheckSprint();
        Movement();
        CheckFire();
        CalculateFall();
        CameraRotation();
    }
    void Movement()
    {
        var movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movement = transform.TransformDirection(movement) * moveSpeed;
        if (cCont.isGrounded)
        {
            yMovement = Physics.gravity.y * Time.deltaTime;
            velocity = Vector3.zero;
        }
        movement.y = yMovement += Physics.gravity.y * Time.deltaTime * fallSpeed;
        if (Input.GetButtonDown("Jump") && cCont.isGrounded)
        {
            velocity = movement / 2;
            velocity.y = 0;
            movement.y = yMovement = jumpSpeed;
        }
        if (movement.y > 0 && Physics.SphereCast(new Ray(transform.position, transform.up), widthOfPlayer, topOfPlayer))
        {
            movement.y = 0;
            yMovement = 0;
        }
        movement += outSideForce;
        velocity *= .999f;
        movement += velocity;
        outSideForce = Vector3.zero;
        cCont.Move(movement * Time.deltaTime);
    }
    void CalculateFall()
    {
        if (!cCont.isGrounded)
            if (yMovement < 0 && max.magnitude == 0)
                max = transform.position;
            else
                fall = max.y - transform.position.y;
    }
    void CheckFire()
    {
        if (Input.GetButtonDown("Reload") && weaponController.equipWep.GetComponent<WeaponBase>().curBullets < weaponController.equipWep.GetComponent<WeaponBase>().capacity)
            weaponController.equipWep.GetComponent<WeaponBase>().Reload();

        if (Input.GetButton("Fire1"))
        {
            CmdFire(new NetworkPosition(Camera.main.transform.forward, Camera.main.transform.position));
            if (!isServer)
            {
                weaponController.FireNoBullet(true);
            }
        }
    }
    void CameraRotation()
    {
        Vector3 thisMove;
        if (cameraRot.x > 80)
            thisMove = new Vector3((-Input.GetAxis("Mouse Y") < 0) ? -Input.GetAxis("Mouse Y") : 0, Input.GetAxis("Mouse X")) * 4;
        else if (cameraRot.x < -80)
            thisMove = new Vector3((-Input.GetAxis("Mouse Y") > 0) ? -Input.GetAxis("Mouse Y") : 0, Input.GetAxis("Mouse X")) * 4;
        else
            thisMove = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * 4;
        cameraRot += thisMove;
        if (!isLocalPlayer)
            return;
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        Camera.main.transform.eulerAngles = cameraRot;

    }
    void CheckSprint()
    {
        if (cCont.isGrounded)
            if (Input.GetButton("Fire3"))
            {
                float temp = Mathf.Lerp(moveSpeed, running.sprintSpeed, running.sprintRampSpeed);
                moveSpeed = temp;
            }
            else
            {
                float temp = Mathf.Lerp(moveSpeed, running.walksSpeed, running.sprintRampSpeed);
                moveSpeed = temp;
            }
        else
        {
            float temp = Mathf.Lerp(moveSpeed, running.airMoveSpeed, running.airRampSpeed);
            moveSpeed = temp;
        }
    }
    [Command]
    void CmdFallDamage(int damage)
    {
        health.health -= damage;
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        max = Vector3.zero;
        if (isLocalPlayer)
            if (fall > fallDamageStats.minFall)
            {
                float damagePercent = fallDamageStats.curve.Evaluate((fall / fallDamageStats.maxFall));
                CmdFallDamage((int)Mathf.Ceil(fallDamageStats.relitiveDamage * damagePercent));
                fall = 0;
            }
    }

}
