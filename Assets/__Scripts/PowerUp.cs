﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(1f, 1.25f);
    public float lifeTime = 1f;
    public float fadeTime = 2.5f;
    
    

    [Header("Set Dynamically")]
    public WeaponType type;
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;
    public float birthTime;
    public float duration;
    public float direction;

    private Rigidbody _rigidbody;
    private BoundsCheck _boundsCheck;
    private Renderer _cubeRend;
    

    void Awake()
    {
        cube = transform.Find("Cube").gameObject;
        letter = GetComponent<TextMesh>();
        _rigidbody = GetComponent<Rigidbody>();
        _boundsCheck = GetComponent<BoundsCheck>();
        _cubeRend = cube.GetComponent<Renderer>();
        

        Vector3 vel = Random.onUnitSphere;
        direction = Random.Range(0, 1);
        if(direction <= 0.5)
        {
            direction = -1.5f;
        }
        else
        {
            direction = 1.5f;
        }

        vel.z = 0;
        vel.Normalize();
        vel.x = direction*Random.Range(driftMinMax.x, driftMinMax.y);
        vel.y = -17.5f*Random.Range(driftMinMax.x, driftMinMax.y);
        _rigidbody.velocity = vel;

        transform.rotation = Quaternion.identity;

        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y));
        birthTime = Time.time;

    }

    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;

        //destroys the powerup if it is on screen too long
        if(u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        // deals with fading
        if(u > 0)
        {
            Color c = _cubeRend.material.color;
            c.a = 1f - u;
            _cubeRend.material.color = c;

            c = letter.color;
            c.a = 1f - (u * 0.75f);
            letter.color = c;
        }

        // destroys the powerup if it goes off screen
        if (!_boundsCheck.onScreen)
        {
            Destroy(gameObject);
        }
        
    }

    //sets the powerup type depending on what WeaponDefinition / WeaponType is associated with it.
    public void SetType(WeaponType wt)
    {
        WeaponDefinition def = Main_MainScene.GetWeaponDefinition(wt);
        _cubeRend.material.color = def.color;
        letter.text = def.letter;
        duration = def.duration;
        type = wt;
    }

    //destroys the powerup when appropriate
    public void AbsorbedBy(GameObject target)
    {
        Destroy(this.gameObject);
    }
}
