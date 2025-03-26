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
                    .WithColumn("last_name").AsString(100).NotNullable()
                    .WithColumn("first_name").AsString(100).NotNullable()
                    .WithColumn("email").AsString(100).NotNullable();

            Create.Table("budgets")
                        .WithColumn("id").AsInt32().PrimaryKey().Identity()
                        .WithColumn("name").AsString(100).NotNullable()
                        .WithColumn("start_date").AsDateTime().NotNullable()
                        .WithColumn("finish_date").AsDateTime().Nullable()
                        .WithColumn("description").AsString(255).Nullable()
                        .WithColumn("creator_id").AsInt32().ForeignKey("users", "id").NotNullable();

            Create.Table("budgetrecords")
                            .WithColumn("id").AsInt32().PrimaryKey().Identity()
                            .WithColumn("name").AsString(100).NotNullable()
                            .WithColumn("creation_date").AsDateTime().NotNullable()
                            .WithColumn("spending_date").AsDateTime().NotNullable()
                            .WithColumn("budget_id").AsInt32().ForeignKey("budgets", "id").NotNullable()
                            .WithColumn("total").AsDecimal().WithDefaultValue(0.0)
                            .WithColumn("comment").AsString(255).Nullable();

            Create.Table("budgetresults")
                            .WithColumn("id").AsInt32().PrimaryKey().Identity()
                            .WithColumn("budget_id").AsInt32().ForeignKey("budgets", "id").NotNullable()
                            .WithColumn("total_profit").AsDecimal().WithDefaultValue(0.0);

            Insert.IntoTable("users")
                .Row(new { 
                    first_name = "John", 
                    last_name = "Rambo", 
                    email = "jrjr@mail.ru" 
                });
            Insert.IntoTable("budgets")
                .Row(new { 
                    name = "FromWageToWage", 
                    start_date = new DateTime(2012, 12, 21), 
                    finish_date = DateTime.Now, 
                    creator_id = 1
                });
            Insert.IntoTable("budgetrecords")
                .Row(new
                {
                    name = "TV",
                    creation_date = new DateTime(2013, 12, 21),
                    spending_date = new DateTime(2013, 12, 22),
                    budget_id = 1,
                    total = 2000.0,
                    comment = "mynewtv"
                });
            Insert.IntoTable("budgetrecords")
                .Row(new 
                {
                    name = "PC",
                    creation_date = new DateTime(2014, 5, 3),
                    spending_date = new DateTime(2014, 5, 4),
                    budget_id = 1,
                    total = 13500.0,
                    comment = "mynewPC"
                });
            Insert.IntoTable("budgetresults")
                .Row(new
                {
                    budget_id = 1,
                    total_profit = 15500.0
                });
        }

        public override void Down()
        {
            Delete.Table("budgetrecords");
            Delete.Table("budgetresults");
            Delete.Table("budgets");
            Delete.Table("users");
        }
    }
}