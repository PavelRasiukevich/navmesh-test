using System;

public static class EventManager
{
    public static Action ChangeArrowToSword;
    public static Action ChangeSwordToArrow;

    public static void CallChangeCursorToSword()
    {
        ChangeArrowToSword?.Invoke();
    }

    public static void CallChangeCursorToArrow()
    {
        ChangeSwordToArrow?.Invoke();
    }
}
