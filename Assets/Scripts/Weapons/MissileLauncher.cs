﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MissileLauncher : MonoBehaviour
{
    private Transform Target;
    public Transform leftMissileAppearPoint;
    public Transform rightMissileAppearPoint;

    public GameObject Missile;

    public string TargetTag = "Enemy";

    public float cooldown = 2f;

    private bool canFire = false;

    private bool didLeftLast = false; //I know i know don't @ me

    public float additionalFireSpeed = 10.0f;

    public InputActionAsset controls;

    void Start ()
    {
        StartCoroutine(EnableFiring());
    }

    void Awake ()
    {
        controls.actionMaps[0].FindAction("SecondaryFire", true).performed += ctx => Shoot();
    }

    void Shoot()
    {
        if(canFire)
        {
            Target = GetBestTarget();
            LaunchMissile();
            canFire = false;
            StartCoroutine(EnableFiring());
        }
    }

    private IEnumerator EnableFiring ()
    {
        yield return new WaitForSeconds(cooldown);
        canFire = true;
    }

    private Transform GetBestTarget()
    {
        return GetComponent<Targeting>().Target;
    }


    private void LaunchMissile()
    {
        UIManager.instance.HandleRocketLaunch(cooldown);

        GameObject missile;
        if (didLeftLast)
        {
            missile = Instantiate(Missile, rightMissileAppearPoint.position, rightMissileAppearPoint.rotation);
            didLeftLast = false;
        }
        else
        {
            missile = Instantiate(Missile, leftMissileAppearPoint.position, leftMissileAppearPoint.rotation);
            didLeftLast = true;
        }
        
        MissileController missileController = missile.GetComponent<MissileController>();
        missileController.Target = Target;
        Rigidbody myRb = GetComponent<Rigidbody>();
        if(myRb != null)
        {
            missile.GetComponent<SpaceshipController>().Velocity = myRb.velocity + (transform.forward * additionalFireSpeed);
        }
    }
}
