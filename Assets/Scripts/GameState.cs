using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

[System.Serializable]
public class GameState
{
    [Header("Endings")]
    public bool WalterGoodEnding;
    public bool AnnieGoodEnding;
    public bool TylerGoodEnding;

    [Header("Journal Day 2")]
    public bool WalterBlamesHimself;
    public bool WalterBlamesTyler;
    public bool WalterBlamesEveryone;
    public bool TylerFrustratedWithWalter;
    public bool TylerWantsDistance;
    public bool TylerWantsIndependence;
    public bool AnnieMissesCity;
    public bool AnnieMovesForward;
    public bool AnnieBreakForReal;

    [Header("Unsorted")]
    public bool KnowBerniceLiveTime;
    public bool PackedLightly;
    public bool MetWalter = false;
    public bool MetAnnie = false;
    public bool MetTyler = false;
    public bool WalterSafePath = false;
    public bool WalterRockyPath = false;
    public bool AnnieWalterSlowDown = false;
    public bool TylerPranked = false;
    public bool TylerMessy = false;
    public bool AnnieRangDoorbell = false;
    public bool AnnieJoked = false;
    public bool TylerVeggiesToBernice = false;
    public bool WalterCares = false;
    public bool WalterSolitude = false;
    public bool WalterConflicted = false;
    public bool WalterFullStory = false;
    public bool WalterFish = false;
    public bool AnnieBrushOff = false;
    public bool AnnieConnections = false;
    public bool AnnieFullStory = false;
    public bool AnnieBreak = false;
    public bool AnnieSelf = false;
    public bool TylerCool = false;
    public bool TylerTryhard = false;
    public bool TylerFindSelf = false;
    public bool AnnieRaces = false;
    public bool AnnieTakesABreak = false;
    public bool TylerIsCool = false;
    public bool TylerKeepBike = false;
    public bool TylerKeepArt = false;
    public bool TylerKeepGrill = false;
    public bool WalterFishForTyler = false;
    public bool WalterLakesidePhoto = false;
    public bool WalterTalkedAboutLakesidePhoto = false;
    public bool WalterTalkedAboutTylerFishing = false;
    public bool TylerStory = false;
    public bool AnnieWalterBreak = false;
    public bool WalterGoodEndingPush;

    [Header("Map day info")]
    [Range(1, 3)]
    public int DayNumber = 1;
    public bool AnnieVisited;
    public bool WalterVisited;
    public bool TylerVisited;
    public bool BerniceIntro;

    public GameState()
    {
        
    }
}
