using Godot;
using Godot.Collections;

namespace Data
{
    [GlobalClass]
    public partial class StatBlock : Resource
    {
        [Export]
        public Dictionary StatList = new Dictionary()
    {
        { new StringName(Stat.Vit), 10},
        { new StringName(Stat.Str), 10},
        { new StringName(Stat.Dex), 10},
        { new StringName(Stat.Int), 10},
    };
    } 
}