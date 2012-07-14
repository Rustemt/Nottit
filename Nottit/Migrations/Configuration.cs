namespace Nottit.Migrations {
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Nottit.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Nottit.Models.NottitDb> {
        public Configuration() {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Nottit.Models.NottitDb context) {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Users.AddOrUpdate(new User {
                Id = 1,
                UserName = "Alexis",
                Password = "password",
            },
            new User {
                Id = 2,
                UserName = "Steve",
                Password = "password"
            });
        }
    }
}
