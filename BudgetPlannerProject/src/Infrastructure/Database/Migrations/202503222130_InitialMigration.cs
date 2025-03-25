using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Migrations
{
    [Migration(202503222130)]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                    .WithColumn("id").AsInt32().PrimaryKey().Identity()
                    .WithColumn("lastname").AsString(100).NotNullable()
                    .WithColumn("firstname").AsString(100).NotNullable()
                    .WithColumn("email").AsString(100).NotNullable();

            Create.Table("budgets")
                        .WithColumn("id").AsInt32().PrimaryKey().Identity()
                        .WithColumn("name").AsString(100).NotNullable()
                        .WithColumn("startdate").AsDateTime().NotNullable()
                        .WithColumn("finishdate").AsDateTime().Nullable()
                        .WithColumn("description").AsString(255).Nullable()
                        .WithColumn("creatorid").AsInt32().ForeignKey("users", "id").NotNullable();

            Create.Table("budgetrecords")
                            .WithColumn("id").AsInt32().PrimaryKey().Identity()
                            .WithColumn("name").AsString(100).NotNullable()
                            .WithColumn("creationdate").AsDateTime().NotNullable()
                            .WithColumn("spendingdate").AsDateTime().NotNullable()
                            .WithColumn("budgetid").AsInt32().ForeignKey("budgets", "id").NotNullable()
                            .WithColumn("total").AsDecimal().WithDefaultValue(0.0)
                            .WithColumn("comment").AsString(255).Nullable();

            Create.Table("budgetresults")
                            .WithColumn("id").AsInt32().PrimaryKey().Identity()
                            .WithColumn("budgetid").AsInt32().ForeignKey("budgets", "id").NotNullable()
                            .WithColumn("totalprofit").AsDecimal().WithDefaultValue(0.0);

            Insert.IntoTable("users")
                .Row(new { firstname = "John", lastname = "Rambo", email = "jrjr@mail.ru" });
            Insert.IntoTable("budgets")
                .Row(new { 
                    name = "FromWageToWage", 
                    startdate = new DateTime(2012, 12, 21), 
                    finishdate = DateTime.Now, 
                    creatorid = 1
                });
            Insert.IntoTable("budgetrecords")
                .Row(new
                {
                    name = "TV",
                    creationdate = new DateTime(2013, 12, 21),
                    spendingdate = new DateTime(2013, 12, 22),
                    budgetid = 1,
                    total = 2000.0,
                    comment = "mynewtv"
                });
            Insert.IntoTable("budgetrecords")
                .Row(new 
                {
                    name = "PC",
                    creationdate = new DateTime(2014, 5, 3),
                    spendingdate = new DateTime(2014, 5, 4),
                    budgetid = 1,
                    total = 13500.0,
                    comment = "mynewPC"
                });
            Insert.IntoTable("budgetresults")
                .Row(new
                {
                    budgetid = 1,
                    totalprofit = 15500.0
                });
        }

        public override void Down()
        {
            Delete.Table("users");
            Delete.Table("budgets");
            Delete.Table("budgetrecrods");
            Delete.Table("budgetresults");
        }
    }
}