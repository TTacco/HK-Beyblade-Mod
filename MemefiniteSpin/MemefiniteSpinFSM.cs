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
            On.HealthManager.TakeDamage += (orig, self, instance) =>
            {
                RemoveCycloneKnockback(orig, self, instance);
            };
            activatedAlready = false;
            StartCoroutine(InitializeFSM());
        }

        public void OnDestroy()
        {
            ModHooks.Instance.AfterAttackHook -= Attack;
            On.HealthManager.TakeDamage -= (orig, self, instance) =>
            {
                RemoveCycloneKnockback(orig, self, instance);
            };
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

                nailArtFSM.InsertAction("Inactive", new CallMethod
                {
                    behaviour = GameManager.instance.GetComponent<MemefiniteSpinFSM>(),
                    methodName = "Test",
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

        public void Test()
        {
            Modding.Logger.Log("Inactive Reached");
        }

        //Gets called every frame

        public void RemoveCycloneKnockback(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance instance)
        {
            if (instance.Source.transform.parent.parent.gameObject.name == "Cyclone Slash")
            {
                var recoil = self.GetAttr<Recoil>("recoil");
                self.SetAttr<Recoil>("recoil", null);
                orig(self, instance);
                self.SetAttr("recoil", recoil);
                return;
            }
            orig(self, instance);
        }

    }       
}
