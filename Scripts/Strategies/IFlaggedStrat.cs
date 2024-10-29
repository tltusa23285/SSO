using Commands.Combat;
using Godot;
using System;

namespace Strategies
{
	public interface IFlaggedStrat
	{
		public void SetupFlagCondition(CombatAction action);
	} 
}
