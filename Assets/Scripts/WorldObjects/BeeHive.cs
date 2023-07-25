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
    public int Honey
    {
        get => honey;
        set
        {

            var oldHoney = honey;
            honey = value;
            if (honey > maxHoney) honey = maxHoney;
            var diff = honey - oldHoney;

            honeyCounter += diff;
            if (honeyCounter >= 100)
            {
                Heal();
            }
            else
            {
                UpdateSprite();
            }
        }
    }
    public int maxHoney;
    private float honeyCounter;

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
        FullHoneyAnimation();

        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject() && honey >= maxHoney)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(pos, transform.position) < 0.5f)
            {
                AudioSource.PlayClipAtPoint(gainPollen, transform.position, 0.6f);
                Inventory.Instance.Honey += Honey;
                Stats.Instance[Stats.Name.TotalHoney] += Honey;
                Honey = 0;
            }
        }
    }
    void OnDestroy()
    {
        StopAllCoroutines();
    }
    void FullHoneyAnimation()
    {
        if (honey < maxHoney)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = Vector3.one + Vector3.one * Mathf.Sin(Time.time * 2 * Mathf.PI) / 4f;
        }
    }
    public void Hurt()
    {
        currentLife--;
        Stats.Instance[Stats.Name.HiveDamage]++;
        if (life <= 0)
        {
            Stats.Instance[Stats.Name.HivesDestroyed]++;
            Destroy(gameObject);
            return;
        }

        UpdateSprite();
        StartCoroutine(ShowHurt());
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
        honeyCounter -= 100;
        currentLife++;
        UpdateSprite();
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
}
