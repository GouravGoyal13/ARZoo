using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BombHit
{
    public string HitObjectName { get; set; }
    public int Score { get; set; }

    public BombHit (string name, int score = 0)
    {
        HitObjectName = name;
        Score = score;
    }
}


public class BombBehaviour : MonoBehaviour
{
    public static event EventHandler OnBombPlanted;
    public static event EventHandler<BombHit> OnBombHit;

    [SerializeField] GameObject[] numbers;
    [SerializeField] int explodeTime = 3; // in sec
    [SerializeField] LineRenderer line;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] int firePower = 2;

    WaitForSeconds wait = new WaitForSeconds(1);

    private void Awake()
    {
        OnBombPlanted += DestroyBomb;
    }

    private void OnDestroy()
    {
        OnBombPlanted -= DestroyBomb;
    }

    void Start()
    {
        StartCoroutine(StartTimer(explodeTime));
    }

    IEnumerator StartTimer(int timeDelay)
    {
        int idx = 0;
        while (timeDelay != 0)
        {
            if(idx!=0)
            {
                numbers[idx - 1].SetActive(false);
            }
            numbers[idx].SetActive(true);
            idx++;
            timeDelay -= 1;
            yield return wait;
        }
        numbers[idx - 1].SetActive(false);

        if (OnBombPlanted != null)
            OnBombPlanted(this,EventArgs.Empty);
    }

    void DestroyBomb(object sender,EventArgs e)
    {
        StartCoroutine(CreateExplosion(Vector3.right));
        StartCoroutine(CreateExplosion(Vector3.left));
        StartCoroutine(CreateExplosion(Vector3.up));
        StartCoroutine(CreateExplosion(Vector3.down));

        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 3);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                OnBombHit(this, new BombHit(col.tag));
                Destroy(col.gameObject, .1f);
            }
        }
        this.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, .3f);
    }

    IEnumerator CreateExplosion(Vector3 direction)
    {
        for (int i = 1; i < firePower; i++)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(transform.position, direction, i);
            Debug.DrawRay(transform.position, direction * i,Color.yellow);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider != null)
                {
                    if (!hit.collider.tag.Equals("wall"))
                    {
                        Debug.Log(hit.collider.tag + " Hit");
                        switch (hit.collider.tag)
                        {
                            case "block":
                                OnBombHit(this, new BombHit(hit.collider.tag));
                                Destroy(hit.collider.gameObject, .1f);

                                break;
                            case "Player":
                                OnBombHit(this, new BombHit(hit.collider.tag));
                                Destroy(hit.collider.gameObject, .1f);

                                break;
                            case "Enemy":
                                EnemyBehaviour enemy = hit.collider.GetComponent<EnemyBehaviour>();
                                if (enemy != null)
                                {
                                    Debug.Log("enemy Kiiled");
                                    OnBombHit(this, new BombHit(hit.collider.tag, enemy.EnemyKillScore));
                                }
                                Destroy(hit.collider.gameObject, .1f);

                                break;
                        }
                        Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);

            yield return new WaitForSeconds(.05f);
        }
    }
}
