using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Creature_Spawner : MonoBehaviour
{
    public MonoBehaviour objectToSubscribe;

    public GameObject creaturePrefab;
    public float speedMp = 1;
    public List<Transform> cameraCornerPoints;
    public bool doDestroyOnEnd;

    public event EventHandler<OnSpawnEnemyArgs> OnSpawnCreature;
    public class OnSpawnEnemyArgs : EventArgs {
        public GameObject creature;
    }

    public float minDelay, maxDelay;


    Creature_Chase_AI _creatureAi;
    CanvasGroup _generalFade;


    void Start()
    {
        _generalFade = GameObject.FindGameObjectWithTag("GameplayFade").GetComponent<CanvasGroup>();
    }

    public IEnumerator StartSearch(float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);

        float randomDelay = UnityEngine.Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(randomDelay);

        GameObject creature = Instantiate(creaturePrefab, transform.position, Quaternion.Euler(Vector3.zero));
        _creatureAi = creature.GetComponent<Creature_Chase_AI>();

        _creatureAi.speed /= speedMp;
        _creatureAi.SearchEndResult += CurrentChaseEnd;

        _creatureAi.PlayGrowl();
        _creatureAi.CalculateDestination(cameraCornerPoints);
        OnSpawnCreature?.Invoke(this, new OnSpawnEnemyArgs { creature = creature });

        StartCoroutine(ShowWarning());
    }

    IEnumerator ShowWarning()
    {
        _generalFade.DOFade(0.75f, 1);

        yield return new WaitForSeconds(0.5f);

        _creatureAi.DoSearch();
    }

    public void CurrentChaseEnd(object sender, Creature_Chase_AI.SearchEndResultArgs e)
    {
        _generalFade.DOFade(0, 1);

        if(doDestroyOnEnd)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    public IEnumerator UnFade()
    {
        yield return new WaitForSeconds(2);
        _generalFade.DOFade(0, 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player") StartCoroutine(StartSearch(0));
    }
}
