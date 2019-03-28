﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody rigidBodyProjectile;
    protected BoundsCheck _bounds;


    protected WeaponType _type;
    public WeaponType type
    {
        get {return _type; }
        set { SetType(value); } //auto-property could be used but this has been done as more functionality will be added later
    }

    //to properly destroy projectile - needs to be done for missil (go up to parent and destroy that so everything dies)
    public static void DestroyProjectile(Projectile p)
    {

    }

    // Start is called before the first frame update
    protected void Awake()
    {
        rigidBodyProjectile = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        _bounds = GetComponent<BoundsCheck>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //destroy game object if it is not on screen
        if (!_bounds.onScreen)
        {
            Destroy(gameObject); 
        }
    }

    //changes the weapon type
    public void SetType(WeaponType eWeapon)
    {
        _type = eWeapon;
    }
}
