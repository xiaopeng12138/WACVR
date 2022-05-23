using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlLocker : MonoBehaviour
{
    private int colliderCount = 0;
    private bool isLocked = false;

    [SerializeField]
    private RawImage statusImg;
    [SerializeField]
    private Texture lockImg;
    [SerializeField]
    private Texture unlockImg;
    [SerializeField]
    private List<ControlPanel> cpanButtons;

    private void Start()
    {
        statusImg.texture = unlockImg;
    }

    private void OnTriggerEnter(Collider _)
    {
        Debug.Log("Lock btn pressed.");
        colliderCount++;
        if (colliderCount == 1)
        {
            // set state var
            isLocked = !isLocked;

            // set img
            statusImg.texture = isLocked ? lockImg : unlockImg;

            // set buttons' state
            foreach (var btn in cpanButtons)
            {
                btn.GetComponent<Collider>().enabled = !isLocked;
            }
        }
    }

    private void OnTriggerExit(Collider _)
    {
        Debug.Log("Lock btn unpressed.");
        colliderCount--;
    }
}
