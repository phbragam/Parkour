using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    bool inAction;

    EnviromentScanner enviromentScanner;
    Animator animator;
    PlayerController playerController;

    private void Awake()
    {
        enviromentScanner = GetComponent<EnviromentScanner>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetButton("Jump") && !inAction)
        {
            var hitData = enviromentScanner.ObstacleCheck();

            if (hitData.forwardHitFound)
            {
                // Debug.Log("Obstacle found" + hitData.forwardHit.transform.name);
                StartCoroutine(DoParkourAction());
            }
        }

    }

    IEnumerator DoParkourAction()
    {
        inAction = true;
        playerController.SetControl(false);
        animator.CrossFade("StepUp", 0.2f);
        yield return null;

        var animState = animator.GetNextAnimatorStateInfo(0);

        yield return new WaitForSeconds(animState.length);

        playerController.SetControl(true);
        inAction = false;
    }
}
