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
    }
}
