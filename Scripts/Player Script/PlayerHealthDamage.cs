using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthDamage : MonoBehaviour {

    [SerializeField]
    private Transform playerBullet;

    private float distanceBeforeNewPlatforms = 120f;
    private LevelGenerator levelGenerator;

    private LevelGeneratorpooling levelgeneratorpool;

    [HideInInspector]
    public bool canShoot;

    
    private Button shootBtn;
    





	// Use this for initialization
	void Awake () {
        levelGenerator = GameObject.Find(Tags.LEVEL_GENERATOR).GetComponent<LevelGenerator>();
        levelgeneratorpool = GameObject.Find(Tags.LEVEL_GENERATOR).GetComponent<LevelGeneratorpooling>();
        shootBtn = GameObject.Find(Tags.SHOOT_BTN_OBJ).GetComponent<Button>();
        shootBtn.onClick.AddListener(()  => Shoot());
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Fire();
	}

    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
           if (canShoot)
            {
                Vector3 bulletPos = transform.position;
                bulletPos.x += 1f;
                bulletPos.y = 1.5f;
                Transform newBullet = (Transform)Instantiate(playerBullet, bulletPos, Quaternion.identity);
                newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1500f);
                newBullet.parent = transform;
            }
            
        }
    }

    public void Shoot()
    {
        if (canShoot)
        {
            Vector3 bulletPos = transform.position;
            bulletPos.x += 1f;
            bulletPos.y += 1.5f;
            Transform newBullet = (Transform)Instantiate(playerBullet, bulletPos, Quaternion.identity);
            newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1500f);
            newBullet.parent = transform;
        }   
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tags.MONSTER_BULLET_TAG || target.tag == Tags.BOUNDS_TAG )
        {
            //INFORM GAME PLAYER DIED
            GameplayController.instance.TakeDamage();
            Destroy(gameObject);
        }
        if (target.tag == Tags.HEALTH_TAG)
        {
            //INFORM GAME PLAY HEALTH ++
            GameplayController.instance.IncrementHealth();
            target.gameObject.SetActive(false);
        }
        if (target.tag == (Tags.MORE_PLATFORMS))
        {
            Vector3 temp = target.transform.position;
            temp.x += distanceBeforeNewPlatforms;
            target.transform.position = temp;
            levelgeneratorpool.PoolingPlatforms();
            GameplayController.instance.IncrementLevel();
        }
    }

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag == Tags.MONSTER_TAG)
        {
            //INFORM GAME PLAYER DIED
            GameplayController.instance.TakeDamage();
            Destroy(gameObject);
        }
    }

}//class
