using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Examples
{
    public class LightSwitch : NetworkBehaviour
    {
        [SerializeField]
        private Light[] lights;

        [SerializeField]
        private bool initialState = true;

        [SerializeField]
        private GameObject onView;

        [SerializeField]
        private GameObject offView;

        public NetworkVariable<bool> isOn = new NetworkVariable<bool>();

        public override void OnNetworkSpawn()
        {
            isOn.OnValueChanged += HandleValueChanged;

            if (IsServer)
                isOn.Value = initialState;

            else HandleValueChanged(default, isOn.Value);
        }

        public override void OnNetworkDespawn()
        {
            isOn.OnValueChanged -= HandleValueChanged;
        }

        public void Toggle()
        {
            isOn.Value = !isOn.Value;
        }

        private void HandleValueChanged(bool previous, bool current)
        {
            onView.SetActive(current);
            offView.SetActive(!current);

            foreach (var l in lights)
                l.enabled = current;
        }
    }

    public static class AnimationTools
    {
        public static IEnumerator PlayOneshot(this AnimationClip clip, GameObject target)
        {
            float elapsedTime = 0;

            while (elapsedTime < clip.length)
            {
                clip.SampleAnimation(target, elapsedTime);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            clip.SampleAnimation(target, clip.length);
        }
    }
}
