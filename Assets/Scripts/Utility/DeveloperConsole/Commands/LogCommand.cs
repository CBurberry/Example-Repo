using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.DeveloperConsole.Commands
{
    [CreateAssetMenu(fileName = "New Log Command", menuName = "Utilities/DevConsole/LogCommand")]
    public class LogCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            string logText = string.Join(" ", args);
            Debug.Log(logText);
            return true;
        }
    }
}

