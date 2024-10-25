using Actors;
using Commands;
using Godot;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Reference collection of actions
    /// </summary>
    [GlobalClass]
    public partial class ActionBook : Resource
    {
        [Export] public BarAction DefaultAttack { get; set; }
        [Export] private BarAction[] Commands;

        public IEnumerable<BarAction> Actions
        {
            get
            {
                if(Commands == null) Commands = new BarAction[0];
                foreach (BarAction command in Commands) { yield return command; }
            }
        }

        public void LoadBook(Actor source)
        {
            DefaultAttack.Initialize(source);
            foreach (BarAction command in Actions)
            {
                command.Initialize(source);
            }
        }
    } 
}