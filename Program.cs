using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ParkingSession
{
    public DateTime EntryTime { get; }
    public DateTime ExitTime { get; }

    public ParkingSession(string entry, string exit)
    {
        EntryTime = DateTime.Parse(entry);
        ExitTime = DateTime.Parse(exit);

        if (ExitTime <= EntryTime)
            throw new ArgumentException("Exit time must be after entry time.");
    }

    public int GetHoursRoundedUp()
    {
        var totalHours = (ExitTime - EntryTime).TotalHours;
        return (int)Math.Ceiling(totalHours);
    }

    public decimal CalculateHourlyFee(decimal hourlyRate)
    {
        return hourlyRate * GetHoursRoundedUp();
    }

    public decimal CalculateFlatFee(decimal flatRate)
    {
        return flatRate;
    }

    public decimal CalculateProgressiveFee(decimal firstHourRate, decimal subsequentHourRate)
    {
        int hours = GetHoursRoundedUp();
        if (hours <= 1)
            return firstHourRate;
        return firstHourRate + (hours - 1) * subsequentHourRate;
    }

    public (string PlanName, decimal Fee) GetCheapestPlan(
        decimal hourlyRate,
        decimal flatRate,
        decimal firstHourRate,
        decimal subsequentHourRate)
    {
        var hourly = CalculateHourlyFee(hourlyRate);
        var flat = CalculateFlatFee(flatRate);
        var progressive = CalculateProgressiveFee(firstHourRate, subsequentHourRate);

        (string Plan, decimal Fee)[] plans = {
            ("Hourly", hourly),
            ("Flat", flat),
            ("Progressive", progressive)
        };

        var cheapest = plans[0];
        foreach (var plan in plans)
        {
            if (plan.Fee < cheapest.Fee)
                cheapest = plan;
        }

        return cheapest;
    }
}

namespace _25__Parking_Lot_Fee_Optimizer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var session = new ParkingSession("2025-08-14 08:15", "2025-08-14 11:45");

           
            decimal hourlyRate = 50m;               
            decimal flatRate = 200m;                
            decimal firstHourRate = 60m;            
            decimal subsequentHourRate = 40m;       

            Console.WriteLine($"Entry: {session.EntryTime}");
            Console.WriteLine($"Exit:  {session.ExitTime}");
            Console.WriteLine($"Hours (rounded up): {session.GetHoursRoundedUp()}");

            var cheapest = session.GetCheapestPlan(hourlyRate, flatRate, firstHourRate, subsequentHourRate);
            Console.WriteLine($"\nCheapest Plan: {cheapest.PlanName} → Fee: {cheapest.Fee:C}");
        }
    }
}




