using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using Fungus;

[CommandInfo("Flow",
                 "ExitToMenu",
                 "Exit to main menu scene.")]
public class ExitToMenu : Command {

    public override void OnEnter()
    {
        SceneManager.LoadScene("MainMenu 1");
    }
}
