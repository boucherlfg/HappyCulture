using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeeHive : Buyable
{
    [SerializeField]
    private int life = 3;
    private int currentLife;

    [SerializeField]
    private Sprite[] hiveStates;
    
    public AudioClip gainPollen;
    public GameObject beePrefab;
    public int maxBees = 20;
    public float spawnRate = 5;
    private int honey;

    public void AddHoney(int honey)
    {
        var oldHoney = this.honey;
        this.honey += Mathf.Min(maxHoney, this.honey + honey);
        var diff = honey - oldHoney;
        honeyCounter += diff;
        Stats.Instance[Stats.Name.TotalHoney] += diff;
        if (honeyCounter >= 100)
        {
            Heal();
        }
        if (diff == 0)
        {
            Hurt();
        }
        UpdateSprite();
    }
    
    public int maxHoney;
    private float honeyCounter;

    public bool HasHoneyInIt => 0 < honey;

    [HideInInspector]
    public int beeCount;
    private float beeSpawnCounter = 0;
    void SpawnBees()
    {
        beeSpawnCounter += Time.deltaTime;
        if (beeSpawnCounter < spawnRate) return;
        beeSpawnCounter = 0;

        //check if right flower exists nearby
        var allHits = Physics2D.OverlapCircleAll(transform.position, 10);
        if (!allHits.Any(x => x.GetComponent<Flower>()))
        {
            Hurt();
            return;
        }

        if (beeCount >= maxBees)
        {
            return;
        }

        var bee = Instantiate(beePrefab, transform.position, Quaternion.identity);
        bee.name = "bee" + beeCount;
        bee.transform.SetParent(transform.parent);
        bee.GetComponent<Bee>().beehive = this;
        
    }
    void Start()
    {
        currentLife = life;
        Stats.Instance[Stats.Name.HivesPlaced]++;
    }
    // Update is called once per frame
    void Update()
    {
        SpawnBees();
        Animate();
    }
    void OnDestroy()
    {
        StopAllCoroutines();
    }
    void Animate()
    {
        transform.localScale = Vector3.one;
        if (Hover)
        {
            transform.localScale *= 1.2f;
        }
        else if(honey >= maxHoney)
        {
            transform.localScale += Vector3.one * Mathf.Sin(Time.time * 2 * Mathf.PI) / 4f;
        }
    }
    public void Hurt()
    {
        currentLife--;
        Stats.Instance[Stats.Name.HiveDamage]++;
        if (currentLife <= 0)
        {
            Stats.Instance[Stats.Name.HivesDestroyed]++;
            Destroy(gameObject);
            return;
        }

        UpdateSprite();
        if (gameObject)
        {
            StartCoroutine(ShowHurt());
        }
        IEnumerator ShowHurt()
        {
            var rend = GetComponent<SpriteRenderer>();
            rend.color = Color.red;
            while (rend.color.g < 1 && rend.color.b < 1)
            {
                var col = rend.color;
                col.g += Time.deltaTime;
                col.b += Time.deltaTime;
                rend.color = col;
                yield return null;
            }
            rend.color = Color.white;
        }
    }
    public void Heal()
    {
        IEnumerator ShowHeal()
        {
            var rend = GetComponent<SpriteRenderer>();
            rend.color = Color.green;
            while (rend.color.r < 1 && rend.color.b < 1)
            {
                var col = rend.color;
                col.r += Time.deltaTime;
                col.b += Time.deltaTime;
                rend.color = col;
                yield return null;
            }
            rend.color = Color.white;
        }
        honeyCounter -= 100;
        currentLife++;
        UpdateSprite();
        StartCoroutine(ShowHeal());
    }
    void UpdateSprite()
    {
        int life = this.currentLife - 1;

        int x = 3 * life / (this.life);
        int y = 4 * honey / maxHoney;

        int index = x + y * this.life;
        if (index < 0 || index > hiveStates.Length - 1) return;
        GetComponent<SpriteRenderer>().sprite = hiveStates[index];
    }
    public override bool OnClick()
    {
        if (0 < honey && honey <= maxHoney)
        {
            AudioSource.PlayClipAtPoint(gainPollen, transform.position, 0.6f);
            Inventory.Instance.Honey += honey;
            honey = 0;
            Heal();
            return true;
        }
        return false;
    }
}
