using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class LocomotionToggle : MonoBehaviour
{
    [System.Serializable]
    public class LocomotionToggleEvent : UnityEvent<bool> { }

    public static bool IsEnabled
    {
        get { return _state; }
    }
    private static bool _state = true;

    private float timer = 0;
    private bool actionDone = false;

    private bool leftHeld = false;
    private bool rightHeld = false;

    public LocomotionToggleEvent locoEvent;

    private AudioClip soundOn;
    private AudioClip soundOff;
    private AudioSource audioSrc;

    [Header("Settings")]
    [SerializeField]
    private float holdTime;

    [Header("References")]
    [SerializeField]
    private GameObject locomotionController;
    [SerializeField]
    private InputActionProperty leftHandAction;
    [SerializeField]
    private InputActionProperty rightHandAction;


    private void Start()
    {
        if (locoEvent == null)
            locoEvent = new LocomotionToggleEvent();

        audioSrc = GetComponent<AudioSource>();
        soundOn = Resources.Load<AudioClip>("Audio/loco on");
        soundOff = Resources.Load<AudioClip>("Audio/loco off");

        leftHandAction.action.Enable();
        rightHandAction.action.Enable();

        leftHandAction.action.started +=
            (InputAction.CallbackContext _) => leftHeld = true;
        leftHandAction.action.canceled +=
            (InputAction.CallbackContext _) => leftHeld = false;
        rightHandAction.action.started +=
            (InputAction.CallbackContext _) => rightHeld = true;
        rightHandAction.action.canceled +=
            (InputAction.CallbackContext _) => rightHeld = false;

        locomotionController.SetActive(_state);
    }

    private void Update()
    {
        if (leftHeld && rightHeld)
        {
            timer += Time.unscaledDeltaTime;
            if (timer >= holdTime && !actionDone)
            {
                _state = !_state;
                locoEvent.Invoke(_state);
                locomotionController.SetActive(_state);
                actionDone = true;

                audioSrc.clip = _state ? soundOn : soundOff;
                audioSrc.Play();
            }
        }
        else
        {
            timer = 0;
            actionDone = false;
        }
    }
}
