using System.Collections;
using UnityEngine;

public class CandyAnimator
{
    public IEnumerator AnimateSelect(Transform target)
    {
        Vector3 originalScale = Vector3.one;
        Vector3 selectedScale = Vector3.one * 1.1f;

        float duration = 0.1f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            target.localScale =
                Vector3.Lerp(originalScale, selectedScale, timer / duration);

            yield return null;
        }

        target.localScale = selectedScale;
    }

    public IEnumerator AnimateSelectedLoop(Transform target)
    {
        while (true)
        {
            float angle = Mathf.Sin(Time.time * 12f) * 6f;

            target.rotation = Quaternion.Euler(0, 0, angle);

            yield return null;
        }
    }

    public IEnumerator AnimateUnselect(Transform target)
    {
        target.rotation = Quaternion.identity;

        Vector3 startScale = target.localScale;

        float duration = 0.1f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            target.localScale =
                Vector3.Lerp(startScale, Vector3.one, timer / duration);

            yield return null;
        }

        target.localScale = Vector3.one;
    }

    public IEnumerator AnimateSwap(
        Transform first,
        Vector3 firstTargetPosition,
        Transform second,
        Vector3 secondTargetPosition)
    {
        Vector3 firstStartPosition = first.position;
        Vector3 secondStartPosition = second.position;

        float duration = 0.18f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            first.position = Vector3.Lerp(firstStartPosition, firstTargetPosition, t);
            second.position = Vector3.Lerp(secondStartPosition, secondTargetPosition, t);

            yield return null;
        }

        first.position = firstTargetPosition;
        second.position = secondTargetPosition;
    }
}