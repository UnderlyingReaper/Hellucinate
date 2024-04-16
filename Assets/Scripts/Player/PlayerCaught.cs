using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCaught : MonoBehaviour
{
    public List<GameObject> spawners;
    public CanvasGroup cg;


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

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
