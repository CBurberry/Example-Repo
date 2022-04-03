using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utility.DeveloperConsole.Commands;

namespace Utility.DeveloperConsole
{
    //In a game that requires keyboard focus, the input handling of this class should be readdressed
    public class DeveloperConsoleBehaviour : MonoBehaviour
    {
        [SerializeField] string prefix = string.Empty;
        [SerializeField] ConsoleCommand[] commands = new ConsoleCommand[0];

        [Header("UI")]
        [SerializeField] GameObject uiCanvas = null;
        [SerializeField] TMP_InputField inputField = null;

        [Header("Input")]
        [SerializeField] KeyCode consoleToggleKey = KeyCode.BackQuote;
        [SerializeField] KeyCode consoleInputKey = KeyCode.Return;

        private float pausedTimeScale;
        private static DeveloperConsoleBehaviour instance;
        private DeveloperConsole developerConsole;

        public KeyCode ConsoleToggleKey => consoleToggleKey;
        public KeyCode ConsoleInputKey => consoleInputKey;
        public bool IsOpen => uiCanvas.activeSelf;

        private DeveloperConsole DeveloperConsole
        {
            get 
            {
                if (developerConsole != null) 
                {
                    return developerConsole;
                }
                return developerConsole = new DeveloperConsole(prefix, commands);
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this) 
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        //Doing the old inputsystem implementation that doesn't use input contexts
        //Its a bit messier that way but implementation on new context is here if required
        //https://youtu.be/usShGWFLvUk?t=784
        public void Toggle() 
        {
            if (uiCanvas.activeSelf)
            {
                Time.timeScale = pausedTimeScale;
                inputField.text = string.Empty;
                uiCanvas.SetActive(false);
            }
            else 
            {
                pausedTimeScale = Time.timeScale;
                Time.timeScale = 0;
                uiCanvas.SetActive(true);
                inputField.ActivateInputField();
            }
        }

        public void ProcessCommand()
        {
            DeveloperConsole.ProcessCommand(inputField.text);
            inputField.text = string.Empty;
        }
    }
}