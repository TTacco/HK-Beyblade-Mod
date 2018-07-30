using Modding;
using UnityEngine;
using UnityEngine.UI;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using GlobalEnums;
using Modding.Menu;
using System.Collections;
using ModCommon.Util;
using System;

namespace MemefiniteSpin
{

    //Comment Test
    //More Test
    //Wew Lad
    //Test
    //More comment test

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

                nailArtFSM.InsertAction("Inactive", new CallMethod
                {
                    behaviour = GameManager.instance.GetComponent<MemefiniteSpinFSM>(),
                    methodName = "OnInactive",
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
           /* if (activatedAlready)
            {
                nailArtFSM.SetState("Cyclone End");
                activatedAlready = false;
            }
            */
        }

        //CommentTest
        
        public void test()
        {

        }

        public void OnCycloneSpin()
        {
            nailArtFSM.SetState("Cyc Send Msg");
            activatedAlready = true;
        }

        public void FixedUpdate()
        {
            if (!MemefiniteSpinMain.stateIsWaiting)
            {
                nailArtFSM.SetState("Inactive");
                if (activatedAlready)
                {
                    activatedAlready = false;
                }
            }
        }


        public void OnInactive()
        {

        }

    }
}
