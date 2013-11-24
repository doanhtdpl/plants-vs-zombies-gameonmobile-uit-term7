using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SCSEngine.Utils.GameObject.Component;
using PlantsVsZombies.GameComponents.GameMessages;
using PlantsVsZombies.GameObjects;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GameCore;
using PlantsVsZombies.GameObjects.Implements;

namespace PlantsVsZombies.GameComponents.Behaviors.Bullet
{
    public class B_NormalLogicBehavior : BaseLogicBehavior
    {
        public override void Update(IMessage<MessageType> message, GameTime gameTime)
        {

            base.Update(message, gameTime);
        }

        public override void OnCollison(IMessage<MessageType> msg, GameTime gameTime)
        {
            CollisionDetectedMsg message = msg as CollisionDetectedMsg;
            if (msg == null)
                throw new Exception("B_NormalLogicBehavior: message is not CollisionDetectedMsg");

            NormalZombie zom = message.TargetCollision as NormalZombie;
            // Send message change to 
            if (zom != null)
            {
                LogicComponent logicCOm = zom.GetComponent(typeof(LogicComponent)) as LogicComponent;
                if (logicCOm == null)
                    throw new Exception("B_NormalLogicBehavior: Expect Target Logic Component");
                logicCOm.Health-=10;
                PZObjectManager.Instance.RemoveObject(Owner.Owner.ObjectId);
            }

            base.OnCollison(msg, gameTime);
        }
    }
}
