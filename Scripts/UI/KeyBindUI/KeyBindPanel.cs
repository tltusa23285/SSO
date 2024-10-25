using Godot;
using Inputs;
using System.Collections.Generic;

public partial class KeyBindPanel : Control
{
    [Export] private Container GroupParent;
    [Export] private Node GroupPrefab;
    [Export] private PackedScene LinePrefab;

    private Dictionary<string, Node> TabGroups = new Dictionary<string, Node>();

    public override void _Ready()
    {
        foreach (var item in KeyMap.GetSystemInputs()) AddBinding("System", item);
        foreach (var item in KeyMap.GetGeneralInputs()) AddBinding("General", item);
        foreach (var item in KeyMap.GetHotbarInputs()) AddBinding("Hotbars", item);
        
        AddBinding("General", "Jump");
    }

    private void AddBinding(in string group, in string input)
    {
        if (!TabGroups.ContainsKey(group))
        {
            Node ng = GroupPrefab.Duplicate();
            ng.Name = group;
            GroupParent.AddChild(ng);
            TabGroups.Add(group, ng);
        }
        KeyBindLine line = LinePrefab.Instantiate<KeyBindLine>();
        TabGroups[group].FindChild("LineParent",false,false).AddChild(line);
        line.Init(input);
    }
}
