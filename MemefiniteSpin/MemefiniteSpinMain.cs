﻿using Modding;
using System.Reflection;
using GlobalEnums;
using UnityEngine;
using ModCommon;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace MemefiniteSpin
{
    public class MemefiniteSpinMain : Mod
    {
        public override string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static bool stateIsWaiting;

        public override void Initialize()
        {
            ModHooks.Instance.AfterSavegameLoadHook += SaveGame;
            ModHooks.Instance.NewGameHook += NewGame;
            ModHooks.Instance.HeroUpdateHook += GetState;
        }

        public void SaveGame(SaveGameData sv)
        {
            GameManager.instance.gameObject.AddComponent<MemefiniteSpinFSM>(); 
        }

        public void NewGame()
        {
            GameManager.instance.gameObject.AddComponent<MemefiniteSpinFSM>();
        }

        public void GetState()
        {
            stateIsWaiting = HeroController.instance.transitionState.ToString().Contains("WAITING");
        }
    }
}
