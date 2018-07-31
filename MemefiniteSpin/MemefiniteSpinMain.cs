using Modding;
using System.Reflection;

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
            ModHooks.Instance.CharmUpdateHook += ChargeTimer;
        }

        public void SaveGame(SaveGameData sv)
        {
            GameManager.instance.gameObject.AddComponent<MemefiniteSpinFSM>(); 
        }

        public void NewGame()
        {
            GameManager.instance.gameObject.AddComponent<MemefiniteSpinFSM>();
        }

        public void ChargeTimer(PlayerData data, HeroController controller)
        {
            HeroController.instance.NAIL_CHARGE_TIME_DEFAULT = .2f;
            HeroController.instance.NAIL_CHARGE_TIME_CHARM = .2f;
        }
    }
}
