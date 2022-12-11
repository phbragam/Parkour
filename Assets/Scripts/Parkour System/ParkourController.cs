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

        animator.SetBool("mirrorAction", action.Mirror);
        animator.CrossFade(action.AnimName, 0.2f);
        yield return null;

        var animState = animator.GetNextAnimatorStateInfo(0);
        if (!animState.IsName(action.AnimName))
            Debug.LogError("The parkour animation is wrong!");

        float timer = 0f;

        while (timer <= animState.length)
        {
            timer += Time.deltaTime;

            if (action.RotateToObstacle)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, action.TargetRotation, playerController.RotationSpeed * Time.deltaTime);

            if (action.EnableTargetMatching)
                MatchTarget(action);

            // checking if timer is > .5 because we dont want to check transition from other animamation to actual animation
            if(animator.IsInTransition(0) && timer > .5f)
                break;

            yield return null;
        }

        yield return new WaitForSeconds(action.PostActionDelay);

        playerController.SetControl(true);
        inAction = false;
    }

    void MatchTarget(ParkourAction action)
    {
        if (animator.isMatchingTarget) return;

        animator.MatchTarget(action.MatchPos, transform.rotation, action.MatchBodyPart, new MatchTargetWeightMask(action.MatchPosWeight, 0),
            action.MatchStartTime, action.MatchTargetTime);
    }
}
