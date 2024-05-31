[System.Serializable]
public class GameState
{
    public enum Section
    {
        VALLEY_INTRODUCTION,
        BERNICE_INTRODUCTION,
        CHARACTER_INTRODUCTION,
        SCOUT_INTROSPECTION,
        MINIGAME_1,
        SCOUT_BERNICE_CALL,
        MINIGAME_2,
        COOKING,
        END,
    }

    public Section section;
    public bool annie;
    public bool walter;
    public bool tyler;

    public GameState()
    {
        section = Section.VALLEY_INTRODUCTION;
        annie = false;
        walter = false;
        tyler = false;
    }
}
