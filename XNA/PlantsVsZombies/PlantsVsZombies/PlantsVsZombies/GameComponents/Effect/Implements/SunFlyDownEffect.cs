﻿using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents.Behaviors;
using PlantsVsZombies.GameComponents.Behaviors.Implements;
using PlantsVsZombies.GameComponents.Components;
using SCSEngine.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameComponents.Effect.Implements
{
    class SunFlyDownEffect : IEffect
    {
        TimeSpan curentTime = TimeSpan.Zero;
        public TimeSpan TimeDurring { get; set; }

        Vector2 v0 = new Vector2(0, 50);
        Vector2 a = new Vector2(0, 0);

        public SunFlyDownEffect()
        {
            double time = GRandom.RandomDouble(3, 6);
            ////Debug.WriteLine(time);
            TimeDurring = TimeSpan.FromSeconds(time);
        }

        public void Update(GameTime gameTime)
        {
            if (curentTime >= TimeDurring)
            {
                // Remove itseft
                MoveComponent moveCOm = this.Owner.Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;
                if (moveCOm == null)
                    throw new Exception("SlowMoveEffect: Expect Target Move Component");

                MoveBehavior moveBehavior = (moveCOm.GetCurrentBehavior() as MoveBehavior);
                moveBehavior.Velocity = Vector2.Zero;

                this.Owner.RemoveEffect(this);
            }
            else
            {
                // Take effect
                MoveComponent moveCOm = this.Owner.Owner.Owner.GetComponent(typeof(MoveComponent)) as MoveComponent;
                if (moveCOm == null)
                    throw new Exception("SlowMoveEffect: Expect Target Move Component");

                Vector2 v = v0;

                MoveBehavior moveBehavior = (moveCOm.GetCurrentBehavior() as MoveBehavior);
                moveBehavior.Velocity = v;
                curentTime += gameTime.ElapsedGameTime;
            }
        }


        private BaseLogicBehavior _owner = null;
        public BaseLogicBehavior Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
            }
        }
    }
}
