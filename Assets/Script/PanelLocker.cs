using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelLocker : MonoBehaviour
{
    private int colliderCount = 0;
    private bool isLocked = false;
    private float timer = 0f;
    private bool actionTaken = false;

    [Header("Settings")]
    [SerializeField]
    private float holdTime = 1f;

    [Header("Components")]
    [SerializeField]
    private RawImage statusImg;
    [SerializeField]
    private Image timerRing;
    [SerializeField]
    private AudioSource audioSrc;
    [SerializeField]
    private List<GameObject> panelButtons;

    [Header("Assets")]
    [SerializeField]
    private Texture lockImg;
    [SerializeField]
    private Texture unlockImg;
    [SerializeField]
    private AudioClip lockSound;
    [SerializeField]
    private AudioClip unlockSound;

    private void Start()
    {
        statusImg.texture = unlockImg;
        audioSrc.clip = lockSound;
    }

    private void OnTriggerEnter(Collider _)
    {
        ++colliderCount;
    }

    private void OnTriggerExit(Collider _)
    {
        colliderCount = Mathf.Clamp(colliderCount - 1, 0, colliderCount);
    }

    private void Update()
    {
        if (colliderCount >= 1)
        {
            timer += Time.unscaledDeltaTime;
            float ratio = Mathf.Clamp(timer, 0, holdTime) / holdTime;

            timerRing.fillAmount = Mathf.Pow(ratio, 3f);

            if (ratio >= 1 && !actionTaken)
            {
                isLocked = !isLocked;
            
                audioSrc.clip = isLocked ? lockSound : unlockSound;
                audioSrc.Play();

                timerRing.color = Color.cyan;
                statusImg.texture = isLocked ? lockImg : unlockImg;
                
                foreach (var btn in panelButtons)
                {
                    btn.SetActive(!isLocked);
                }

                actionTaken = true;
            }
        }
        else
        {
            timer = 0;
            timerRing.fillAmount = 0;
            timerRing.color = Color.white;
            actionTaken = false;
        }
    }
}
