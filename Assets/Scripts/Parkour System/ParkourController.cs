using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    [SerializeField] List<ParkourAction> parkourActions;
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
                foreach (var action in parkourActions)
                {
                    if (action.CheckIfPossible(hitData, transform))
                    {
                        // Debug.Log("Obstacle found" + hitData.forwardHit.transform.name);
                        StartCoroutine(DoParkourAction(action));
                        break;
                    }
                }
            }
        }

    }

    IEnumerator DoParkourAction(ParkourAction action)
    {
        inAction = true;
        playerController.SetControl(false);
        animator.CrossFade(action.AnimName, 0.2f);
        yield return null;

        var animState = animator.GetNextAnimatorStateInfo(0);
        if (!animState.IsName(action.AnimName))
            Debug.LogError("The parkour animation is wrong!");

        yield return new WaitForSeconds(animState.length);

        playerController.SetControl(true);
        inAction = false;
    }
}
