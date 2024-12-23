using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Drop : MonoBehaviour
{
    UIManager manager;
    void Start()
    {
        manager = GameObject.Find("_UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collected()
    {
       GetComponent<AudioSource>().Play();
       GameObject goldAmount = GameObject.Find("GoldAmount");
       
        
        StartCoroutine(MoveToPos(goldAmount));
    }

    IEnumerator MoveToPos(GameObject go)
    {
        Vector3 startPos = transform.position;
        float time = 0;
        float maxTime = 0.33f;
        Camera camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        Vector3 pos= camera.ScreenToWorldPoint(go.transform.position);
        while ((transform.position - pos).magnitude>0.01f)
        {
            pos = camera.ScreenToWorldPoint(go.transform.position);
            //Debug.Log(pos);
            time += Time.deltaTime;
            transform.position=Vector3.Lerp(startPos, pos, time/maxTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        manager.IncreaseGoldAmount();
        Destroy(this);
    }
}
