using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowAI : MonoBehaviour
{
    [Header("三种力的合力")]
    public Vector3 sumForce = Vector3.zero;
    [Header("分离的力")]
    public Vector3 separationForce = Vector3.zero;
    [Header("队列的力")]
    public Vector3 alignmentForce = Vector3.zero;
    [Header("聚集的力")]
    public Vector3 cohesionForce = Vector3.zero;

    [Tooltip("距离这个鸟有三米以内的鸟类，跟其他鸟进行分离")]
    [Header("分离阈值")]
    public float separationDistance = 3.0f;
    [Header("队列阈值")]
    public float alignmentDistance = 5.0f;
    [Header("存储附近的鸟")]
    public List<GameObject> separationNeighbors = new List<GameObject>();
    public List<GameObject> alignmentNeighbors = new List<GameObject>();
    [Header("分离的力所占的比重")]
    public float separationWeight = 0.0f;
    [Header("队列的力所占比重")]
    public float alignmentWeight = 0.0f;
    [Header("聚合的力所占比重")]
    public float cohesionWeight = 0.0f;
    [Header("鸟的质量")]
    public float crowM = 1.0f;
    
    public Vector3 velocity = Vector3.forward;
    
    public float checkInteterval = 0.2f;

    public float animRandomTime = 2f;
    private Animation[] anim;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CalForce", 0,checkInteterval);
        anim = GetComponentsInChildren<Animation>();
        //anim[1].Play();
        //InvokeRepeating("PlayAnim", 0, Random.Range(1, 3));
    }

    
//    /// <summary>
//    /// 动画播放
//    /// </summary>
//    void PlayAnim()
//    {
//        //Debug.Log(anim);
//        //Debug.Log("2");
//        anim[1].Play();
//    }

    /// <summary>
    /// 计算力
    /// </summary>
    void CalForce()
    {
        sumForce = Vector3.zero;
        //分离的力
        separationForce = Vector3.zero;
        //队列的力
        alignmentForce = Vector3.zero;
        //聚集的力
        cohesionForce = Vector3.zero;

        #region 分离
        separationNeighbors.Clear();
                
        //分离阈值内的鸟类碰撞体集合
        Collider[] colliders = Physics.OverlapSphere(transform.position, separationDistance);
        
        foreach (var VARIABLE in colliders)
        {
            //非自身
            if (VARIABLE != null && VARIABLE.gameObject != this.gameObject)
            {
                separationNeighbors.Add(VARIABLE.gameObject);
            }
        }
        //计算分离的力
        foreach (var VARIABLE in separationNeighbors)
        {
            //分离的方向
            Vector3 dir = transform.position - VARIABLE.transform.position;
            
            //距离越远，加的力越小
            separationForce += dir.normalized / dir.magnitude;
        }
        if (separationNeighbors.Count > 0)
        {
            anim[1].Play();
            //力乘以这个力的权重
            separationForce *= separationWeight;   
            
            sumForce += separationForce;
        }
        #endregion

        #region 队列
        //队列阈值内的鸟类碰撞体集合
        Collider[] collidersAlign = Physics.OverlapSphere(transform.position, alignmentDistance);
        
        foreach (var VARIABLE in collidersAlign)
        {
            if (VARIABLE != null && VARIABLE.gameObject != this.gameObject)
            {
                alignmentNeighbors.Add(VARIABLE.gameObject);
            }
        }
        //初始化队列里鸟的平均方向
        Vector3 avgDir = Vector3.zero;
        
        foreach (var VARIABLE in alignmentNeighbors)
        {
            avgDir += VARIABLE.transform.forward;
        }
        if (alignmentNeighbors.Count > 0)
        {
            avgDir /= alignmentNeighbors.Count;
            //用向量的减法实现目标方向的偏移 这里减前方向而不是位置本身， 是为了让鸟与鸟群在前进方向上相对静止，只是减少偏移鸟的偏移方向速度
            alignmentForce = avgDir - transform.forward;
            
            alignmentForce *= alignmentWeight;
            sumForce += alignmentForce;
        }
        #endregion

        #region 聚集
        if (alignmentNeighbors.Count <= 0)
        {
            return;
        }
        Vector3 center = Vector3.zero;
        foreach (var VARIABLE in alignmentNeighbors)
        {
            center += VARIABLE.transform.position;
        }
        //计算鸟群的中心位置
        center /= alignmentNeighbors.Count;

        Vector3 dirToCenter = center - transform.position;
        cohesionForce += dirToCenter;
        cohesionForce *= cohesionWeight;
        sumForce += cohesionForce;

        #endregion
        
    }
    // Update is called once per frame
    void Update()
    {
        //加速度
        Vector3 a = sumForce / crowM;
        velocity += a * Time.deltaTime;
        transform.Translate(velocity * Time.deltaTime,Space.World);
    }
}
