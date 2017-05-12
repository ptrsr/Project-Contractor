using System.Collections;
using System.Collections.Generic;

public abstract class FishState
{
    protected Fish fish;

    public FishState(Fish pFish)
    {
        fish = pFish;
    }

    public abstract void Initialize();
    public abstract void Step();
}