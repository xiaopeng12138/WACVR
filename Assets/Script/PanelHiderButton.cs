using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelHiderButton : MonoBehaviour
{
    private int colliderCount = 0;
    private bool isLocked = false;
    private float timer = 0f;
    private bool actionTaken = false;
    private Renderer r;

    [Header("Settings")]
    [SerializeField]
    private float holdTime = 1f;

    [Header("Components")]
    [SerializeField]
    private RawImage statusImg;
    [SerializeField]
    private Image timerRing;
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

    private AudioSource audioSrc;
    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        r = GetComponent<Renderer>();
        statusImg.texture = isLocked ? lockImg : unlockImg;
        audioSrc.clip = lockSound;
    }

    private void OnTriggerEnter(Collider _)
    {
        r.material.color = Color.white;
        ++colliderCount;
    }

    private void OnTriggerExit(Collider _)
    {
        r.material.color = Color.gray;
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
                
                foreach (var btn in panelButtons)
                {
                    btn.SetActive(!isLocked);
                }
                actionTaken = true;

                timerRing.color = Color.cyan;
                statusImg.texture = isLocked ? lockImg : unlockImg;

                audioSrc.clip = isLocked ? lockSound : unlockSound;
                audioSrc.Play();
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
