using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.ScreenManagement;

namespace PlantsVsZombies.GameScreen.ScreenFactory
{
    public class TestScreenFactory : IGameScreenFactory
    {
        IGameScreenManager manager;

        public TestScreenFactory(IGameScreenManager screenManager)
        {
            this.manager = screenManager;
        }

        public IGameScreen CreateGameScreen()
        {
            return new GamePlayScreen(this.manager);
        }
    }
}
