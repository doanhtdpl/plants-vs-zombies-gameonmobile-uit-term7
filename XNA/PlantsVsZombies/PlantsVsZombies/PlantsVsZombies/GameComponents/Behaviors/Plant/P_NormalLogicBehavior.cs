using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SCSEngine.Utils.GameObject.Component;
using PlantsVsZombies.GameComponents.GameMessages;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GameCore;
using SCSEngine.Services;
using PlantsVsZombies.GameObjects;
using SCSEngine.Serialization;
using SCSEngine.Audio;

namespace PlantsVsZombies.GameComponents.Behaviors.Plant
{
    enum eNormalPlantState
    {
        STANDING, SHOOTING
    }
    public class P_NormalLogicBehavior : BaseLogicBehavior
    {
        private static TimeSpan _lastTimeSound;
        private static TimeSpan _timeDelaySound = TimeSpan.FromSeconds(1);
        private bool justCollect = false;

        eNormalPlantState PlantState { get; set; }
        TimeSpan currentTimeShoot = TimeSpan.Zero;
        TimeSpan shootTime = new TimeSpan(0, 0, 0, 0, 500);
        Vector2 shootPoint = new Vector2(90, 45);

        Sound _soundShoot;

        public P_NormalLogicBehavior()
            : base()
        {
            _soundShoot = SCSServices.Instance.ResourceManager.GetResource<Sound>("Sounds/PeaShot");
        }

        public Double DShootTime
        {
            get { return shootTime.TotalSeconds; }
            set { shootTime = TimeSpan.FromSeconds(value); }
        }

        public override void Update(IMessage<MessageType> msg, GameTime gameTime)
        {
            PlantState = eNormalPlantState.STANDING;
            // Sound
            if (justCollect)
            {
                if (gameTime.TotalGameTime - _lastTimeSound > _timeDelaySound)
                {
                    SCSServices.Instance.AudioManager.PlaySound(_soundShoot, false, true);
                    _lastTimeSound = gameTime.TotalGameTime;
                }
            }
            justCollect = false;
            // Shoot
            IDictionary<ulong, ObjectEntity> objs = new Dictionary<ulong, ObjectEntity>(PZObjectManager.Instance.GetObjects());
            foreach (var item in objs)
            {
                if (item.Value == this.Owner)
                    continue;
                MoveComponent obj1 = this.Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;
                MoveComponent obj2 = item.Value.GetComponent(typeof(MoveComponent)) as MoveComponent;

                if (obj2.Position.Y == obj1.Position.Y && (obj1.Position.X < obj2.Position.X) && obj2.Position.X < SCSServices.Instance.Game.GraphicsDevice.Viewport.Width
                    && (obj2.Owner as ObjectEntity).ObjectType == eObjectType.ZOMBIE)
                {
                    // Change to shoot
                    //RenderBehaviorChangeMsg renderMsg = new RenderBehaviorChangeMsg(MessageType.CHANGE_RENDER_BEHAVIOR, this);
                    //renderMsg.RenderBehaviorType = GameComponents.Components.eMoveRenderBehaviorType.PL_SHOOTING;
                    //renderMsg.DestinationObjectId = this.Owner.Owner.ObjectId;
                    
                    //PZObjectManager.Instance.SendMessage(renderMsg, gameTime);
                    PlantState = eNormalPlantState.SHOOTING;
                }
            }
            if (PlantState == eNormalPlantState.SHOOTING)
            {
                if (currentTimeShoot > shootTime)
                {
                    Vector2 pos = (this.Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent).Position;
                    ObjectEntity bullet = PZObjectFactory.Instance.createNormalBullet(new Vector2(pos.X + shootPoint.X, pos.Y - shootPoint.Y));
                    //bullet.SetPosition();
                    PZObjectManager.Instance.AddObject(bullet);
                    //SCSServices.Instance.AudioManager.PlaySound(_soundShoot, false, true);
                    justCollect = true;
                    currentTimeShoot = TimeSpan.Zero;
                }
                else
                    currentTimeShoot += gameTime.ElapsedGameTime;
            }

            // Die
            LogicComponent logicCOm = this.Owner as LogicComponent;
            if (logicCOm == null)
                throw new Exception("PL_NormalLogicBehavior: Expect Logic Component");
            if (logicCOm.Health < 0)
            {
                PZObjectManager.Instance.RemoveObject(this.Owner.Owner.ObjectId);
            }
            base.Update(msg, gameTime);
        }

        public override void OnCollison(IMessage<MessageType> msg, GameTime gameTime)
        {
            CollisionDetectedMsg message = msg as CollisionDetectedMsg;
            if (msg == null)
                throw new Exception("PL_NormalLogicBehavior: message is not CollisionDetectedMsg");

            base.OnCollison(msg, gameTime);
        }

        public override IBehavior<MessageType> Clone()
        {

            var clone = new P_NormalLogicBehavior();

            clone.DShootTime = this.DShootTime;
            return clone;
        }


        public override void Deserialize(IDeserializer deserializer)
        {
            // CODE HERE
            shootTime = TimeSpan.FromSeconds(deserializer.DeserializeDouble("TimeShoot"));
        }
    }
}
