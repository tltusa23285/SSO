using Godot;
using System;
using Actors;


namespace TemporaryEffects
{
	public enum TEMPEFF_TYPE
	{
		Other,  // Generally informational or neutral, ie a resource for combat abilities
		Buff,	// Generally positive effects, ie +10% str
		Debuff, // Generally negatve effects, ie -10% str
	}

	public enum TEMPEFF_VISIBILITY
	{
		Internal, // Non-player facing effects, ie a resource displayed in dedicated UI
		Visible,  // Player-facing, hoverable effects
		// Hidden, // Non-essential Visible-types that can be toggled, // TODO : TBD if useful
	}

	/// <summary>
	/// Base data-only class for temporary effects, non-local and passed to new instances
	/// </summary>
	[GlobalClass]
	public abstract partial class TemporaryEffectData : Resource
    {
		[ExportGroup("Display")]
		[Export] public string EffectId;
		[Export] public string DisplayName;
        [Export(PropertyHint.MultilineText)] public string FlavorText;
        [Export] public Texture2D Icon { get; set; } = null;

        [ExportGroup("Functional")]
        [Export] public TEMPEFF_TYPE Type = TEMPEFF_TYPE.Other;
        [Export] public TEMPEFF_VISIBILITY Visibility = TEMPEFF_VISIBILITY.Internal;
        [Export] public bool Removable = true;

        [ExportSubgroup("Duration")]
        [Export] public int InitialDuration = 10;
        [Export] public int MaxDuration = 10;
        [Export] public int TimeAddedOnRefresh = 999;


		public bool ApplyEffect(in Actor actor)
		{
			if (!CanApply(actor)) return false;
			actor.TempEffects.AddEffect(EffectId, GetEffect());
			return true;
		}

		protected abstract bool CanApply(in Actor actor);
		protected abstract TemporaryEffect GetEffect();

	}

	/// <summary>
	/// Base for buffs/debufs, etc.
	/// </summary>
	public abstract class TemporaryEffect
	{
		public TemporaryEffectData Data;

		public TemporaryEffect(TemporaryEffectData dat)
		{
			Data = dat;
		}

		protected Actor Target;
		public float Timer { get; protected set; }

		public void InitialApplication(Actor a)
		{
			Target = a;

			Timer = Data.MaxDuration;

			OnApply();
		}

		public void ReApply()
		{
			Timer = Mathf.Min(Data.MaxDuration, Timer + Data.TimeAddedOnRefresh);
			OnRefreshed();
		}

		public void Remove()
        {
			OnExpire();
            Target.TempEffects.RemoveEffect(this.Data.EffectId);
        }

		public void UpdateTime(float delta)
		{
			Timer -= delta;
			if (Timer <= 0)
			{
				Remove();
			}
		}

		/// <summary>
		/// When effect is initially applied
		/// </summary>
        protected abstract void OnApply();

		/// <summary>
		/// When effect fully expires, and no stacks left
		/// </summary>
		protected abstract void OnExpire();

		/// <summary>
		/// When an existing effect is reapplied, a stack added, etc;
		/// </summary>
		protected abstract void OnRefreshed();
	} 
}
