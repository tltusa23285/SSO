using Commands.Combat;
using Godot;
using System;

namespace Strategies
{
	[GlobalClass]
	public abstract partial class FlaggedStratResource : Resource, IFlaggedStrat
	{
		protected CombatAction Action;
        public void SetupFlagCondition(CombatAction action)
		{
			Action = action;
			SetCondition();
		}

		protected abstract void SetCondition();
    } 
}
