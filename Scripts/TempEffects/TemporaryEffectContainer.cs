using Actors;
using Godot;
using System;
using System.Collections.Generic;

namespace TemporaryEffects
{
	/// <summary>
	/// Manages temporary effects currently on the actor.
	/// </summary>
	public partial class TemporaryEffectContainer : Node
	{
		public delegate void EffectUpdated(TemporaryEffect eff);

		public EffectUpdated OnEffectAdded;
        public EffectUpdated OnEffectRemoved;
        public EffectUpdated OnEffectUpdate;


        [Export] private Actor Actor;

		private Dictionary<string, TemporaryEffect> Effects = new Dictionary<string, TemporaryEffect>();

		public void AddEffect(string id, in Actor source, TemporaryEffect effect)
		{
			if (Effects.ContainsKey(id))
			{
				Effects[id].ReApply();
				return;
			}
			Effects.Add(id, effect);
			effect.InitialApplication(source, Actor);
			OnEffectAdded?.Invoke(effect);
		}

		public void RemoveEffect(string id) 
		{
			if (!Effects.ContainsKey(id)) return;
			OnEffectRemoved?.Invoke(Effects[id]);
			Effects.Remove(id);
		}

        public override void _Process(double delta)
        {
			foreach (var item in Effects.Values)
			{
				item.UpdateTime((float)delta);
				OnEffectUpdate?.Invoke(item);
			}
        }
    } 
}
