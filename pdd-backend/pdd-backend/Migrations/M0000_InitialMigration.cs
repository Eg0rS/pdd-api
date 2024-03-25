using FluentMigrator;
using FluentMigrator.Snowflake;

namespace pdd_backend.Migrations;

[Migration(1)]
public class M0000_InitialMigration: Migration
{
    public override void Up()
    {
        Create.Table("requests")
            .WithColumn("id").AsInt64().Identity().NotNullable().PrimaryKey()
            .WithColumn("mac_address").AsString().NotNullable()
            .WithColumn("created_at").AsDateTimeOffset().NotNullable()
            .WithColumn("longitude").AsDouble().NotNullable()
            .WithColumn("latitude").AsDouble().NotNullable()
            .WithColumn("address").AsString().NotNullable()
            .WithColumn("file_id").AsString().NotNullable();
        Create.Table("resolutions")
            .WithColumn("id").AsInt64().Identity().NotNullable().PrimaryKey()
            .WithColumn("request_id").AsInt64().NotNullable().ForeignKey("requests", "id")
            .WithColumn("file_id").AsString().NotNullable()
            .WithColumn("is_violation").AsBoolean().NotNullable()
            .WithColumn("violations").AsString()
            .WithColumn("plate").AsString();
    }

    public override void Down()
    {
        Delete.Table("requests");
        Delete.Table("resolutions");
    }
}
