using System;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Moonstorm.Cores.Components
{
    public class WinchControl : MonoBehaviour
    {

        public Transform tailTransform;
        public string attachmentString = "WinchHole";
        private ProjectileGhostController projectileGhostController;
        private Transform attachmentTransform;

        private void Start()
        {
            this.attachmentTransform = this.FindAttachmentTransform();
            if (this.attachmentTransform)
            {
                this.tailTransform.position = this.attachmentTransform.position;
            }
        }

        private void Update()
        {
            if (!this.attachmentTransform)
            {
                this.attachmentTransform = this.FindAttachmentTransform();
            }
            if (this.attachmentTransform)
            {
                //LogCore.LogI("uhhhhhh");
                this.tailTransform.position = this.attachmentTransform.position;
            }
        }

        private Transform FindAttachmentTransform()
        {
            this.projectileGhostController = base.GetComponentInParent<ProjectileGhostController>();
            if (this.projectileGhostController)
            {
                Transform authorityTransform = this.projectileGhostController.authorityTransform;
                if (authorityTransform)
                {
                    ProjectileController component = authorityTransform.GetComponent<ProjectileController>();
                    if (component)
                    {
                        GameObject owner = component.owner;
                        if (owner)
                        {
                            ModelLocator component2 = owner.GetComponent<ModelLocator>();
                            if (component2)
                            {
                                Transform modelTransform = component2.modelTransform;
                                if (modelTransform)
                                {
                                    ChildLocator component3 = modelTransform.GetComponent<ChildLocator>();
                                    if (component3)
                                    {
                                        return component3.FindChild("WinchHole");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
