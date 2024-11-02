using System;

public interface Beverage
{
    double Cost();
    string GetDescription();
}


public class Espresso : Beverage
{
    public double Cost() => 2.00;
    public string GetDescription() => "Espresso";
}

public class Tea : Beverage
{
    public double Cost() => 1.50;
    public string GetDescription() => "Tea";
}

public class Latte : Beverage
{
    public double Cost() => 3.00;
    public string GetDescription() => "Latte";
}

public class Mocha : Beverage
{
    public double Cost() => 3.50;
    public string GetDescription() => "Mocha";
}


public abstract class BeverageDecorator : Beverage
{
    protected Beverage _beverage;

    protected BeverageDecorator(Beverage beverage)
    {
        _beverage = beverage;
    }

    public virtual double Cost() => _beverage.Cost();
    public virtual string GetDescription() => _beverage.GetDescription();
}


public class Milk : BeverageDecorator
{
    public Milk(Beverage beverage) : base(beverage) { }

    public override double Cost() => _beverage.Cost() + 0.50;
    public override string GetDescription() => _beverage.GetDescription() + ", Milk";
}

public class Sugar : BeverageDecorator
{
    public Sugar(Beverage beverage) : base(beverage) { }

    public override double Cost() => _beverage.Cost() + 0.20;
    public override string GetDescription() => _beverage.GetDescription() + ", Sugar";
}

public class WhippedCream : BeverageDecorator
{
    public WhippedCream(Beverage beverage) : base(beverage) { }

    public override double Cost() => _beverage.Cost() + 0.70;
    public override string GetDescription() => _beverage.GetDescription() + ", Whipped Cream";
}


class Program
{
    static void Main(string[] args)
    {
        // Базовый напиток
        Beverage myDrink = new Espresso();
        Console.WriteLine($"{myDrink.GetDescription()} : ${myDrink.Cost()}");

        // Добавляем молоко
        myDrink = new Milk(myDrink);
        Console.WriteLine($"{myDrink.GetDescription()} : ${myDrink.Cost()}");

        // Добавляем сахар
        myDrink = new Sugar(myDrink);
        Console.WriteLine($"{myDrink.GetDescription()} : ${myDrink.Cost()}");

        // Добавляем взбитые сливки
        myDrink = new WhippedCream(myDrink);
        Console.WriteLine($"{myDrink.GetDescription()} : ${myDrink.Cost()}");
    }
}


public interface IPaymentProcessor
{
    void ProcessPayment(double amount);
}

public class PayPalPaymentProcessor : IPaymentProcessor
{
    public void ProcessPayment(double amount)
    {
        Console.WriteLine($"Processing payment of {amount} via PayPal.");
    }
}

public class StripePaymentService
{
    public void MakeTransaction(double totalAmount)
    {
        Console.WriteLine($"Making transaction of {totalAmount} via Stripe.");
    }
}

public class StripePaymentAdapter : IPaymentProcessor
{
    private StripePaymentService _stripeService;

    public StripePaymentAdapter(StripePaymentService stripeService)
    {
        _stripeService = stripeService;
    }

    public void ProcessPayment(double amount)
    {
        _stripeService.MakeTransaction(amount);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Beverage myDrink = new Espresso();
        Console.WriteLine($"{myDrink.GetDescription()} : ${myDrink.Cost()}");

        myDrink = new Milk(myDrink);
        Console.WriteLine($"{myDrink.GetDescription()} : ${myDrink.Cost()}");

        myDrink = new Sugar(myDrink);
        Console.WriteLine($"{myDrink.GetDescription()} : ${myDrink.Cost()}");

        myDrink = new WhippedCream(myDrink);
        Console.WriteLine($"{myDrink.GetDescription()} : ${myDrink.Cost()}");
        
        IPaymentProcessor paypalProcessor = new PayPalPaymentProcessor();
        paypalProcessor.ProcessPayment(100.0);

        StripePaymentService stripeService = new StripePaymentService();
        IPaymentProcessor stripeAdapter = new StripePaymentAdapter(stripeService);
        stripeAdapter.ProcessPayment(200.0);
    }
}
