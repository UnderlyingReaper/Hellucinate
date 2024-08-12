using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Journal_Manager : MonoBehaviour
{
    public bool isOpen;
    public GameObject journalObj;
    public Transform pagesHolder;
    public float xOffset;
    public int maxPages;
    public int currPage;

    public AudioClip writeClip, flipClip;

    public TextMeshProUGUI currPageDisplay;
    public List<Page> pages = new();


    PlayerInputManager _playerInput;
    AudioSource _source;



    void Start()
    {
        _playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputManager>();
        _playerInput.playerInput.Player.Journal.performed += OnButtonPress;

        _source = GetComponent<AudioSource>();
    }

    private void OnButtonPress(InputAction.CallbackContext context)
    {
        if(isOpen)
        {
            journalObj.SetActive(false);
            isOpen = false;
        }
        else if(!isOpen)
        {
            ActivatePage();
            journalObj.SetActive(true);
            isOpen = true;
        }
    }

    Vector2 _orgScale;
    public void OnButtonHoverEnter(Transform obj)
    {
        _orgScale = obj.localScale;
        obj.DOScale(obj.localScale * 1.1f, 0.3f);
    }

    public void OnButtonHoverPress(int dir)
    {
        DeactivatePage();

        currPage += dir;
        currPage = Math.Clamp(currPage, 0, maxPages);
        currPageDisplay.text = (currPage + 1).ToString();

        if(currPage != 0 || currPage != maxPages) _source.PlayOneShot(flipClip);

        ActivatePage();
    }

    void ActivatePage()
    {
        if(currPage > pages.Count - 1 || currPage < 0) return;

        if(pages[currPage].rightPageContents != null)
        {
            pages[currPage].rightPageContents.gameObject.SetActive(true);
        }

        if(pages[currPage].leftPageContents != null)
            pages[currPage].leftPageContents.SetActive(true);
    }

    void DeactivatePage()
    {
        if(currPage > pages.Count - 1 || currPage < 0) return;

        if(pages[currPage].rightPageContents != null)
            pages[currPage].rightPageContents.SetActive(false);

        if(pages[currPage].leftPageContents != null)
            pages[currPage].leftPageContents.SetActive(false);
    }

    public void AddPage(GameObject page, bool isDouble)
    {
        _source.PlayOneShot(writeClip);

        if(isDouble)
        {
            if(pages[pages.Count - 1].rightPageContents != null)
            {
                Page newPg2 = new();
                pages.Add(newPg2);
            }

            GameObject newObj = Instantiate(page, pagesHolder);
            newObj.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

            pages[pages.Count - 1].rightPageContents = newObj;
            pages[pages.Count - 1].leftPageContents = newObj;

            if(currPage != pages.Count -1) newObj.SetActive(false);


            Page newPg = new();
            pages.Add(newPg);

            return;
        }

        if(pages[pages.Count - 1].rightPageContents == null)
        {
            GameObject newObj = Instantiate(page, pagesHolder);
            newObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(-xOffset, 0, 0);
            pages[pages.Count - 1].rightPageContents = newObj;

            if(currPage != pages.Count -1) newObj.SetActive(false);

            return;
        }
        else if(pages[pages.Count - 1].leftPageContents == null)
        {
            GameObject newObj = Instantiate(page, pagesHolder);
            newObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(xOffset, 0, 0);
            pages[pages.Count - 1].leftPageContents = page;

            if(currPage != pages.Count -1) newObj.SetActive(false);
        }

        Page newPage = new();
        pages.Add(newPage);
    }

    public void OnButtonHoverExit(Transform obj)
    {
        obj.DOScale(_orgScale, 0.15f);
    }
}

[System.Serializable]
public class Page
{
    public GameObject rightPageContents, leftPageContents;
}
