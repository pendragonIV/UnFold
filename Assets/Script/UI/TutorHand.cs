using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorHand : MonoBehaviour
{
    private void Start()
    {
        if(LevelManager.instance.currentLevelIndex == 0)
        {
            StartCoroutine(WaitForActive());
            Vector3 destination = transform.localPosition + new Vector3(0, 1.5f, 0);
            this.transform.DOLocalMove(destination, 1).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Restart);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator WaitForActive()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(.5f);
        this.GetComponent<SpriteRenderer>().enabled = true;
    }
}
