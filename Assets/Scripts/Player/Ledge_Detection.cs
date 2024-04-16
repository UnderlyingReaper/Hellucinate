using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Ledge_Detection : MonoBehaviour
{
    [Header("General")]
    public bool ledgeDetected;
    public LayerMask groundLayerMask;

    public Transform player;
    public Vector3 offset;
    public Animator playerAnim;
    public bool _canDetect;

    [Header("Colliders")]
    public float radius;
    public Vector3 boxOffset;
    public Vector2 boxSize;
    




    void Update()
    {
        _canDetect = !Physics2D.OverlapBox(transform.position + boxOffset, boxSize, 0, groundLayerMask);

        if(_canDetect)
            ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, groundLayerMask);
        if(!_canDetect) ledgeDetected = false;

        if(ledgeDetected && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ClimbLedge());
        }
    }

    IEnumerator ClimbLedge()
    {
        playerAnim.SetBool("climbLedge", true);
        yield return new WaitForSeconds(playerAnim.GetCurrentAnimatorClipInfo(0).Length);
        playerAnim.SetBool("climbLedge", false);

        player.transform.position = transform.position + offset;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireCube(transform.position + boxOffset, boxSize);
    }
}
