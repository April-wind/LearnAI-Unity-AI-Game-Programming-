using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//学习自 https://zhuanlan.zhihu.com/p/28903099
[HelpURL("https://zhuanlan.zhihu.com/p/28903099")]
public class Crow : MonoBehaviour
{
    /*运动相关*/
    [Header("运动速度")]
    public float speed = 1.0f;
    [Header("运动目标")]
    [SerializeField]
    private Transform target;
    
    /*动画相关*/
    [Header("相邻两次动画播放时间间隔的下限")]
    public float animRandomTimeStart = 1.0f;
    [Header("相邻两次动画播放时间间隔的上限")]
    public float animRandomTimeEnd = 2.0f;
    
    private Animation anim;
    [SerializeField]
    [Header("判断是否开启协程")]
    private bool canStart = true;

    void Start()
    {
        anim = GetComponentInChildren<Animation>();
        target = GameObject.Find("Target").transform;
    }
    private IEnumerator Anim ()
    {
        canStart = false;
        //每隔0到2秒之间的随机数等待
        yield return new WaitForSeconds (Random.Range (animRandomTimeStart, animRandomTimeEnd));
        canStart = true;
        //Debug.Log("1");
        anim.Play();
    }
	
    void Update () {
        if (canStart == true)
        {
            StartCoroutine(Anim());
        }
        
        transform.LookAt(target.position);
        transform.Translate (Vector3.forward * Time.deltaTime * speed, Space.Self);
    }
}
