
// KEVIN: Artifact script from when my plan for the map involved an interface with different scripts for each house. It doesn't
// have a purpose aside from holding a character enum since putting that in another script felt weird, so until I get to figuring
// that out this'll be kept.
public interface IHouse
{
    void Click();

    void setActiveStatus(bool s);
}

public enum Character {
    Annie, Scout, Tyler, Walter
}
