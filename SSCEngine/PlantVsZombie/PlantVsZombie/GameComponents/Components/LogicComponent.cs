using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.Utils.GameObject.Component;
using Microsoft.Xna.Framework;
using PlantVsZombie.GameComponents.Behaviors;

namespace PlantVsZombie.GameComponents.Components
{
    public class LogicComponent : IComponent<MessageType>
    {
        public int Health { get; set; }

        private BaseLogicBehavior _logicBehavior = null;
        public BaseLogicBehavior LogicBehavior
        {
            get { return _logicBehavior;}
            set
            {
                _logicBehavior = value;
                _logicBehavior.Owner = this;
            }
        }

        public LogicComponent()
        {
            Health = 100;
        }

        public IEntity<MessageType> Owner
        {
            get;
            set;
        }

        public void OnMessage(IMessage<MessageType> message, GameTime gameTime)
        {
            switch (message.MessageType)
            {
                case MessageType.FRAME_UPDATE:
                    if (LogicBehavior != null)
                        LogicBehavior.Update(message, gameTime);
                    break;
                case MessageType.COLLISON_DETECT:
                    if (LogicBehavior != null)
                        LogicBehavior.OnCollison(message, gameTime);
                    break;
                default:
                    break;
            }
        }
    }
}
