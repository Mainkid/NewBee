using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject[] UpBounce;
    public GameObject[] UpFuel;
    public Sprite[] UpDamage;
    public GameObject[] UpExplosion;
    public GameObject[] UpEngine;

    public bool IsReadingInput;
    public bool IsInputBlocked = false;


    public Transform LeftWheel;
    public Transform RightWheel;

    public AudioClip[] HitSounds;
    public Sprite[] HitSprites;

    private Rigidbody2D _rb;
    public ParticleSystem _particleSystem;
    private Rigidbody2D _rightWheelRb;
    private GameObject _drop;
    private GameObject _explosion;
    private AudioSource _audioSource;
    public UIManager _uiManager;
    private bool _isInLevel = false;
    private bool _isReactingToCollisions = true;

    private ParticleSystem _hopperSystem1;
    private ParticleSystem _hopperSystem2;

    private float _engineMultiplier;
    private float _bounceFactor;
    private float _explosiveMultiplier;
    private float _maxFuel;
    private float _fuel;

    public Rigidbody2D[] rbs;

    private const float _delayBetweenSounds = 1.0f;
    [SerializeField]
    private float _currentDelayBetweenSounds = 0.0f;

    private int hitAmount = 0;

    void Start()
    {
        GameObject.Find("Fade").GetComponent<Animator>().Play("FadeOut");
        _rb = GetComponent<Rigidbody2D>();
        InitUpgrades();
        if (!IsReadingInput)
        {
            //Debug.Log("RETURNING");
            return;
        }

        
        _drop = Resources.Load<GameObject>("Prefabs/gold");
        _explosion = Resources.Load<GameObject>("Prefabs/Explosion");
        _audioSource = GetComponent<AudioSource>();
        if (GameObject.Find("HopperSystem1"))
            _hopperSystem1 = GameObject.Find("HopperSystem1").GetComponent<ParticleSystem>();
        if (GameObject.Find("HopperSystem2"))
            _hopperSystem2 = GameObject.Find("HopperSystem2").GetComponent<ParticleSystem>();

        _maxFuel = UpgradeSystem.Instance.GetTopUpgradeValue(UpgradeGroup.Fuel);
        _fuel = _maxFuel;
    }

    void InitUpgrades()
    {
        for (int i=0;i < UpExplosion.Length; i++)
        {
            if (i <= UpgradeSystem.Instance.CountUpgradesOfGroup(UpgradeGroup.Explosive)-1)
                UpExplosion[i].SetActive(true);
            else
                UpExplosion[i].SetActive(false);
        }

        for (int i=0;i<UpBounce.Length;i++)
        {
            if (i <= UpgradeSystem.Instance.CountUpgradesOfGroup(UpgradeGroup.Bounce)-1)
                UpBounce[i].SetActive(true);
            else
                UpBounce[i].SetActive(false);
        }

        if (UpgradeSystem.Instance.GetTopUpgradeValue(UpgradeGroup.Damage) == 0)
        {
            GameObject.Find("SwordPlayer").gameObject.SetActive(false);
        }
        else
        {
            GameObject.Find("SwordPlayer").GetComponent<SpriteRenderer>().sprite = UpDamage[Convert.ToInt32(UpgradeSystem.Instance.GetTopUpgradeValue(UpgradeGroup.Damage)-1)];
        }

        for (int i = 0; i < UpEngine.Length; i++)
        {
            if (i <= UpgradeSystem.Instance.CountUpgradesOfGroup(UpgradeGroup.Engine)-1)
                UpEngine[i].SetActive(true);
            else
            {
                UpEngine[i].SetActive(false);
            }
        }

        PhysicsMaterial2D actorMaterial = new PhysicsMaterial2D();
        actorMaterial.bounciness = _bounceFactor;

        _engineMultiplier = UpgradeSystem.Instance.GetTopUpgradeValue(UpgradeGroup.Engine);
        _bounceFactor = UpgradeSystem.Instance.GetTopUpgradeValue(UpgradeGroup.Bounce);
        _explosiveMultiplier = UpgradeSystem.Instance.GetTopUpgradeValue(UpgradeGroup.Explosive);
        _rb.sharedMaterial = actorMaterial;
        _rb.sharedMaterial.bounciness = _bounceFactor;
        _rb.sharedMaterial.friction = 0.4f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsReadingInput || IsInputBlocked)
        {
            //_rb.velocity = new Vector2(1, 0);
            return;
        }
            

        if (!_isReactingToCollisions)
            return;
        
        
        _currentDelayBetweenSounds += Time.deltaTime;
        
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && _fuel>0)
        {
            _fuel -= Time.deltaTime;
            //Debug.Log(_fuel);
            _rb.AddForce(new Vector2(1,0) *Time.deltaTime*500.0f * _engineMultiplier);
            if (_hopperSystem1)
            { 
                var emission = _hopperSystem1.emission;
                emission.rateOverDistance = 30;
            }
            if (_hopperSystem2)
            {
                var emission = _hopperSystem2.emission;
                emission.rateOverDistance = 30;
            }

        }
        else
        {
            if (_hopperSystem1)
            {
                var emission = _hopperSystem1.emission;
                emission.rateOverDistance = 0;
            }
            if (_hopperSystem2)
            {
                var emission = _hopperSystem2.emission;
                emission.rateOverDistance = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            _uiManager.Pause(true);
        }

        if (Mathf.Abs( gameObject.transform.rotation.eulerAngles.z)>20.0f)
        {
            var emision = _particleSystem.emission;
            emision.rateOverDistance = 0;
        }
        else
        {
            var emision = _particleSystem.emission;
            emision.rateOverDistance = 10;
        }

        _uiManager.SetFuel(_fuel / _maxFuel);   

        if (_isInLevel && _rb.velocity.magnitude < 1.0f && _fuel<0.0f)
        {
            _isInLevel = false;

            SaveSystem.Instance.SaveGame();

            //Debug.Log("GAME OVER");
            StartCoroutine(ShowLevelEnded(false));
            
        }
    }

    IEnumerator ShowLevelEnded(bool isFinished)
    {
        
        yield return new WaitForSeconds(1);
        _isReactingToCollisions = false;
        UIManager.Instance.ShowLevelEnded(isFinished);
    }

    IEnumerator RandomForceApply(Rigidbody2D rb)
    {
        //Debug.Log("Applying Force");
        while (true)
        {
            rb.AddForce(new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1)));
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 1.0f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isReactingToCollisions)
            return;

        if (collision.gameObject.tag=="Environment")
        {
            collision.gameObject.GetComponent<Animator>().Play("EnvironmentDisolve");
            
            SpawnResources(UnityEngine.Random.Range(0, 1), collision.gameObject.transform.position);
        }
        else if (collision.gameObject.tag=="Mob")
        {
            //Debug.Log("LL");
            if (UpgradeSystem.Instance.GetTopUpgradeValue(UpgradeGroup.Damage) > 0)
            {
                collision.gameObject.GetComponent<MobAI>().Hurt(Convert.ToInt32(UpgradeSystem.Instance.GetTopUpgradeValue(UpgradeGroup.Damage)));
                if (collision.gameObject.GetComponent<MobAI>().IsExplosive)
                {
                    GameObject go = Instantiate(_explosion, collision.gameObject.transform.position, Quaternion.identity);
                    _rb.AddForce(new Vector2(1, 1) * 300 * _explosiveMultiplier);
                    go.GetComponent<Animator>().Play("Explosion");
                }
                SpawnResources(UnityEngine.Random.Range(1, 3), collision.gameObject.transform.position);
            }
        }
        else if (collision.gameObject.tag=="Drop")
        {
            collision.gameObject.GetComponent<Drop>().Collected();
            
        }
        else if (collision.gameObject.name == "_LevelVolume")
        {
            _isInLevel = true;
        }
        else if (collision.gameObject.tag == "Finish")
        {
            _isInLevel = false;
            StartCoroutine(ShowLevelEnded(true));
        }
        else if (collision.gameObject.tag == "Lava")
        {
            _isInLevel = false;
            StartCoroutine(ShowLevelEnded(false));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_currentDelayBetweenSounds > _delayBetweenSounds && _rb.velocity.magnitude>5.0f)
        {
            _audioSource.clip = HitSounds[UnityEngine.Random.Range(0, HitSounds.Length - 1)];
            _audioSource.Play();
            _currentDelayBetweenSounds = 0;
            hitAmount++;
            transform.Find("DestroyDecal").GetComponent<SpriteRenderer>().sprite = HitSprites[Mathf.Clamp(hitAmount,0,HitSprites.Length - 1)];
            float randomVal = UnityEngine.Random.Range(0, 10);
            if (randomVal <5 && hitAmount>4)
            {
                if (transform.Find("RightWheel") != null)
                {
                    transform.Find("RightWheel").GetComponent<HingeJoint2D>().enabled = false;
                    transform.Find("RightWheel").transform.parent = null;
                }
                else if (transform.Find("LeftWheel") != null)
                {
                    transform.Find("LeftWheel").GetComponent<HingeJoint2D>().enabled = false;
                    transform.Find("LeftWheel").transform.parent = null;
                }
            }
        }
    }

    void SpawnResources(int amount,Vector3 pos)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(_drop, pos, Quaternion.identity);
        }
    }

    


}
