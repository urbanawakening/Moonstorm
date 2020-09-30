using R2API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Moonstorm.Cores
{
    class ShitpostCore
    {
        public ShitpostCore() => Shitpost(); //shitpost
        public void Shitpost() {
            LogCore.LogI("Initializing Core: " + base.ToString());

            BrickWallMithrix(); }

        protected void BrickWallMithrix()
        {
            LanguageAPI.Add("BROTHER_BODY_NAME", "DON BRIDGE");
            LanguageAPI.Add("BROTHER_BODY_SUBTITLE", "DON BRIDGE");

            var body = Resources.Load<GameObject>("prefabs/characterbodies/BrotherBody");
            var child = body.transform.Find("ModelBase/mdlBrother/BrotherHammerConcrete");
            var renderer = child.GetComponent<Renderer>();
            renderer.material = UnityEngine.Object.Instantiate<Material>(AssetsCore.brickWall);
        }
    }
}
