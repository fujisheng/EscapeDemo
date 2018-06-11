using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Json;

public class NpcManager {

    static NpcManager instance;

    List<Npc> allNpc = new List<Npc>();

    public static NpcManager GetInstance(){
        if (instance == null)
            instance = new NpcManager();
        return instance;
    }

    public void Init(){
        allNpc = JsonFile.ReadFromFile<JsonList<Npc>>("Text/", "npcInfo").ToList();
    }
}
