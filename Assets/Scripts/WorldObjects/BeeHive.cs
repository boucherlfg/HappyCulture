using System.Collections;
using System.Linq;
using UnityEngine;

public class BeeHive : Buyable
{
    [SerializeField]
    private int life = 3;
    private int currentLife;

    [SerializeField]
    private Sprite[] hiveStates;
    public GameObject honeyExplosion;
    public AudioClip gainPollen;
    public GameObject beePrefab;
    public int maxBees = 20;
    public float spawnRate = 5;
    private int honey;
    public int maxHoney;
    public float FillRatio => honey / (float)maxHoney;

    public void AddHoney(int honey)
    {
        var oldHoney = this.honey;

        this.honey = Mathf.Min(maxHoney, this.honey + honey);
        
        var diff = this.honey - oldHoney;
        Stats.Instance[Stats.Name.TotalHoney] += diff;

        if (diff == 0 && honey > 0)
        {
            Hurt();
        }
        UpdateSprite();
    }
    

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
        Stats.Instance[Stats.Name.CurrentHive]++;
    }
    // Update is called once per frame
    void Update()
    {
        SpawnBees();
        UpdateScale();
    }
    void OnDestroy()
    {
        Stats.Instance[Stats.Name.CurrentHive]--;
        StopAllCoroutines();
    }
    protected override void UpdateScale()
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
        if (!Settings.Instance.PeaceMode)
        {
            currentLife--;
            Stats.Instance[Stats.Name.HiveDamage]++;
            if (currentLife <= 0)
            {
                Stats.Instance[Stats.Name.HivesDestroyed]++;
                Destroy(gameObject);
                return;
            }
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
        UpdateSprite();
        
    }
    public void Heal()
    {
        if (!Settings.Instance.PeaceMode)
        {
            currentLife = Mathf.Min(currentLife + 1, life);

        }
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
        StartCoroutine(ShowHeal());
        UpdateSprite();
    }
    void UpdateSprite()
    {
        const int width = 3;
        const int height = 4;

        int life = this.currentLife - 1;

        int x = width * life / this.life;
        int y = (height - 2) * honey / maxHoney;

        x = Mathf.Min(x, width - 1);
        y = Mathf.Min(y, height - 2);

        if (honey <= 0) y = 0;
        else if (honey >= maxHoney) y = height - 1;
        else y += 1;

        int index = x + y * width;
        if (index < 0 || index > hiveStates.Length - 1) return;
        GetComponent<SpriteRenderer>().sprite = hiveStates[index];
    }
    public override bool OnClick()
    {
        float multiplier = currentLife / (float)life;
        if (0 < honey)
        {
            Sound.Instance.PlayOnce(gainPollen, 0.2f);
            Inventory.Instance.Honey += (int)(multiplier * honey);
            Instantiate(honeyExplosion, transform.position, Quaternion.identity);
            honey = 0;
            Heal();
            return true;
        }
        return false;
    }
}
