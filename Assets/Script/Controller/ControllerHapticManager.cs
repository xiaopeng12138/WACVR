using UnityEngine.UI;
using UnityEngine;
using UnityEngine.XR;
public class ControllerHapticManager : MonoBehaviour
{
    public XRNode Hand;
    InputDevice device;
    public float duration = 0.1f;
    public float amplitude = 1f;
    void Start()
    {
        var durationWidget = ConfigManager.GetConfigPanelWidget("HapticDuration");
        var amplitudeWidget = ConfigManager.GetConfigPanelWidget("HapticAmplitude");
        var durationSlider = durationWidget.GetComponent<Slider>();
        var amplitudeSlider = amplitudeWidget.GetComponent<Slider>();
        durationSlider.onValueChanged.AddListener( (float value) => { duration = value;});
        amplitudeSlider.onValueChanged.AddListener( (float value) => { amplitude = value;});
        durationSlider.onValueChanged?.Invoke(duration);
        amplitudeSlider.onValueChanged?.Invoke(amplitude);
    }
    private void OnTriggerEnter(Collider other)
    {
        device = InputDevices.GetDeviceAtXRNode(Hand);
        device.SendHapticImpulse(0, amplitude, duration);
    }
    private void OnTriggerExit(Collider other)
    {
        device = InputDevices.GetDeviceAtXRNode(Hand);
        device.StopHaptics();
    }
}
