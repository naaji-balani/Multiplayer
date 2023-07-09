using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class SliderSync : NetworkBehaviour
{
    [SerializeField]
    private float sliderValue;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsHost)
        {
            // Add a listener to the slider's OnValueChanged event
            slider.onValueChanged.AddListener(OnLocalSliderValueChanged);
            slider.interactable = true;
        }
        else
        {
            slider.interactable = false;
        }
    }

    private void OnLocalSliderValueChanged(float value)
    {
        // Update the local slider value
        sliderValue = value;

        // Call a server-side method to notify other clients about the value change
        UpdateSliderValueServerRpc(value);
    }

    private void OnSliderValueChanged(float oldValue, float newValue)
    {
        // Update the slider value on remote clients
        slider.value = newValue;
    }

    [ServerRpc]
    private void UpdateSliderValueServerRpc(float value)
    {
        // Update the slider value on the server
        sliderValue = value;

        // Update the slider value on remote clients
        UpdateSliderValueClientRpc(value);
    }

    [ClientRpc]
    private void UpdateSliderValueClientRpc(float value)
    {
        // Update the slider value on remote clients
        if (!IsOwner)
        {
            sliderValue = value;
            slider.value = value;
        }
    }
}
