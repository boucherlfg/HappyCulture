using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Robot : Buyable
{
    public const string robot_left = nameof(robot_left),
                        robot_right = nameof(robot_right),
                        robot_back = nameof(robot_back),
                        robot_front = nameof(robot_front),
                        robot_idle_left = nameof(robot_idle_left),
                        robot_idle_right = nameof(robot_idle_right),
                        robot_idle_back = nameof(robot_idle_back),
                        robot_idle_front = nameof(robot_idle_front);

    public float speed = 3;
    public float turnSpeed = 1;
    public float wallDetection = 3;
    private Rigidbody2D rbody;
    private Animator anim;
    private Vector2 target;
    public AudioClip trashSound;

    private float interactTimeCounter;
    public float interactTime;
    public float abandonTime = 10;
    private float abandonTimer;
    private bool foundTarget;
    private bool interacting => Vector2.Distance(target, transform.position) < 0.5
                             && interactTimeCounter < interactTime;
    
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rbody.velocity = new Vector2(Mathf.Cos(0), Mathf.Sin(0)) * speed;
    }

    void Update()
    {
        if (!foundTarget)
        {
            if (Vector2.Distance(transform.position, target) < 0.1)
            {
                ChooseRandomDirection();
            }
            Scout();
        }
        else if (interacting)
        {
            Activate();
        }
        else
        {
            MoveTowards();
        }
        Animate();
    }

    void Animate()
    {
        var moving = rbody.velocity.magnitude > 0.1;
        anim.SetBool("moving", moving);
        if (moving)
        {
            anim.SetFloat("vx", rbody.velocity.x);
            anim.SetFloat("vy", rbody.velocity.y);
        }
        UpdateScale();
    }

    
    void ChooseRandomDirection() 
    {
        target = FindValidPosition();
    }
    
    void Scout()
    {
        var rand = GetVectorTowards(target);
        var avoid = Vector2.zero;//GetAvoidanceVector();
        rbody.velocity = (rand + avoid).normalized * speed;

        var bounds = Map.Instance.Bounds(transform.position);
        //chercher pour des trash
        var hits = Physics2D.OverlapAreaAll(bounds.min, bounds.max).ToList();
        var trashes = hits.FindAll(x => x.GetComponent<Trash>());

        if (trashes.Count() > 0)
        {
            var pos = transform.position;
            float dist(Vector2 other) => Vector2.Distance(pos, other);
            target = trashes.Minimum(x => dist(x.transform.position)).transform.position;
            foundTarget = true;
        }
        else 
        {
            //chercher pour des hives
            var hiveObjs = hits.FindAll(x => x.GetComponent<BeeHive>() && x.GetComponent<BeeHive>().FillRatio > 0.25f);

            if (hiveObjs.Count() > 0)
            {
                var pos = transform.position;
                float dist(Vector2 other) => Vector2.Distance(pos, other);
                target = hiveObjs.Minimum(x => dist(x.transform.position)).transform.position;
                foundTarget = true;
            }
        }
    }
    void MoveTowards()
    {
        var dir = GetVectorTowards(target);
        var avoid = Vector2.zero;// GetAvoidanceVector();
        rbody.velocity = (dir + avoid).normalized * speed;

        abandonTimer += Time.deltaTime;
        if (abandonTimer > abandonTime)
        {
            abandonTime = 0;
            foundTarget = false;
        }
    }
    void Activate()
    {
        rbody.velocity = Vector2.zero;
        interactTimeCounter += Time.deltaTime;
        if (interactTimeCounter < interactTime) return;
        interactTimeCounter = 0;

        var obj = Physics2D.OverlapPointAll(target);
        var trash = obj.FirstOrDefault(x => x.GetComponent<Trash>());

        if (trash)
        {
            Destroy(trash.gameObject);
            Sound.Instance.PlayOnce(trashSound, 0.5f);
        }
        else
        {
            var beeHive = obj.FirstOrDefault(x => x.GetComponent<BeeHive>());
            if (beeHive)
            {
                var comp = beeHive.GetComponent<BeeHive>();
                comp.OnClick();
            }
        }
        foundTarget = false;
    }

    /// <summary>
    /// find a point that is in bounds and not over something else
    /// </summary>
    Vector2 FindValidPosition()
    {
        Vector2 whereToSpawn;
        var bounds = Map.Instance.Bounds(transform.position);
        for (int _ = 0; _ < 1000; _++)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            whereToSpawn = new Vector2(x, y);

            if (!Map.Instance.Contains(whereToSpawn)) continue;
            if (Physics2D.OverlapPoint(whereToSpawn)) continue;
            return whereToSpawn;
        }
        throw new System.Exception("cant find valid point");
    }
    //Vector2 GetAvoidanceVector()
    //{
    //    Vector2 getCloser(Vector2 a, Vector2 b) => Vector2.Distance(a, transform.position) < Vector2.Distance(b, transform.position) ? a : b;

    //    var bound = Map.Instance.Bounds(Vector2Int.RoundToInt(transform.position));

    //    var vect = new Vector2(bound.min.x + 1, transform.position.y);
    //    vect = getCloser(new Vector2(bound.max.x - 1, transform.position.y), vect);
    //    vect = getCloser(new Vector2(transform.position.x, bound.min.y + 1), vect);
    //    vect = getCloser(new Vector2(transform.position.x, bound.max.x - 1), vect);

    //    var diff = (vect - (Vector2)transform.position);
    //    var dist = diff.magnitude;

    //    var effect = wallDetection - dist;
    //    if (effect < 0) return Vector2.zero;

    //    return -diff.normalized * effect;
    //}
    Vector2 GetVectorTowards(Vector2 target)
    {
        var diff = target - (Vector2)transform.position;
        return diff.normalized;
    }
}
