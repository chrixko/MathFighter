using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MathFighterXNA.Bang {
    public class ActionList : IAction {

        public List<IAction> Actions = new List<IAction>();

        private bool isBlocking { get; set; }

        bool IAction.IsBlocking() {
            return isBlocking;
        }

        bool IAction.IsComplete() {
            return Actions.Count == 0;
        }

        void IAction.Block() {
            isBlocking = true;
        }

        void IAction.Unblock() {
            isBlocking = false;
        }

        public void AddAction(IAction action, bool blocking) {
            if (blocking) {
                action.Block();
            } else {
                action.Unblock();
            }

            Actions.Add(action);
        }

        void IAction.Update(GameTime gameTime) {
            foreach (IAction action in Actions.ToArray<IAction>()) {
                action.Update(gameTime);

                if (action.IsComplete()) Actions.Remove(action);

                if (action.IsBlocking()) break;
            }            
        }

        void IAction.Complete() {
            throw new NotImplementedException("Not supported yet :)");
        }
    }
}
