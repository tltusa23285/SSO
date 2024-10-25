namespace StatModifiers
{
    public interface IStatModifier
    {
        public enum MOD_PRIORITY : byte
        {
            Flat            = 0,
            Additive        = 50,
            Multiplicative  = 100
        }
        public MOD_PRIORITY Type { get; }
        public void ModifyValue(ref int v);
    } 
}