using EntityStates;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace Cloudburst.Enemies.EntityStatez.ArchaicWispMonster
{
    public class Summon : BaseState
    {
        public GameObject effectPrefab;
        public float baseDuration = 1f;
        private float duration;
        public string muzzleLeft = "MuzzleLeft";
        public string muzzleRight = "MuzzleRight";
        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            this.duration = this.baseDuration / this.attackSpeedStat;
            if (this.effectPrefab)
            {

                EffectManager.SimpleMuzzleFlash(this.effectPrefab, base.gameObject, muzzleLeft, false);
                EffectManager.SimpleMuzzleFlash(this.effectPrefab, base.gameObject, muzzleRight, false);
            }
            base.PlayAnimation("Gesture", "FireCannons", "FireCannons.playbackRate", this.duration);
            if (base.isAuthority && base.modelLocator && base.modelLocator.modelTransform)
            {

                if (base.modelLocator.modelTransform.GetComponent<ChildLocator>())
                {
                    ChildLocator childLocator = base.modelLocator.modelTransform.GetComponent<ChildLocator>();
                    //literally compiler names but who will look at this code anyways?
                    int muzzleLeftShildIndex = childLocator.FindChildIndex(muzzleLeft);
                    int muzzleRightChildIndex = childLocator.FindChildIndex(muzzleRight);
                    Transform muzzleLeftTransform = childLocator.FindChild(muzzleLeftShildIndex);
                    Transform muzzleRightTransform = childLocator.FindChild(muzzleRightChildIndex);
                    if (muzzleLeftTransform)
                    {
                        CharacterMaster characterMaster;
                        characterMaster = new MasterSummon
                        {
                            masterPrefab = MasterCatalog.FindMasterPrefab("WispMaster"),
                            position = muzzleLeftTransform.position,
                            rotation = muzzleLeftTransform.rotation,
                            summonerBodyObject = null,
                            ignoreTeamMemberLimit = true,
                            teamIndexOverride = TeamIndex.Monster

                        }.Perform();


                        AIOwnership component4 = characterMaster.gameObject.GetComponent<AIOwnership>();
                        BaseAI component5 = characterMaster.gameObject.GetComponent<BaseAI>();

                        if (component4 && characterBody.master)
                        {
                            component4.ownerMaster = base.characterBody.master;
                        }
                        if (component5 && characterBody.master.gameObject)
                        {
                            component5.leader.gameObject = base.characterBody.master.gameObject;
                            component5.isHealer = false;
                            component5.fullVision = true;
                        }
                    }
                    if (muzzleRightTransform)
                    {
                        CharacterMaster characterMaster;
                        characterMaster = new MasterSummon
                        {
                            masterPrefab = MasterCatalog.FindMasterPrefab("WispMaster"),
                            position = muzzleRightTransform.position,
                            rotation = muzzleRightTransform.rotation,
                            summonerBodyObject = null,
                            ignoreTeamMemberLimit = true,
                            teamIndexOverride = TeamIndex.Monster

                        }.Perform();


                        AIOwnership component4 = characterMaster.gameObject.GetComponent<AIOwnership>();
                        BaseAI component5 = characterMaster.gameObject.GetComponent<BaseAI>();

                        if (component4 && characterBody.master)
                        {
                            component4.ownerMaster = base.characterBody.master;
                        }
                        if (component5 && characterBody.master.gameObject)
                        {
                            component5.leader.gameObject = base.characterBody.master.gameObject;
                            component5.isHealer = false;
                            component5.fullVision = true;
                        }
                    }
                }
            }
        }
        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}