using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCaught : MonoBehaviour
{
    public Player_Movement pm;
    public List<GameObject> spawners;
    public CanvasGroup cg;
    public TextMeshProUGUI deathTxt;


    void Start()
    {
        spawners = new List<GameObject>();
        GameObject[] spawnerObjs = GameObject.FindGameObjectsWithTag("Spawner");

        foreach(GameObject spawnerObj in spawnerObjs)
        {
            spawners.Add(spawnerObj);
            spawnerObj.GetComponent<Creature_Spawner>().OnSpawnCreature += OnEnemySpawn;
        }
    }

    public void OnEnemySpawn(object sender, Creature_Spawner.OnSpawnEnemyArgs e)
    {
        e.creature.GetComponent<Creature_Chase_AI>().SearchEndResult += CreatureSearchResult;
        Debug.Log("Enemy Spawned");
    }

    public void CreatureSearchResult(object sender, Creature_Chase_AI.SearchEndResultArgs e)
    {
        if(e.didCatchPlayer)
        {
            Debug.Log("Player is Dead");
            pm.allow = false;
            deathTxt.text = e.deathTxt;
            StartCoroutine(PlayDeathAnimation());
        }
        else Debug.Log("Player is alive");
    }

    IEnumerator PlayDeathAnimation()
    {
        transform.DORotate(new Vector3(0, 0, -5), 2f);
        transform.DOLocalMove(new Vector3(0, 0, 4), 2f);

        yield return new WaitForSeconds(1.5f);

        cg.DOFade(1, 0.5f);

        yield return new WaitForSeconds(1.5f);

        deathTxt.DOFade(1, 2);

        yield return new WaitForSeconds(5);

        deathTxt.DOFade(0, 2);

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
