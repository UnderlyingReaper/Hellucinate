using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Creature_Spawner : MonoBehaviour
{
    public GameObject creaturePrefab;
    public float speedMp = 1;
    public List<Transform> cameraCornerPoints;
    public List<GameObject> warningSigns;
    public bool doDestroyOnEnd;

    public event EventHandler<OnSpawnEnemyArgs> OnSpawnCreature;
    public class OnSpawnEnemyArgs : EventArgs {
        public GameObject creature;
    }

    public int minDelay, maxDelay;


    Creature_Chase_AI _creatureAi;
    CanvasGroup _generalFade;


    void Start()
    {
        _generalFade = GameObject.FindGameObjectWithTag("GameplayFade").GetComponent<CanvasGroup>();
    }

    public IEnumerator StartSearch()
    {
        float randomDelay = UnityEngine.Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(randomDelay);

        GameObject creature = Instantiate(creaturePrefab, transform.position, Quaternion.Euler(Vector3.zero));
        _creatureAi = creature.GetComponent<Creature_Chase_AI>();

        _creatureAi.speed *= speedMp;
        _creatureAi.SearchEndResult += CurrentChaseEnd;

        _creatureAi.PlayGrowl();
        _creatureAi.CalculateDestination(cameraCornerPoints);
        OnSpawnCreature?.Invoke(this, new OnSpawnEnemyArgs { creature = creature });

        StartCoroutine(WarningSymbolsAnimation(_creatureAi.IsFacingRight()));
    }

    IEnumerator WarningSymbolsAnimation(bool isFacingRight)
    {
        _generalFade.DOFade(0.75f, 1);

        if(isFacingRight) warningSigns[1].SetActive(true);
        else warningSigns[0].SetActive(true);

        yield return new WaitForSeconds(0.3f);

        if(isFacingRight) warningSigns[1].SetActive(false);
        else warningSigns[0].SetActive(false);

        yield return new WaitForSeconds(0.3f);

        if(isFacingRight) warningSigns[1].SetActive(true);
        else warningSigns[0].SetActive(true);

        yield return new WaitForSeconds(0.3f);

        if(isFacingRight) warningSigns[1].SetActive(false);
        else warningSigns[0].SetActive(false);

        yield return new WaitForSeconds(0.3f);

        
        if(isFacingRight) warningSigns[1].SetActive(true);
        else warningSigns[0].SetActive(true);

        yield return new WaitForSeconds(0.3f);

        warningSigns[1].SetActive(false);
        warningSigns[0].SetActive(false);

        yield return new WaitForSeconds(0.5f);

        _creatureAi.DoSearch();
    }

    public void CurrentChaseEnd(object sender, Creature_Chase_AI.SearchEndResultArgs e)
    {
        _generalFade.DOFade(0, 1);

        if(doDestroyOnEnd) Destroy(gameObject);
    }

    public IEnumerator UnFade()
    {
        yield return new WaitForSeconds(2);
        _generalFade.DOFade(0, 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player") StartCoroutine(StartSearch());
    }
}
