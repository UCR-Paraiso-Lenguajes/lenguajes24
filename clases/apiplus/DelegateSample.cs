using System;

public class Foo
{
    public void FooFunction()
    {
        Console.WriteLine("Foo function is called.");
    }
}

public class Goo
{
    public void Process(Action action)
    {
        action(); // invoking the delegate function passed as parameter
    }
}