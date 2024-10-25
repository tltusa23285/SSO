using Godot;

public partial class Health : Node
{
	[Signal]
	public delegate void HealthChangeEventHandler(int current, int max);

    public int CurrentHealth { get; private set; }
	[Export] public int MaxHealth;
	[Export] private Actors.Actor Source;

	public void Setup(in int max)
	{
		MaxHealth = max;
        CurrentHealth = MaxHealth;
        EmitSignal(SignalName.HealthChange, CurrentHealth, MaxHealth);
    }

    public void Damage(int amount)
	{
		CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, MaxHealth);
		DamageNumberSpawner.Instance.Spawn(this.Source, amount);
		EmitSignal(SignalName.HealthChange, CurrentHealth, MaxHealth);
    }

	public void Heal(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        EmitSignal(SignalName.HealthChange, CurrentHealth, MaxHealth);
	}
}