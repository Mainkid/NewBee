using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObject : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private GameObject _explosionPrefab;
    public bool ProduceExplosion;
    public AudioClip[] DestroySound;
    void Start()
    {
        _particleSystem = gameObject.transform.GetComponentInChildren<ParticleSystem>();
        _explosionPrefab = Resources.Load<GameObject>("Prefabs/Explosion");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayParticle()
    {
        GetComponent<AudioSource>().clip = DestroySound[Random.Range(0,DestroySound.Length-1)];
        GetComponent<AudioSource>().Play();

        GetComponent<BoxCollider2D>().enabled = false;
        if (_particleSystem)
            _particleSystem.Play();
        if (ProduceExplosion)
        {
           GameObject go= Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            go.GetComponent<Animator>().Play("Explosion");
            GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1) * 400);
        }
            


    }
}
