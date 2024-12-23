using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAI : MonoBehaviour
{
    public Transform LeftPoint;
    public Transform RightPoint;

    public string AnimationWalkingState;
    public bool IsExplosive;
    public int Health;
    public GameObject[] Lifes;

    private int _direction = 1;
    private float _speed = 1.0f;

    public AudioClip hurtSound;
    public AudioClip deathSound;

    private Transform _root;
    private bool _isHurted = false;

    void Start()
    {
        gameObject.GetComponentInChildren<Animator>().Play(AnimationWalkingState);
        _root = transform.Find("Root").transform;

        for (int i=Health;i< Lifes.Length;i++)
        {
            Lifes[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x<RightPoint.position.x && transform.position.x>LeftPoint.position.x)
        {
            transform.position += new Vector3(1,0,0)*_direction * _speed * Time.deltaTime;
        }
        else if (transform.position.x>RightPoint.position.x)
        {
            transform.position += new Vector3(-1, 0, 0) * _speed * Time.deltaTime;
            _direction = -1;
        }
        else if (transform.position.x<LeftPoint.position.x)
        {
            transform.position += new Vector3(1, 0, 0) * _speed * Time.deltaTime;
            _direction = 1;
        }

        _root.localScale = new Vector3(_direction,1,1);
    }

    public void Death()
    {
        gameObject.GetComponentInChildren<Animator>().SetBool("isDead", true);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<AudioSource>().clip = deathSound;
        GetComponent<AudioSource>().Play();
        gameObject.transform.Find("Lifes").gameObject.SetActive(false);
        Invoke("DestroyMob", 1.5f);
    }

    void DestroyMob()
    {
        Destroy(gameObject);
    }

    public void Hurt(int damage)
    {
        if (_isHurted)
            return;
        _isHurted = true;
        Health-=damage;
        if (Health<=0)
        {
            Death();
            return;
        }
        GameObject Lifes = gameObject.transform.Find("Lifes").gameObject;

        for (int i=Health;i<Lifes.transform.childCount;i++)
        {
            Lifes.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
        GetComponent<AudioSource>().clip = hurtSound;
        GetComponent<AudioSource>().Play();
        Debug.Log("HURTS");
    }
    
}
