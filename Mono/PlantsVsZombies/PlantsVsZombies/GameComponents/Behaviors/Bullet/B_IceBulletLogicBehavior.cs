using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Behaviors.Implements;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GameComponents.Effect.Implements;
using PlantsVsZombies.GameComponents.GameMessages;
using PlantsVsZombies.GameCore;
using PlantsVsZombies.GameObjects;
using SCSEngine.Serialization;
using SCSEngine.Utils.GameObject.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameComponents.Behaviors.Bullet
{
    class B_IceBulletLogicBehavior : BaseLogicBehavior
    {
        private float _damage = 9;
        public override void Update(IMessage<MessageType> message, GameTime gameTime)
        {

            base.Update(message, gameTime);
        }

        public override void OnCollison(IMessage<MessageType> msg, GameTime gameTime)
        {
            CollisionDetectedMsg message = msg as CollisionDetectedMsg;
            if (msg == null)
                throw new Exception("B_IceBulletLogicBehavior: message is not CollisionDetectedMsg");

            ObjectEntity zom = message.TargetCollision as ObjectEntity;
            // Send message change to 
            if (zom != null && zom.ObjectType == eObjectType.ZOMBIE)
            {
                LogicComponent logicCOm = zom.GetComponent(typeof(LogicComponent)) as LogicComponent;
                if (logicCOm == null)
                    throw new Exception("B_IceBulletLogicBehavior: Expect Target Logic Component");
                logicCOm.Health -= _damage;

                SlowMoveEffect slowEff = new SlowMoveEffect();
                slowEff.TimeDurring = TimeSpan.FromSeconds(2);

                logicCOm.LogicBehavior.AddEffect(slowEff);

                PZObjectManager.Instance.RemoveObject(Owner.Owner.ObjectId);
            }

            base.OnCollison(msg, gameTime);
        }

        public override IBehavior<MessageType> Clone()
        {
            B_IceBulletLogicBehavior clone = new B_IceBulletLogicBehavior();
            clone._damage = _damage;
            return clone;
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            // CODE HERE
            LogicComponent logicCOm = this.Owner as LogicComponent;
            if (logicCOm == null)
                throw new Exception("PL_NormalLogicBehavior: Expect Logic Component");
            _damage = deserializer.DeserializeInteger("Damage");
        }
    }
}
