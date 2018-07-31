using Modding;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using GlobalEnums;
using System.Collections;
using ModCommon.Util;
using System;

namespace MemefiniteSpin
{

    class MemefiniteSpinFSM : MonoBehaviour
    {
        PlayMakerFSM nailArtFSM;
        bool activatedAlready = false;

        public void Start()
        {
            ModHooks.Instance.AfterAttackHook += Attack;
            activatedAlready = false;
            StartCoroutine(InitializeFSM());
        }

        private IEnumerator InitializeFSM()
        {
            while (GameManager.instance == null || HeroController.instance == null)
            {
                yield return null;
            }

            FSMInit();
        }

        public void FSMInit()
        {
            nailArtFSM = HeroController.instance.gameObject.LocateMyFSM("Nail Arts");
            InitializeCycloneFSM();
            Modding.Logger.Log("Memefinite Spin Activated");
        }

        public void InitializeCycloneFSM()
        {
            try
            {
                nailArtFSM.InsertAction("Cyclone Spin", new CallMethod
                {
                    behaviour = GameManager.instance.GetComponent<MemefiniteSpinFSM>(),
                    methodName = "OnCycloneSpin",
                    parameters = new FsmVar[0],
                    everyFrame = false
                }, 0);

                nailArtFSM.InsertAction("Flash 2", new CallMethod
                {
                    behaviour = GameManager.instance.GetComponent<MemefiniteSpinFSM>(),
                    methodName = "GSlashFlash",
                    parameters = new FsmVar[0],
                    everyFrame = false
                }, 0);
            }
            catch (Exception e)
            {
                Modding.Logger.Log("Error initializing Cyclone " + e);
            }
        }

        public void Attack(AttackDirection at)
        {
            if (activatedAlready)
            {
                nailArtFSM.SetState("Cancel All");
                activatedAlready = false;
            }
        }

        public void OnCycloneSpin()
        {
            nailArtFSM.SetState("Cyc Send Msg");
            activatedAlready = true;
        }

        public void GSlashFlash()
        {
            nailArtFSM.SetState("Flash");
        }

    }
}
