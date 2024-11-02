using System;

public interface IReport
{
    string Generate();
}

public class SalesReport : IReport
{
    public string Generate()
    {
        return "Sales Report Data";
    }
}

public class UserReport : IReport
{
    public string Generate()
    {
        return "User Report Data";
    }
}


public abstract class ReportDecorator : IReport
{
    protected IReport _report;

    protected ReportDecorator(IReport report)
    {
        _report = report;
    }

    public virtual string Generate() => _report.Generate();
}


public class DateFilterDecorator : ReportDecorator
{
    private readonly DateTime _startDate;
    private readonly DateTime _endDate;

    public DateFilterDecorator(IReport report, DateTime startDate, DateTime endDate)
        : base(report)
    {
        _startDate = startDate;
        _endDate = endDate;
    }

    public override string Generate()
    {
        return $"{_report.Generate()} with date filter from {_startDate.ToShortDateString()} to {_endDate.ToShortDateString()}";
    }
}

public class SortingDecorator : ReportDecorator
{
    private readonly string _sortCriteria;

    public SortingDecorator(IReport report, string sortCriteria) : base(report)
    {
        _sortCriteria = sortCriteria;
    }

    public override string Generate()
    {
        return $"{_report.Generate()} sorted by {_sortCriteria}";
    }
}

public class CsvExportDecorator : ReportDecorator
{
    public CsvExportDecorator(IReport report) : base(report) { }

    public override string Generate()
    {
        return $"{_report.Generate()} in CSV format";
    }
}

public class PdfExportDecorator : ReportDecorator
{
    public PdfExportDecorator(IReport report) : base(report) { }

    public override string Generate()
    {
        return $"{_report.Generate()} in PDF format";
    }
}


public interface IInternalDeliveryService
{
    void DeliverOrder(string orderId);
    string GetDeliveryStatus(string orderId);
}

public class InternalDeliveryService : IInternalDeliveryService
{
    public void DeliverOrder(string orderId)
    {
        Console.WriteLine($"Delivering order {orderId} via internal service.");
    }

    public string GetDeliveryStatus(string orderId)
    {
        return $"Status of order {orderId} in internal service.";
    }
}


public class ExternalLogisticsServiceA
{
    public void ShipItem(int itemId)
    {
        Console.WriteLine($"Shipping item {itemId} via External Logistics A.");
    }

    public string TrackShipment(int shipmentId)
    {
        return $"Tracking shipment {shipmentId} via External Logistics A.";
    }
}

public class ExternalLogisticsServiceB
{
    public void SendPackage(string packageInfo)
    {
        Console.WriteLine($"Sending package {packageInfo} via External Logistics B.");
    }

    public string CheckPackageStatus(string trackingCode)
    {
        return $"Status of package {trackingCode} via External Logistics B.";
    }
}


public class LogisticsAdapterA : IInternalDeliveryService
{
    private readonly ExternalLogisticsServiceA _externalService;

    public LogisticsAdapterA(ExternalLogisticsServiceA externalService)
    {
        _externalService = externalService;
    }

    public void DeliverOrder(string orderId)
    {
        int itemId = int.Parse(orderId);
        _externalService.ShipItem(itemId);
    }

    public string GetDeliveryStatus(string orderId)
    {
        int shipmentId = int.Parse(orderId);
        return _externalService.TrackShipment(shipmentId);
    }
}

public class LogisticsAdapterB : IInternalDeliveryService
{
    private readonly ExternalLogisticsServiceB _externalService;

    public LogisticsAdapterB(ExternalLogisticsServiceB externalService)
    {
        _externalService = externalService;
    }

    public void DeliverOrder(string orderId)
    {
        _externalService.SendPackage(orderId);
    }

    public string GetDeliveryStatus(string orderId)
    {
        return _externalService.CheckPackageStatus(orderId);
    }
}


public class DeliveryServiceFactory
{
    public static IInternalDeliveryService GetDeliveryService(string serviceType)
    {
        return serviceType switch
        {
            "Internal" => new InternalDeliveryService(),
            "ExternalA" => new LogisticsAdapterA(new ExternalLogisticsServiceA()),
            "ExternalB" => new LogisticsAdapterB(new ExternalLogisticsServiceB()),
            _ => throw new ArgumentException("Invalid service type")
        };
    }
}


class Program
{
    static void Main(string[] args)
    {
        IReport report = new SalesReport();

        report = new DateFilterDecorator(report, DateTime.Now.AddDays(-7), DateTime.Now);

        report = new SortingDecorator(report, "Date");

        report = new CsvExportDecorator(report);

        Console.WriteLine(report.Generate())

        IInternalDeliveryService deliveryService = DeliveryServiceFactory.GetDeliveryService("ExternalA");

        deliveryService.DeliverOrder("123");
        Console.WriteLine(deliveryService.GetDeliveryStatus("123"));

        deliveryService = DeliveryServiceFactory.GetDeliveryService("ExternalB");

        deliveryService.DeliverOrder("456");
        Console.WriteLine(deliveryService.GetDeliveryStatus("456"));
    }
}
