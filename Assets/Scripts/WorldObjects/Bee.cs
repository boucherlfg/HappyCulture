using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Bee : MonoBehaviour
{
    private float oneOverE = 1/Mathf.Exp(1);
    public static List<Bee> all = new List<Bee>();
    #region [physics]
    private float speedBoost = 1;
    public float speed = 3;
    public float turnSpeed = 1;
    public float wallDetection = 3;
    private Rigidbody2D rbody;
    private SpriteRenderer rend;
    private Flower target;
    private float dirX = 1;
    #endregion

    #region [timers]
    public float lifeTime = 60;
    public float pollinateTime = 1;
    private float pollinateTimeCounter = 0;
    private float lifeTimeCounter = 0;
    #endregion

    #region [logic]
    private bool foundFlower => target;
    private bool isFleeing = false;
    private bool isOver => lifeTimeCounter >= lifeTime || badHoney;
    private bool pollinating => foundFlower && Vector2.Distance(target.transform.position, transform.position) < 0.5
                             && pollinateTimeCounter < pollinateTime;
    private bool isAtBase => beehive && Vector2.Distance(beehive.transform.position, transform.position) < 0.5
                             && isOver;
    private bool badHoney;
    #endregion

    #region flowers
    public BeeHive beehive;
    public GameObject pollenEffect;
    private int honey;
    private FlowerType flowerType;
    [SerializeField]
    private float flowerDetectionRange = 10;
    [System.Serializable]
    public struct FlowerAuraPair
    {
        public FlowerType flowerType;
        public GameObject aura;
    }
    public List<FlowerAuraPair> flowerAuraPairs;
    #endregion

    public void AddSpeedBoost(float adder)
    {
        speedBoost += adder;
    }
    // Start is called before the first frame update
    void Start()
    {
        all.Add(this);
        ChooseFlower();
        rbody = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        rbody.velocity = new Vector2(Mathf.Cos(0), Mathf.Sin(0)) * speed;
        beehive.beeCount++;
        Stats.Instance[Stats.Name.CurrentBees]++;
        Stats.Instance[Stats.Name.BeesSpawned]++;
    }
    void OnDestroy()
    {
        all.Remove(this);
        if (beehive)
        {
            if (badHoney)
            {
                beehive.Hurt();
                honey = 0;
            }
            beehive.AddHoney(honey);
            beehive.beeCount--;
        }
        Stats.Instance[Stats.Name.CurrentBees]--;
    }

    // Update is called once per frame
    void Update()
    {
        var regularSpeed = speed;
        var regularTurnSpeed = turnSpeed;
        var regularPollinateTime = pollinateTime;

        speed = regularSpeed + regularSpeed * (speedBoost - 1);
        turnSpeed = regularTurnSpeed + regularTurnSpeed * (speedBoost - 1) / 2;
        pollinateTime = regularPollinateTime * (1/Mathf.Pow(speedBoost, oneOverE));
        
        lifeTimeCounter += Time.deltaTime;

        if (!beehive)
        {
            Flee();
        }
        else if (isAtBase)
        {
            Destroy(gameObject);
        }
        else if (isOver)
        {
            ReturnToBase();
        }
        else if (!foundFlower)
        {
            Scout();
        }
        else if (pollinating)
        {
            Pollinate();
        }
        else
        {
            MoveTowards();
        }
        Animate();

        speedBoost = 1;
        speed = regularSpeed;
        turnSpeed = regularTurnSpeed;
        pollinateTime = regularPollinateTime;
    }
    void Animate()
    {
        rend.flipX = dirX < 0.2f;
        dirX = rbody.velocity.x;
    }

#region AI methods
void Scout()
{
        float dist(Vector2 other) => Vector2.Distance(transform.position, other);

        var rand = GetRandomVector();
        var avoid = GetAvoidanceVector();
        rbody.velocity = (rand + avoid).normalized * speed;

        var bounds = Map.Instance.Bounds(transform.position);
        var flowers = Physics2D.OverlapAreaAll(bounds.min, bounds.max)
                               .Where(x => x.GetComponent<Flower>())
                               .Select(x => x.GetComponent<Flower>()).ToList();
        flowers = flowers.FindAll(x => x.IsFlowerOk(flowerType) && dist(x.transform.position) < flowerDetectionRange);

        if (flowers.Count() > 0)
        {
            target = flowers.Minimum(x => dist(x.transform.position));
        }
    }
    void Flee()
    {
        if (!isFleeing)
        {
            isFleeing = true;
            rbody.velocity = Random.insideUnitCircle.normalized;
        }
        if (Vector2.Distance(transform.position, Vector2.zero) > 400)
        {
            Destroy(gameObject);
        }
    }
    void MoveTowards()
    {
        var rand = GetRandomVector() * 0.01f;
        var dir = GetVectorTowards(target.transform.position);
        rbody.velocity = (rand + dir).normalized * speed;
    }
    void ReturnToBase()
    {
        if (beehive)
        {
            var baseDir = GetVectorTowards(beehive.transform.position);
            rbody.velocity = baseDir * speed;
        }
    }
    void ChooseFlower()
    {
        var flowerAuraPair = flowerAuraPairs.PickRandom();
        flowerType = flowerAuraPair.flowerType;

        //deactivate all aura except right colour
        foreach (var pair in flowerAuraPairs) pair.aura.SetActive(flowerAuraPair.flowerType == pair.flowerType);
    }
    void Pollinate()
    {
        MoveTowards();
        pollinateTimeCounter += Time.deltaTime;
        if (pollinateTimeCounter < pollinateTime) return;
        pollinateTimeCounter = 0;

        if (target)
        {
            var comp = target.GetComponent<Flower>();
            if (comp)
            {
                Instantiate(pollenEffect, transform.position, Quaternion.identity);
                honey += comp.TakePollen();
                badHoney = comp is Trash;
            }
        }
        target = null;
        ChooseFlower();
        
    }
    #endregion

    Vector2 GetAvoidanceVector()
    {
        Vector2 getCloser(Vector2 a, Vector2 b) => Vector2.Distance(a, transform.position) < Vector2.Distance(b, transform.position) ? a : b;

        var bound = Map.Instance.Bounds(Vector2Int.RoundToInt(transform.position));

        var vect = new Vector2(bound.min.x, transform.position.y);
        vect = getCloser(new Vector2(bound.max.x, transform.position.y), vect);
        vect = getCloser(new Vector2(transform.position.x, bound.min.y), vect);
        vect = getCloser(new Vector2(transform.position.x, bound.max.x), vect);

        var diff = (vect - (Vector2)transform.position);
        var dist = diff.magnitude;

        var effect = wallDetection - dist;
        if (effect < 0) return Vector2.zero;

        return -diff.normalized * effect;
    }
    Vector2 GetVectorTowards(Vector2 target)
    {
        var orientation = Mathf.Atan2(rbody.velocity.y, rbody.velocity.x) * Mathf.Rad2Deg;
        var diff = target - (Vector2)transform.position;
        var angle = Vector2.Angle(rbody.velocity.normalized, diff.normalized);
        angle *= turnSpeed * Time.deltaTime;
        orientation += angle;
        return Quaternion.Euler(0, 0, orientation) * Vector2.right;
    }
    Vector2 GetRandomVector()
    {
        var orientation = Mathf.Atan2(rbody.velocity.y, rbody.velocity.x) * Mathf.Rad2Deg;
        orientation += Random.Range(-turnSpeed, turnSpeed);
        return Quaternion.Euler(0, 0, orientation) * Vector2.right;
    }
}
