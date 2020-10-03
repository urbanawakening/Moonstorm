using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R2API;
using RoR2;
using UnityEngine;

namespace Starstorm2.Cores
{
    class TyphoonCore
    {
        public static DifficultyIndex diffIdxTyphoon;
        public const float difScale = 3.5f;
        public const string difName = "Typhoon";
        public const string difDesc =
            @"Maximum challenge. The world is a nightmare, and survival is merely an illusion. Nobody has what it takes.<style=cStack>

>Player health regeneration: <style=cIsHealth>-40%</style>
>Difficulty scaling: <style=cIsHealth>+75%</style></style>";
        public const string iconPath = "@Starstorm2:Assets/TyphoonIcon.png";
        public const string difServerTag = "ty";
        public Color difColor = new Color(0.71f, 0.25f, 0.50f);

        public TyphoonCore()
        {
            string nameToken = "DIFFICULTY_TYPHOON_NAME";
            string descToken = "DIFFICULTY_TYPHOON_DESCRIPTION";

            DifficultyDef typhoon = new DifficultyDef(difScale, nameToken, iconPath, descToken, difColor, difServerTag, true);

            LanguageAPI.Add(nameToken, difName);
            LanguageAPI.Add(descToken, difDesc);

            diffIdxTyphoon = DifficultyAPI.AddDifficulty(typhoon);

            Run.onRunStartGlobal += Run_onRunStartGlobal;
        }

        private void Run_onRunStartGlobal(Run run)
        {
            if (run.selectedDifficulty == diffIdxTyphoon)
            {
                foreach (CharacterMaster cm in run.userMasters.Values)
                {
                    cm.inventory.GiveItem(ItemIndex.MonsoonPlayerHelper);
                }
            }
        }
    }
}
