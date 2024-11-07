using Microsoft.AspNetCore.Http.Features;
using Vehicle_World.Models;
using System.Linq;

namespace Vehicle_World.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();



            if (!context.MakeTypes.Any())
            {
                var makeTypes = new MakeType[]
                {
                    new MakeType { Name = "Toyota" },
                    new MakeType { Name = "Ford" },
                    new MakeType { Name = "Honda" },
                    new MakeType { Name = "Lamborghini" },
                    new MakeType { Name = "Chevrolet" },
                    new MakeType { Name = "BMW" },
                    new MakeType { Name = "Mercedes-Benz" },
                    new MakeType { Name = "Ferrari" },
                    new MakeType { Name = "Audi" },
                    new MakeType { Name = "Volkswagen" },
                    new MakeType { Name = "Nissan" },
                    new MakeType { Name = "Hyundai" },
                    new MakeType { Name = "Kia" },
                    new MakeType { Name = "Mazda" },
                    new MakeType { Name = "Porsche" },
                    new MakeType { Name = "Jaguar" },
                    new MakeType { Name = "Tesla" },
                    new MakeType { Name = "Mitsubishi" },
                };

                foreach (var mt in makeTypes)
                {
                    context.MakeTypes.Add(mt);
                }
                context.SaveChanges();
            }







            if (!context.ModelTypes.Any())
            {
                var modelTypes = new ModelType[]
                {
                    new ModelType { Name = "Mitsubishi Outlander" },
                    new ModelType { Name = "Toyota Corolla" },
                    new ModelType { Name = "Honda Civic" },
                    new ModelType { Name = "Toyota Camry" },
                    new ModelType { Name = "Honda Accord" },
                    new ModelType { Name = "Chevrolet Silverado" },
                    new ModelType { Name = "BMW 3 Series" },
                    new ModelType { Name = "Ford Mustang" },
                    new ModelType { Name = "Tesla Model 3" },
                    new ModelType { Name = "Nissan Altima" },
                    new ModelType { Name = "Mazda CX-5" },
                    new ModelType { Name = "Audi A4" },
                    new ModelType { Name = "Porsche 911" },
                    new ModelType { Name = "Jaguar F-Type" },
                    new ModelType { Name = "Mercedes-Benz E-Class" },
                    new ModelType { Name = "Mitsubishi Outlander" },
                    new ModelType { Name = "Ford Explorer" },
                    new ModelType { Name = "Chevrolet Equinox" },
                };

                foreach (var modt in modelTypes)
                {
                    context.ModelTypes.Add(modt);
                }
                context.SaveChanges();
            }


            if (!context.ConditionTypes.Any())
            {
                var conditionTypes = new ConditionType[]
                {
                    new ConditionType { Name = "New" },
                    new ConditionType { Name = "Used" },
                };

                foreach (var condt in conditionTypes)
                {
                    context.ConditionTypes.Add(condt);
                }
                context.SaveChanges();
            }


            // Initialize BodyTypes
            if (!context.BodyTypes.Any())
            {
                var bodyTypes = new BodyType[]
                {
                    new BodyType { Name = "Coupe" },
                    new BodyType { Name = "Convertible" },
                    new BodyType { Name = "Hatchback" },
                    new BodyType { Name = "Sedan" },
                    new BodyType { Name = "SUV" },
                    new BodyType { Name = "Sports car" },
                    new BodyType { Name = "Pickup truck" },
                    new BodyType { Name = "Wagon" },
                    new BodyType { Name = "Crossover" },
                    new BodyType { Name = "Utility Vehicle" },
                    new BodyType { Name = "Van" },
                    new BodyType { Name = "Estate" },
                    new BodyType { Name = "Minivan" },
                    new BodyType { Name = "MPV" },
                    new BodyType { Name = "Saloon" },
                    new BodyType { Name = "Jeep" },
                    new BodyType { Name = "Limousine" },
                    new BodyType { Name = "City car" },
                    new BodyType { Name = "Luxury" },
                    new BodyType { Name = "Roadster" }
                };

                foreach (var bt in bodyTypes)
                {
                    context.BodyTypes.Add(bt);
                }
                context.SaveChanges();
            }

            // Initialize EngineTypes
            if (!context.EngineTypes.Any())
            {
                var engineTypes = new EngineType[]
                {
                    new EngineType { Name = "V6" },
                    new EngineType { Name = "Inline" },
                    new EngineType { Name = "V Engine" },
                    new EngineType { Name = "Twin cylinder" },
                    new EngineType { Name = "Three-cylinder" },
                    new EngineType { Name = "Four-cylinder" },
                    new EngineType { Name = "Five-cylinder" },
                    new EngineType { Name = "Petrol engine" },
                    new EngineType { Name = "Diesel engine" },
                    new EngineType { Name = "Alternative fuel engines" }
                };

                foreach (var et in engineTypes)
                {
                    context.EngineTypes.Add(et);
                }
                context.SaveChanges();
            }

            // Initialize FuelTypes
            if (!context.FuelTypes.Any())
            {
                var fuelTypes = new FuelType[]
                {
                    new FuelType { Name = "Diesel" },
                    new FuelType { Name = "Gasoline" },
                    new FuelType { Name = "Biodiesel" },
                    new FuelType { Name = "Ethanol" },
                    new FuelType { Name = "Natural gas" },
                    new FuelType { Name = "Lpg" },
                    new FuelType { Name = "Hybrid" },
                    new FuelType { Name = "Hydrogen" },
                    new FuelType { Name = "Alternative fuels" },
                    new FuelType { Name = "Liquefied petroleum" },
                    new FuelType { Name = "Premium" },
                    new FuelType { Name = "Electric" }
                };

                foreach (var ft in fuelTypes)
                {
                    context.FuelTypes.Add(ft);
                }
                context.SaveChanges();
            }

            // Initialize TransmissionTypes
            if (!context.TransmissionTypes.Any())
            {
                var transmissionTypes = new TransmissionType[]
                {
                    new TransmissionType { Name = "Manual" },
                    new TransmissionType { Name = "Automatic" },
                    new TransmissionType { Name = "CVT" }
                };

                foreach (var tt in transmissionTypes)
                {
                    context.TransmissionTypes.Add(tt);
                }
                context.SaveChanges();
            }





        }
    }
}


