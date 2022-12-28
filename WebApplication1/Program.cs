using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json;
using WebApplication1;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;


var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddControllers().AddNewtonsoftJson(x =>
//     x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddDbContext<ApplicationContext>();

var app = builder.Build();

//ApplicationContext db = new ApplicationContext();

app.MapGet("/api/Car/{Id}", (ApplicationContext db, string id) =>
{
    using (db)
    {
        Car? ans = db.Cars.Find(int.Parse(id));
        if (ans != null)
        {
           return Results.Json(ans); 
        }
         else
         {
             return Results.NotFound();
         }
    }
});

app.MapGet("/api/Cars", (ApplicationContext db) =>
{
    using (db)
    {
        List<Car> temp = new List<Car>();
        temp = db.Cars.ToList();
        if (temp.Count!=0)
        {
                   return Results.Json(temp);
        }
        else
        {
            return Results.NoContent();
        }
    }
});

app.MapPost("/api/Car", (ApplicationContext db, Car data) =>
{
    using (db)
    {
        Car? duplicate = db.Cars.FirstOrDefault(u=>u.Name==data.Name);
        if (duplicate == null)
        {

            db.Cars.Add(data);
            db.SaveChanges();
            return Results.Ok();
        }
        else
        {
            return Results.NoContent();
        }
    }
});

app.MapPut("/api/Car", (ApplicationContext db, Car data) =>
{
    using (db)
    {
        Car? temp = db.Cars.Find(data.Id);
        if (temp == null)
        {
            return Results.NotFound();
        }
        else
        {
            temp.Name=data.Name;
            temp.SitCounter = data.SitCounter;
            db.Cars.Update(temp);
            db.SaveChanges();
            return Results.Ok();
        }
    }
});

app.MapDelete("/api/Car/{Id}", (ApplicationContext db, string id) =>
{
    using (db)
    {
        var car = db.Cars.Attach(new Car { Id = int.Parse(id) });
        if (car != null)
        {
            car.State = EntityState.Deleted;
            db.SaveChanges();
            return Results.Ok(); 
        }
        else
        {
            return Results.NotFound();
        }
    }
});

app.MapGet("/api/TaxiDepot/{Id}", (ApplicationContext db, string id) =>
{
    using (db)
    {
        TaxiDepot? taxiDepot = db.TaxiDepots.Find(int.Parse(id));
        if (taxiDepot != null)
        {
            db.TaxiGroups.Where(u => u.TaxiDepotId == taxiDepot.Id).Load();
            List<TaxiGroup> temp = taxiDepot.TaxiGroups.ToList();
            List<TaxiGroupView> taxiGroupViews = new List<TaxiGroupView>();
            foreach (var VARIABLE in temp)
            {
               db.Cars.Where(u=>u.Id==VARIABLE.CarId).Load(); 
            }
            for (int i = 0; i < temp.Count; i++)
            {
                TaxiGroupView taxiGroupView = new TaxiGroupView()
                {
                    TaxiDepotId = temp[i].TaxiDepotId,
                    Car = temp[i].Car,
                    Id = temp[i].Id,
                    Quantity = temp[i].Quantity,
                    CarId = temp[i].CarId,
                };
                taxiGroupViews.Add(taxiGroupView);
            }

            TaxiDepotView ans = new TaxiDepotView()
            {
                Id = taxiDepot.Id,
                Address = taxiDepot.Address, 
                TaxiGroupViews = taxiGroupViews
            };
            return Results.Json(ans);
        }
        else
        {
            return Results.NotFound();
        }
    }
});

app.MapGet("/api/TaxiDepots", (ApplicationContext db) =>
{
    using (db)
    {
        List<TaxiDepot> data = new List<TaxiDepot>();
        data = db.TaxiDepots.Include(x => x.TaxiGroups).ThenInclude(u => u.Car).ToList();
        // data = db.TaxiDepots.ToList();
        // foreach (var VARIABLE in data)
        // {
        //     db.TaxiGroups.Load();
        // }
        if (data.Count!=0)
        {
            List<TaxiDepotView> ansdata = new List<TaxiDepotView>();
            foreach (var taxiDepot in data)
            {
                List<TaxiGroup> temp = taxiDepot.TaxiGroups.ToList();
                List<TaxiGroupView> taxiGroupViews = new List<TaxiGroupView>();
                for (int i = 0; i < temp.Count; i++)
                {
                    TaxiGroupView taxiGroupView = new TaxiGroupView()
                    {
                        TaxiDepotId = temp[i].TaxiDepotId,
                        Car = temp[i].Car,
                        Id = temp[i].Id,
                        Quantity = temp[i].Quantity,
                        CarId = temp[i].CarId,
                    };
                    taxiGroupViews.Add(taxiGroupView);
                }

                TaxiDepotView ans = new TaxiDepotView()
                {
                    Id = taxiDepot.Id,
                    Address = taxiDepot.Address, 
                    TaxiGroupViews = taxiGroupViews
                };
                ansdata.Add(ans);
            }
            return Results.Json(ansdata);
        }
        else
        {
            return Results.NoContent();
        }
    }
});

app.MapPost("api/TaxiDepot", (ApplicationContext db, TaxiDepotView data) =>
{
    using (db)
    {
        TaxiDepot? duplicate = db.TaxiDepots.FirstOrDefault(u=>u.Address==data.Address);
        if (duplicate == null)
        {
            TaxiDepot temp = new TaxiDepot() {Id = data.Id, Address = data.Address};
            db.TaxiDepots.Add(temp);
            db.SaveChangesAsync();
            List<TaxiGroup> taxigrouptemp = new List<TaxiGroup>();
            foreach (var VARIABLE in data.TaxiGroupViews)
            {
                TaxiGroup taxiGroup = new TaxiGroup()
                    {   TaxiDepot = temp, 
                        Quantity = VARIABLE.Quantity,
                        CarId = VARIABLE.CarId,
                        Car = VARIABLE.Car
                    };
                taxigrouptemp.Add(taxiGroup);
            }

            temp.TaxiGroups = taxigrouptemp;
            db.TaxiGroups.AddRange(temp.TaxiGroups);
            db.TaxiDepots.Update(temp);
            //db.TaxiDepots.Add(temp);
            //db.TaxiGroups.AddRange(temp.TaxiGroups);
            db.SaveChanges();
            return Results.Ok();
        }
        else
        {
            return Results.NoContent();
        }
    }
});

app.MapPut("/api/TaxiDepot", (ApplicationContext db, TaxiDepotView data) =>
{
    using (db)
    {
        TaxiDepot? taxidepot = db.TaxiDepots.Include(x => x.TaxiGroups)
            .ThenInclude(u => u.Car)
            .FirstOrDefault(x => x.Id == data.Id);
        // TaxiDepot? taxidepot = db.TaxiDepots.Find(data.Id);
        taxidepot.Address = data.Address;
        // db.TaxiGroups.Where(u => u.TaxiDepotId == taxidepot.Id).Load();
        List<TaxiGroup> oldtaxigroups = taxidepot.TaxiGroups.ToList();
        //List<TaxiGroupView> newparts = data.TaxiGroups.ToList();
        List<TaxiGroup> TaxigroupsToAdd = new List<TaxiGroup>();
        List<TaxiGroup> TaxiGroupsToDelete = new List<TaxiGroup>();
        List<TaxiGroup> TaxiGroupsToUpdate = new List<TaxiGroup>();
        for (int i = 0; i < data.TaxiGroupViews.Count; i++)
        {
            if (oldtaxigroups.Exists(u => u.Car.Name == data.TaxiGroupViews[i].Car.Name))
            {
                TaxiGroup taxiGroupOld = oldtaxigroups[oldtaxigroups.FindIndex(u => u.Car.Name == data.TaxiGroupViews[i].Car.Name)];
                taxiGroupOld.Quantity = data.TaxiGroupViews[i].Quantity;
                TaxiGroupsToUpdate.Add(taxiGroupOld);
            }
            else if (!oldtaxigroups.Exists(u => u.Car.Name == data.TaxiGroupViews[i].Car.Name))
            {
                TaxiGroup parttemp = new TaxiGroup()
                {
                    TaxiDepot = taxidepot,
                    TaxiDepotId = taxidepot.Id,
                    Car = db.Cars.FirstOrDefault(u=>u.Name==data.TaxiGroupViews[i].Car.Name),
                    CarId = db.Cars.FirstOrDefault(u=>u.Name==data.TaxiGroupViews[i].Car.Name).Id,
                    Quantity = data.TaxiGroupViews[i].Quantity
                };
                TaxigroupsToAdd.Add(parttemp);
            }
        }

        foreach (var VARIABLE in oldtaxigroups)
        {
            if (!data.TaxiGroupViews.Exists(u=>u.Car.Name==VARIABLE.Car.Name))
            {
                TaxiGroupsToDelete.Add(VARIABLE);
            }
        }
        
        if (TaxiGroupsToUpdate.Count!=0)
        {
            oldtaxigroups = TaxiGroupsToUpdate;
            db.TaxiGroups.UpdateRange(oldtaxigroups);
        }
        
        if (TaxiGroupsToDelete.Count!=0)
        {
            db.TaxiGroups.RemoveRange(TaxiGroupsToDelete);
        }

        if (TaxigroupsToAdd.Count!=0)
        {
            db.TaxiGroups.AddRange(TaxigroupsToAdd);
        }

        db.TaxiDepots.Update(taxidepot);
        db.SaveChanges();
    }
});


app.MapDelete("/api/TaxiDepot/{Id}", (ApplicationContext db, string id) =>
{
    using (db)
    {
        TaxiDepot? taxidepot = db.TaxiDepots.FirstOrDefault(u=>u.Id==int.Parse(id));
        db.TaxiGroups.Where(u=>u.TaxiDepotId==taxidepot.Id).Load();
        if (taxidepot != null)
        {
            db.TaxiDepots.Remove(taxidepot);
            db.SaveChanges();
            return Results.Ok(); 
        }
        else
        {
            return Results.NotFound();
        }
    }
});

app.Run();

public class TaxiDepotView
{
    public string? Address { get; set; }
    public int Id { get; set; }
    public List<TaxiGroupView> TaxiGroupViews { get; set; } = new List<TaxiGroupView>();
}

public class TaxiGroupView
{
    public int Id { get; set; }
    public int TaxiDepotId { get; set; }
    public int CarId { get; set; }
    public Car Car { get; set; } = null!;
    public int Quantity { get; set; }
}