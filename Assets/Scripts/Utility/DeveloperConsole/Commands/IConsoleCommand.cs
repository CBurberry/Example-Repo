using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.DeveloperConsole.Commands
{
    //Interface included for mocking / unit testing purposes
    public interface IConsoleCommand
    {
        string CommandWord { get; }

        bool Process(string[] args);
    }
}