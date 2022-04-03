using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.DeveloperConsole.Commands
{
    public abstract class ConsoleCommand : ScriptableObject, IConsoleCommand
    {
        [SerializeField] string commandWord = string.Empty;

        public string CommandWord => commandWord;

        public abstract bool Process(string[] args);
    }
}
