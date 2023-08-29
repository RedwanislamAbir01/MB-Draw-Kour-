using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Game
{
    public class MuteSettings : MonoBehaviour
    {
        public Button muteOnButton;
        public Button muteOffButton;

        private void Start()
        {


            // Set initial state
            AudioListener.pause = false; // Sounds are on
            muteOnButton.gameObject.SetActive(true); // Mute on button is active
            muteOffButton.gameObject.SetActive(false);// Mute off button is inactive
        }

        public void MuteOn()
        {
            AudioListener.pause = true; // Mute sounds
            muteOnButton.gameObject.SetActive(false); // Mute on button is inactive
            muteOffButton.gameObject.SetActive(true); // Mute off button is active
        }

        public void MuteOff()
        {
            AudioListener.pause = false; // Unmute sounds
            muteOnButton.gameObject.SetActive(true); // Mute on button is active
            muteOffButton.gameObject.SetActive(false);// Mute off button is inactive
        }
    }
}
