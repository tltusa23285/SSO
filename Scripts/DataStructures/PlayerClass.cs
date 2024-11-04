using Commands;
using Godot;

namespace Data
{
    [GlobalClass]
    public partial class PlayerClass : ActorStatBlock
    {
        [ExportGroup("Description")]
        [Export] public string ClassID;
        [Export(PropertyHint.MultilineText)] public string Description;

        [ExportGroup("Stats")]
        [Export] public string AttackStat = Stat.Str;

        public override int AttackPower => (GetStat(AttackStat, out int v) ? v : 1);
    }
}
