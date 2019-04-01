﻿
    public delegate void fireWeapons(); //creates delegate type
    public fireWeapons fireWeaponsDelegate; //creates variable of type fireWeapons
    public fireWeapons stopWeaponsFire; //this is triggered on space bar up - stops flame thrower

    private GameObject _lastTriggerGo = null;
    private float _startTime = 0;
    private Weapon _weapon;

    


    void Awake()
    {
        if (heroScriptReference == null)
	{
            heroScriptReference = this; //sets up singleton so that only 1 hero can be created.
        }
        else
        {
            Debug.LogError("Attempted Creation of Second Hero");
        }
    }

    void Start()
    {
        _startTime = Time.time;
        _weapon = GetComponentInChildren<Weapon>();

    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - _startTime) <= 2.0f)
        {

            float change = 50f / 5f * (2.0f - (Time.time - _startTime)) * Time.deltaTime;
            Vector3 pos = transform.position;
            pos.y += change;
            transform.position = pos;
        }
        else
        {
            //Switch weapon when the key 'c' is clicked
            if (Input.GetKeyDown("c"))
            {
                if (_weapon.type == WeaponType.single) { _weapon.type = WeaponType.triple; }
                else { _weapon.type = WeaponType.single; }
            }
            if (Input.GetKeyDown("x"))
            {
                if (_weapon.type != WeaponType.plasmaThrower) { _weapon.type = WeaponType.plasmaThrower; }
                else { _weapon.type = WeaponType.homing; }
            }
            if (Input.GetKeyDown("z"))
            {
                if (_weapon.type != WeaponType.freezeGun) { _weapon.type = WeaponType.freezeGun; }
                else { _weapon.type = WeaponType.moab; }
            }

            //these method used input based on the user defined axis and return a value between 1- and 1 depending on which direction is push
            //starts at 0 and builds to 1 the longer you hold it
            GetComponent<BoundsCheck>().boundsCheckActive = true;
            float xAxis = Input.GetAxis("Horizontal");
            float yAxis = Input.GetAxis("Vertical");

            //handles moving
            Vector3 pos = transform.position;
            pos.x += xAxis * speed * Time.deltaTime;
            pos.y += yAxis * speed * Time.deltaTime;
            transform.position = pos;

            //handles ship tilt
            transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

            if (Input.GetAxis("Jump") == 1 && fireWeaponsDelegate != null) //fires on space bar - delegate cannot be null
            {
                if (fireWeaponsDelegate != null)
                {
                    fireWeaponsDelegate(); //will fire the weapon
                }
            }
            if (Input.GetKeyUp("space"))
            {
                if (stopWeaponsFire != null)
                {
                    stopWeaponsFire();
                }
            }
        }
    }


    //checks for collision between hero and enemies
    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        print(other.tag);
        if (other.tag == "ProjectileEnemy")
        {
            Destroy(other.gameObject);
            shieldLevel--;
            return;
        }

        if (go == _lastTriggerGo)
        {
            return;
        }
        //sets the last triggered game object to the "other" gameobject
        _lastTriggerGo = go;
        //decreases shield level upon trigger with an enemy
        //destroys enemy
        
        if (go.tag == "Enemy0" || go.tag == "Enemy1" || go.tag == "Enemy2" || go.tag == "Enemy4")
        {
            shieldLevel--;
            Main_MainScene.scriptReference.DestroyEnemy(go); //destroy enemy function used as it removes the enemy from the list in main
        }
        
    }






    //shield level property allows _shieldLevel to be set as private and is only accessed through get and set
    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {
            //sets shield level to minimum of the current value and 4
            _shieldLevel = Mathf.Min(value, 4);
            //destroys ship and restarts game when no shields remain on the hero
            if (value < 0)
            {
                //Saves highscore 
                if(Score.scoreControllerReference.highscore < Score.scoreControllerReference.score)
                {
                    Score.scoreControllerReference.highscore = Score.scoreControllerReference.score;
                    Score.scoreControllerReference.SavePlayerProgress();
                }
                Main_MainScene.scriptReference.spawnEnemies = false; //stops enemy spawning when ship is destroyed
                Main_MainScene.scriptReference.spawnPickUps = false;
                Destroy(this.gameObject); //destroy the ship
                Main_MainScene.scriptReference.DelayedRestart(gameRestartDelay); //restart game
            }
        }
    }
}
