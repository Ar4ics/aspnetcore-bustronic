using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AspNetCoreBustronic.Models
{
    public partial class BustronicContext : DbContext
    {
        public BustronicContext()
        {
        }

        public BustronicContext(DbContextOptions<BustronicContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admins> Admins { get; set; }
        public virtual DbSet<BugReports> BugReports { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<CityVersions> CityVersions { get; set; }
        public virtual DbSet<Competitors> Competitors { get; set; }
        public virtual DbSet<DriverCompetitors> DriverCompetitors { get; set; }
        public virtual DbSet<DriverRoutes> DriverRoutes { get; set; }
        public virtual DbSet<DriverSmsCodes> DriverSmsCodes { get; set; }
        public virtual DbSet<Drivers> Drivers { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<LogRecords> LogRecords { get; set; }
        public virtual DbSet<Managers> Managers { get; set; }
        public virtual DbSet<Migrations> Migrations { get; set; }
        public virtual DbSet<MovingVehicles> MovingVehicles { get; set; }
        public virtual DbSet<OwnerCities> OwnerCities { get; set; }
        public virtual DbSet<OwnerRoutes> OwnerRoutes { get; set; }
        public virtual DbSet<Owners> Owners { get; set; }
        public virtual DbSet<PassengerDevices> PassengerDevices { get; set; }
        public virtual DbSet<PassengerSmsCodes> PassengerSmsCodes { get; set; }
        public virtual DbSet<Passengers> Passengers { get; set; }
        public virtual DbSet<PasswordResets> PasswordResets { get; set; }
        public virtual DbSet<Presets> Presets { get; set; }
        public virtual DbSet<RouteSegmentPaths> RouteSegmentPaths { get; set; }
        public virtual DbSet<RouteSegmentSpeeds> RouteSegmentSpeeds { get; set; }
        public virtual DbSet<RouteSegments> RouteSegments { get; set; }
        public virtual DbSet<RouteStations> RouteStations { get; set; }
        public virtual DbSet<Routes> Routes { get; set; }
        public virtual DbSet<SegmentPaths> SegmentPaths { get; set; }
        public virtual DbSet<Segments> Segments { get; set; }
        public virtual DbSet<StationPlatformVehicleTypes> StationPlatformVehicleTypes { get; set; }
        public virtual DbSet<StationPlatforms> StationPlatforms { get; set; }
        public virtual DbSet<Stations> Stations { get; set; }
        public virtual DbSet<TestLocationStats> TestLocationStats { get; set; }
        public virtual DbSet<TestSpeedStats> TestSpeedStats { get; set; }
        public virtual DbSet<TimeIntervals> TimeIntervals { get; set; }
        public virtual DbSet<UnverifiedVehicles> UnverifiedVehicles { get; set; }
        public virtual DbSet<VehicleDriverRoutes> VehicleDriverRoutes { get; set; }
        public virtual DbSet<VehicleDrivers> VehicleDrivers { get; set; }
        public virtual DbSet<VehicleKinds> VehicleKinds { get; set; }
        public virtual DbSet<VehicleOwnerRoutes> VehicleOwnerRoutes { get; set; }
        public virtual DbSet<VehicleSenders> VehicleSenders { get; set; }
        public virtual DbSet<VehicleTypes> VehicleTypes { get; set; }
        public virtual DbSet<Vehicles> Vehicles { get; set; }
        public virtual DbSet<VerifiedVehicles> VerifiedVehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Name=Postgres", x => x.UseNetTopologySuite());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pg_cron")
                .HasPostgresExtension("postgis");

            modelBuilder.Entity<Admins>(entity =>
            {
                entity.ToTable("admins");

                entity.HasIndex(e => e.Email)
                    .HasName("admins_email_unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(60);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");
            });

            modelBuilder.Entity<BugReports>(entity =>
            {
                entity.ToTable("bug_reports");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.IsSolved)
                    .HasColumnName("is_solved")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasMaxLength(255);

                entity.Property(e => e.Normalized)
                    .HasColumnName("normalized")
                    .HasColumnType("geography(Point,4326)");

                entity.Property(e => e.Real)
                    .HasColumnName("real")
                    .HasColumnType("geography(Point,4326)");

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.BugReports)
                    .HasForeignKey(d => d.RouteId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("bug_reports_route_id_foreign");
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.ToTable("cities");

                entity.HasIndex(e => e.IsAvailable)
                    .HasName("cities_is_available_index");

                entity.HasIndex(e => e.Location)
                    .HasName("cities_location_index")
                    .HasMethod("gist");

                entity.HasIndex(e => e.Title)
                    .HasName("cities_title_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.IsAvailable)
                    .IsRequired()
                    .HasColumnName("is_available")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnName("location")
                    .HasColumnType("geography(Point,4326)");

                entity.Property(e => e.Timezone).HasColumnName("timezone");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");
            });

            modelBuilder.Entity<CityVersions>(entity =>
            {
                entity.ToTable("city_versions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.VersionCommentary)
                    .IsRequired()
                    .HasColumnName("version_commentary")
                    .HasMaxLength(255);

                entity.Property(e => e.VersionNumber)
                    .IsRequired()
                    .HasColumnName("version_number")
                    .HasMaxLength(255);

                entity.Property(e => e.VersionedAt)
                    .HasColumnName("versioned_at")
                    .HasColumnType("timestamp(0) without time zone")
                    .HasDefaultValueSql("'2018-11-15 13:13:17'::timestamp without time zone");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.CityVersions)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("city_versions_city_id_foreign");
            });

            modelBuilder.Entity<Competitors>(entity =>
            {
                entity.ToTable("competitors");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AutoAssigned).HasColumnName("auto_assigned");

                entity.Property(e => e.CompetitorRouteId).HasColumnName("competitor_route_id");

                entity.Property(e => e.IsAssigned).HasColumnName("is_assigned");

                entity.Property(e => e.OwnerId).HasColumnName("owner_id");

                entity.Property(e => e.Rate).HasColumnName("rate");

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.Property(e => e.RouteSegmentId).HasColumnName("route_segment_id");

                entity.HasOne(d => d.CompetitorRoute)
                    .WithMany(p => p.CompetitorsCompetitorRoute)
                    .HasForeignKey(d => d.CompetitorRouteId)
                    .HasConstraintName("competitors_competitor_route_id_foreign");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Competitors)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("competitors_owner_id_foreign");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.CompetitorsRoute)
                    .HasForeignKey(d => d.RouteId)
                    .HasConstraintName("competitors_route_id_foreign");

                entity.HasOne(d => d.RouteSegment)
                    .WithMany(p => p.Competitors)
                    .HasForeignKey(d => d.RouteSegmentId)
                    .HasConstraintName("competitors_route_segment_id_foreign");
            });

            modelBuilder.Entity<DriverCompetitors>(entity =>
            {
                entity.ToTable("driver_competitors");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CompetitorRouteId).HasColumnName("competitor_route_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DriverRouteId).HasColumnName("driver_route_id");

                entity.Property(e => e.RouteSegmentId).HasColumnName("route_segment_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.HasOne(d => d.CompetitorRoute)
                    .WithMany(p => p.DriverCompetitors)
                    .HasForeignKey(d => d.CompetitorRouteId)
                    .HasConstraintName("driver_competitors_competitor_route_id_foreign");

                entity.HasOne(d => d.DriverRoute)
                    .WithMany(p => p.DriverCompetitors)
                    .HasForeignKey(d => d.DriverRouteId)
                    .HasConstraintName("driver_competitors_driver_route_id_foreign");

                entity.HasOne(d => d.RouteSegment)
                    .WithMany(p => p.DriverCompetitors)
                    .HasForeignKey(d => d.RouteSegmentId)
                    .HasConstraintName("driver_competitors_route_segment_id_foreign");
            });

            modelBuilder.Entity<DriverRoutes>(entity =>
            {
                entity.ToTable("driver_routes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DriverId).HasColumnName("driver_id");

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.DriverRoutes)
                    .HasForeignKey(d => d.DriverId)
                    .HasConstraintName("driver_routes_driver_id_foreign");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.DriverRoutes)
                    .HasForeignKey(d => d.RouteId)
                    .HasConstraintName("driver_routes_route_id_foreign");
            });

            modelBuilder.Entity<DriverSmsCodes>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("driver_sms_codes");

                entity.HasIndex(e => e.ExpiresAt)
                    .HasName("driver_sms_codes_expires_at_index");

                entity.HasIndex(e => e.PhoneNo)
                    .HasName("driver_sms_codes_phone_no_index");

                entity.Property(e => e.ExpiresAt)
                    .HasColumnName("expires_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasColumnName("phone_no")
                    .HasMaxLength(12);

                entity.Property(e => e.SmsCode)
                    .IsRequired()
                    .HasColumnName("sms_code")
                    .HasMaxLength(4);
            });

            modelBuilder.Entity<Drivers>(entity =>
            {
                entity.ToTable("drivers");

                entity.HasIndex(e => e.PhoneNo)
                    .HasName("drivers_phone_no_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("date");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DrivingLicenseCategory)
                    .HasColumnName("driving_license_category")
                    .HasMaxLength(255);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnName("gender")
                    .HasMaxLength(255);

                entity.Property(e => e.IsReady)
                    .HasColumnName("is_ready")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.JoinedAt)
                    .HasColumnName("joined_at")
                    .HasColumnType("date");

                entity.Property(e => e.OwnerId).HasColumnName("owner_id");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(60);

                entity.Property(e => e.PhoneNo)
                    .HasColumnName("phone_no")
                    .HasMaxLength(12);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Drivers)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("drivers_city_id_foreign");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Drivers)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("drivers_owner_id_foreign");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("job", "cron");

                entity.Property(e => e.Jobid)
                    .HasColumnName("jobid")
                    .HasDefaultValueSql("nextval('cron.jobid_seq'::regclass)");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Command)
                    .IsRequired()
                    .HasColumnName("command");

                entity.Property(e => e.Database)
                    .IsRequired()
                    .HasColumnName("database")
                    .HasDefaultValueSql("current_database()");

                entity.Property(e => e.Nodename)
                    .IsRequired()
                    .HasColumnName("nodename")
                    .HasDefaultValueSql("'localhost'::text");

                entity.Property(e => e.Nodeport)
                    .HasColumnName("nodeport")
                    .HasDefaultValueSql("inet_server_port()");

                entity.Property(e => e.Schedule)
                    .IsRequired()
                    .HasColumnName("schedule");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasDefaultValueSql("\"current_user\"()");
            });

            modelBuilder.Entity<LogRecords>(entity =>
            {
                entity.ToTable("log_records");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.MethodTitle)
                    .IsRequired()
                    .HasColumnName("method_title")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserTitle)
                    .IsRequired()
                    .HasColumnName("user_title")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Managers>(entity =>
            {
                entity.ToTable("managers");

                entity.HasIndex(e => e.Email)
                    .HasName("managers_email_unique")
                    .IsUnique();

                entity.HasIndex(e => e.IsActive)
                    .HasName("managers_is_active_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(60);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Managers)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("managers_city_id_foreign");
            });

            modelBuilder.Entity<Migrations>(entity =>
            {
                entity.ToTable("migrations");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Batch).HasColumnName("batch");

                entity.Property(e => e.Migration)
                    .IsRequired()
                    .HasColumnName("migration")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<MovingVehicles>(entity =>
            {
                entity.ToTable("moving_vehicles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AverageSpeed).HasColumnName("average_speed");

                entity.Property(e => e.ConfirmedAt)
                    .HasColumnName("confirmed_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DistanceFromLastPath).HasColumnName("distance_from_last_path");

                entity.Property(e => e.DriverId).HasColumnName("driver_id");

                entity.Property(e => e.IsOnline)
                    .HasColumnName("is_online")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.LastPathPosition).HasColumnName("last_path_position");

                entity.Property(e => e.Normalized)
                    .IsRequired()
                    .HasColumnName("normalized")
                    .HasColumnType("geography(Point,4326)");

                entity.Property(e => e.Real)
                    .IsRequired()
                    .HasColumnName("real")
                    .HasColumnType("geography(Point,4326)");

                entity.Property(e => e.Relevance)
                    .HasColumnName("relevance")
                    .HasDefaultValueSql("100");

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.Property(e => e.RouteSegmentId).HasColumnName("route_segment_id");

                entity.Property(e => e.SegmentId).HasColumnName("segment_id");

                entity.Property(e => e.SourceType)
                    .HasColumnName("source_type")
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.VehicleFromTable)
                    .HasColumnName("vehicle_from_table")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'vehicles'::character varying");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

                entity.HasOne(d => d.RouteSegment)
                    .WithMany(p => p.MovingVehicles)
                    .HasForeignKey(d => d.RouteSegmentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("moving_vehicles_route_segment_id_foreign");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.MovingVehicles)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("moving_vehicles_vehicle_id_foreign");
            });

            modelBuilder.Entity<OwnerCities>(entity =>
            {
                entity.ToTable("owner_cities");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.OwnerId).HasColumnName("owner_id");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.OwnerCities)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("owner_cities_city_id_foreign");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.OwnerCities)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("owner_cities_owner_id_foreign");
            });

            modelBuilder.Entity<OwnerRoutes>(entity =>
            {
                entity.ToTable("owner_routes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OwnerId).HasColumnName("owner_id");

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.OwnerRoutes)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("owner_routes_owner_id_foreign");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.OwnerRoutes)
                    .HasForeignKey(d => d.RouteId)
                    .HasConstraintName("owner_routes_route_id_foreign");
            });

            modelBuilder.Entity<Owners>(entity =>
            {
                entity.ToTable("owners");

                entity.HasIndex(e => e.Email)
                    .HasName("owners_email_unique")
                    .IsUnique();

                entity.HasIndex(e => e.IsActive)
                    .HasName("owners_is_active_index");

                entity.HasIndex(e => e.IsVirtual)
                    .HasName("owners_is_virtual_index");

                entity.HasIndex(e => e.JuridicalTitle)
                    .HasName("owners_juridical_title_index");

                entity.HasIndex(e => e.Title)
                    .HasName("owners_title_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.IsVirtual).HasColumnName("is_virtual");

                entity.Property(e => e.JuridicalTitle)
                    .IsRequired()
                    .HasColumnName("juridical_title")
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(60);

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasColumnName("phone_no")
                    .HasMaxLength(60);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");
            });

            modelBuilder.Entity<PassengerDevices>(entity =>
            {
                entity.ToTable("passenger_devices");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeviceType)
                    .IsRequired()
                    .HasColumnName("device_type")
                    .HasMaxLength(255);

                entity.Property(e => e.PassengerId).HasColumnName("passenger_id");

                entity.Property(e => e.PushToken)
                    .IsRequired()
                    .HasColumnName("push_token")
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Uuid)
                    .IsRequired()
                    .HasColumnName("uuid")
                    .HasMaxLength(36);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.PassengerDevices)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("passenger_devices_city_id_foreign");

                entity.HasOne(d => d.Passenger)
                    .WithMany(p => p.PassengerDevices)
                    .HasForeignKey(d => d.PassengerId)
                    .HasConstraintName("passenger_devices_passenger_id_foreign");
            });

            modelBuilder.Entity<PassengerSmsCodes>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("passenger_sms_codes");

                entity.HasIndex(e => e.ExpiresAt)
                    .HasName("passenger_sms_codes_expires_at_index");

                entity.HasIndex(e => e.PhoneNo)
                    .HasName("passenger_sms_codes_phone_no_index");

                entity.Property(e => e.ExpiresAt)
                    .HasColumnName("expires_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasColumnName("phone_no")
                    .HasMaxLength(12);

                entity.Property(e => e.SmsCode)
                    .IsRequired()
                    .HasColumnName("sms_code")
                    .HasMaxLength(4);
            });

            modelBuilder.Entity<Passengers>(entity =>
            {
                entity.ToTable("passengers");

                entity.HasIndex(e => e.PhoneNo)
                    .HasName("passengers_phone_no_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasColumnName("phone_no")
                    .HasMaxLength(12);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");
            });

            modelBuilder.Entity<PasswordResets>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("password_resets");

                entity.HasIndex(e => e.Email)
                    .HasName("password_resets_email_index");

                entity.HasIndex(e => e.Token)
                    .HasName("password_resets_token_index");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasColumnName("token")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Presets>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("presets");

                entity.HasIndex(e => e.Key)
                    .HasName("presets_key_unique")
                    .IsUnique();

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnName("key")
                    .HasMaxLength(50);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(1000);
            });

            modelBuilder.Entity<RouteSegmentPaths>(entity =>
            {
                entity.ToTable("route_segment_paths");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Distance).HasColumnName("distance");

                entity.Property(e => e.EndPoint)
                    .IsRequired()
                    .HasColumnName("end_point")
                    .HasColumnType("geography(Point,4326)");

                entity.Property(e => e.Position).HasColumnName("position");

                entity.Property(e => e.RouteSegmentId).HasColumnName("route_segment_id");

                entity.Property(e => e.StartPoint)
                    .IsRequired()
                    .HasColumnName("start_point")
                    .HasColumnType("geography(Point,4326)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.HasOne(d => d.RouteSegment)
                    .WithMany(p => p.RouteSegmentPaths)
                    .HasForeignKey(d => d.RouteSegmentId)
                    .HasConstraintName("route_segment_paths_route_segment_id_foreign");
            });

            modelBuilder.Entity<RouteSegmentSpeeds>(entity =>
            {
                entity.ToTable("route_segment_speeds");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AverageSpeed).HasColumnName("average_speed");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.History)
                    .HasColumnName("history")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RouteSegmentId).HasColumnName("route_segment_id");

                entity.Property(e => e.TimeIntervalId).HasColumnName("time_interval_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.HasOne(d => d.RouteSegment)
                    .WithMany(p => p.RouteSegmentSpeeds)
                    .HasForeignKey(d => d.RouteSegmentId)
                    .HasConstraintName("route_segment_speeds_route_segment_id_foreign");

                entity.HasOne(d => d.TimeInterval)
                    .WithMany(p => p.RouteSegmentSpeeds)
                    .HasForeignKey(d => d.TimeIntervalId)
                    .HasConstraintName("route_segment_speeds_time_interval_id_foreign");
            });

            modelBuilder.Entity<RouteSegments>(entity =>
            {
                entity.ToTable("route_segments");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Distance).HasColumnName("distance");

                entity.Property(e => e.IsReverse).HasColumnName("is_reverse");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnName("path")
                    .HasColumnType("geography(LineString,4326)");

                entity.Property(e => e.Position).HasColumnName("position");

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.Property(e => e.SegmentId).HasColumnName("segment_id");

                entity.Property(e => e.SegmentPathId).HasColumnName("segment_path_id");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.RouteSegments)
                    .HasForeignKey(d => d.RouteId)
                    .HasConstraintName("route_segments_route_id_foreign");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.RouteSegments)
                    .HasForeignKey(d => d.SegmentId)
                    .HasConstraintName("route_segments_segment_id_foreign");
            });

            modelBuilder.Entity<RouteStations>(entity =>
            {
                entity.ToTable("route_stations");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.IsReverse).HasColumnName("is_reverse");

                entity.Property(e => e.Position).HasColumnName("position");

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.Property(e => e.StationId).HasColumnName("station_id");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.RouteStations)
                    .HasForeignKey(d => d.RouteId)
                    .HasConstraintName("route_stations_route_id_foreign");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.RouteStations)
                    .HasForeignKey(d => d.StationId)
                    .HasConstraintName("route_stations_station_id_foreign");
            });

            modelBuilder.Entity<Routes>(entity =>
            {
                entity.ToTable("routes");

                entity.HasIndex(e => e.Title)
                    .HasName("routes_title_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.IsReady)
                    .IsRequired()
                    .HasColumnName("is_ready")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.VehicleTypeId).HasColumnName("vehicle_type_id");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Routes)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("routes_city_id_foreign");

                entity.HasOne(d => d.VehicleType)
                    .WithMany(p => p.Routes)
                    .HasForeignKey(d => d.VehicleTypeId)
                    .HasConstraintName("routes_vehicle_type_id_foreign");
            });

            modelBuilder.Entity<SegmentPaths>(entity =>
            {
                entity.ToTable("segment_paths");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Distance).HasColumnName("distance");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnName("path")
                    .HasColumnType("geography(LineString,4326)");

                entity.Property(e => e.SegmentId).HasColumnName("segment_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.SegmentPaths)
                    .HasForeignKey(d => d.SegmentId)
                    .HasConstraintName("segment_paths_segment_id_foreign");
            });

            modelBuilder.Entity<Segments>(entity =>
            {
                entity.ToTable("segments");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.FromStationPlatformId).HasColumnName("from_station_platform_id");

                entity.Property(e => e.ToStationPlatformId).HasColumnName("to_station_platform_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.HasOne(d => d.FromStationPlatform)
                    .WithMany(p => p.SegmentsFromStationPlatform)
                    .HasForeignKey(d => d.FromStationPlatformId)
                    .HasConstraintName("segments_from_station_platform_id_foreign");

                entity.HasOne(d => d.ToStationPlatform)
                    .WithMany(p => p.SegmentsToStationPlatform)
                    .HasForeignKey(d => d.ToStationPlatformId)
                    .HasConstraintName("segments_to_station_platform_id_foreign");
            });

            modelBuilder.Entity<StationPlatformVehicleTypes>(entity =>
            {
                entity.ToTable("station_platform_vehicle_types");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.StationPlatformId).HasColumnName("station_platform_id");

                entity.Property(e => e.VehicleTypeId).HasColumnName("vehicle_type_id");

                entity.HasOne(d => d.StationPlatform)
                    .WithMany(p => p.StationPlatformVehicleTypes)
                    .HasForeignKey(d => d.StationPlatformId)
                    .HasConstraintName("station_platform_vehicle_types_station_platform_id_foreign");

                entity.HasOne(d => d.VehicleType)
                    .WithMany(p => p.StationPlatformVehicleTypes)
                    .HasForeignKey(d => d.VehicleTypeId)
                    .HasConstraintName("station_platform_vehicle_types_vehicle_type_id_foreign");
            });

            modelBuilder.Entity<StationPlatforms>(entity =>
            {
                entity.ToTable("station_platforms");

                entity.HasIndex(e => e.Location)
                    .HasName("station_platforms_location_index")
                    .HasMethod("gist");

                entity.HasIndex(e => e.Title)
                    .HasName("station_platforms_title_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnName("location")
                    .HasColumnType("geography(Point,4326)");

                entity.Property(e => e.OsmId).HasColumnName("osm_id");

                entity.Property(e => e.StationId).HasColumnName("station_id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.StationPlatforms)
                    .HasForeignKey(d => d.StationId)
                    .HasConstraintName("station_platforms_station_id_foreign");
            });

            modelBuilder.Entity<Stations>(entity =>
            {
                entity.ToTable("stations");

                entity.HasIndex(e => e.Title)
                    .HasName("stations_title_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.PublicTitle)
                    .HasColumnName("public_title")
                    .HasMaxLength(255);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Stations)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("stations_city_id_foreign");
            });

            modelBuilder.Entity<TestLocationStats>(entity =>
            {
                entity.ToTable("test_location_stats");

                entity.HasIndex(e => e.Id)
                    .HasName("test_location_stats_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AverageSpeed).HasColumnName("average_speed");

                entity.Property(e => e.Normalized)
                    .HasColumnName("normalized")
                    .HasColumnType("geography(Point,4326)");

                entity.Property(e => e.ObtainedAt)
                    .HasColumnName("obtained_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Real)
                    .HasColumnName("real")
                    .HasColumnType("geography(Point,4326)");

                entity.Property(e => e.RouteSegmentId).HasColumnName("route_segment_id");

                entity.Property(e => e.UniqueId).HasColumnName("unique_id");

                entity.Property(e => e.VehicleTypeId).HasColumnName("vehicle_type_id");

                entity.HasOne(d => d.RouteSegment)
                    .WithMany(p => p.TestLocationStats)
                    .HasForeignKey(d => d.RouteSegmentId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("test_location_stats_route_segment_id_foreign");

                entity.HasOne(d => d.VehicleType)
                    .WithMany(p => p.TestLocationStats)
                    .HasForeignKey(d => d.VehicleTypeId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("test_location_stats_vehicle_type_id_foreign");
            });

            modelBuilder.Entity<TestSpeedStats>(entity =>
            {
                entity.ToTable("test_speed_stats");

                entity.HasIndex(e => e.Id)
                    .HasName("test_speed_stats_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AverageSpeed).HasColumnName("average_speed");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.RouteSegmentId).HasColumnName("route_segment_id");

                entity.Property(e => e.VehicleTypeId).HasColumnName("vehicle_type_id");

                entity.HasOne(d => d.RouteSegment)
                    .WithMany(p => p.TestSpeedStats)
                    .HasForeignKey(d => d.RouteSegmentId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("test_speed_stats_route_segment_id_foreign");

                entity.HasOne(d => d.VehicleType)
                    .WithMany(p => p.TestSpeedStats)
                    .HasForeignKey(d => d.VehicleTypeId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("test_speed_stats_vehicle_type_id_foreign");
            });

            modelBuilder.Entity<TimeIntervals>(entity =>
            {
                entity.ToTable("time_intervals");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week");

                entity.Property(e => e.TimeEnd)
                    .HasColumnName("time_end")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.TimeStart)
                    .HasColumnName("time_start")
                    .HasColumnType("timestamp(0) without time zone");
            });

            modelBuilder.Entity<UnverifiedVehicles>(entity =>
            {
                entity.ToTable("unverified_vehicles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Gosnomer)
                    .IsRequired()
                    .HasColumnName("gosnomer")
                    .HasMaxLength(255);

                entity.Property(e => e.Photo)
                    .IsRequired()
                    .HasColumnName("photo")
                    .HasMaxLength(255);

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.VehicleKindId)
                    .HasColumnName("vehicle_kind_id")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.VehicleSenderId).HasColumnName("vehicle_sender_id");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.UnverifiedVehicles)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("unverified_vehicles_city_id_foreign");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.UnverifiedVehicles)
                    .HasForeignKey(d => d.RouteId)
                    .HasConstraintName("unverified_vehicles_route_id_foreign");

                entity.HasOne(d => d.VehicleKind)
                    .WithMany(p => p.UnverifiedVehicles)
                    .HasForeignKey(d => d.VehicleKindId)
                    .HasConstraintName("unverified_vehicles_vehicle_kind_id_foreign");

                entity.HasOne(d => d.VehicleSender)
                    .WithMany(p => p.UnverifiedVehicles)
                    .HasForeignKey(d => d.VehicleSenderId)
                    .HasConstraintName("unverified_vehicles_vehicle_sender_id_foreign");
            });

            modelBuilder.Entity<VehicleDriverRoutes>(entity =>
            {
                entity.ToTable("vehicle_driver_routes");

                entity.HasIndex(e => e.Id)
                    .HasName("vehicle_driver_routes_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DriverRouteId).HasColumnName("driver_route_id");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

                entity.HasOne(d => d.DriverRoute)
                    .WithMany(p => p.VehicleDriverRoutes)
                    .HasForeignKey(d => d.DriverRouteId)
                    .HasConstraintName("vehicle_driver_routes_driver_route_id_foreign");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.VehicleDriverRoutes)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("vehicle_driver_routes_vehicle_id_foreign");
            });

            modelBuilder.Entity<VehicleDrivers>(entity =>
            {
                entity.ToTable("vehicle_drivers");

                entity.HasIndex(e => e.Id)
                    .HasName("vehicle_drivers_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DriverId).HasColumnName("driver_id");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.VehicleDrivers)
                    .HasForeignKey(d => d.DriverId)
                    .HasConstraintName("vehicle_drivers_driver_id_foreign");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.VehicleDrivers)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("vehicle_drivers_vehicle_id_foreign");
            });

            modelBuilder.Entity<VehicleKinds>(entity =>
            {
                entity.ToTable("vehicle_kinds");

                entity.HasIndex(e => e.IsAvailable)
                    .HasName("vehicle_kinds_is_available_index");

                entity.HasIndex(e => e.Title)
                    .HasName("vehicle_kinds_title_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.IsAvailable)
                    .IsRequired()
                    .HasColumnName("is_available")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.VehicleTypeId).HasColumnName("vehicle_type_id");

                entity.HasOne(d => d.VehicleType)
                    .WithMany(p => p.VehicleKinds)
                    .HasForeignKey(d => d.VehicleTypeId)
                    .HasConstraintName("vehicle_kinds_vehicle_type_id_foreign");
            });

            modelBuilder.Entity<VehicleOwnerRoutes>(entity =>
            {
                entity.ToTable("vehicle_owner_routes");

                entity.HasIndex(e => e.Id)
                    .HasName("vehicle_owner_routes_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OwnerRouteId).HasColumnName("owner_route_id");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

                entity.HasOne(d => d.OwnerRoute)
                    .WithMany(p => p.VehicleOwnerRoutes)
                    .HasForeignKey(d => d.OwnerRouteId)
                    .HasConstraintName("vehicle_owner_routes_owner_route_id_foreign");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.VehicleOwnerRoutes)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("vehicle_owner_routes_vehicle_id_foreign");
            });

            modelBuilder.Entity<VehicleSenders>(entity =>
            {
                entity.ToTable("vehicle_senders");

                entity.HasIndex(e => e.Username)
                    .HasName("vehicle_senders_username_unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CountCorrect).HasColumnName("count_correct");

                entity.Property(e => e.CountIncorrect).HasColumnName("count_incorrect");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Mark).HasColumnName("mark");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<VehicleTypes>(entity =>
            {
                entity.ToTable("vehicle_types");

                entity.HasIndex(e => e.IsAvailable)
                    .HasName("vehicle_types_is_available_index");

                entity.HasIndex(e => e.Title)
                    .HasName("vehicle_types_title_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Color).HasColumnName("color");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.IconUrl)
                    .HasColumnName("icon_url")
                    .HasMaxLength(255);

                entity.Property(e => e.IsAvailable)
                    .IsRequired()
                    .HasColumnName("is_available")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");
            });

            modelBuilder.Entity<Vehicles>(entity =>
            {
                entity.ToTable("vehicles");

                entity.HasIndex(e => e.Login)
                    .HasName("vehicles_login_unique")
                    .IsUnique();

                entity.HasIndex(e => e.Number)
                    .HasName("vehicles_number_index");

                entity.HasIndex(e => e.Title)
                    .HasName("vehicles_title_index");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DeletedAt)
                    .HasColumnName("deleted_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.DriverId).HasColumnName("driver_id");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasMaxLength(255);

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasColumnName("number")
                    .HasMaxLength(20);

                entity.Property(e => e.OwnerId).HasColumnName("owner_id");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(100);

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.VehicleKindId).HasColumnName("vehicle_kind_id");

                entity.Property(e => e.VehicleTypeId).HasColumnName("vehicle_type_id");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("vehicles_city_id_foreign");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("vehicles_driver_id_foreign");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("vehicles_owner_id_foreign");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.RouteId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("vehicles_route_id_foreign");

                entity.HasOne(d => d.VehicleKind)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.VehicleKindId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("vehicles_vehicle_kind_id_foreign");

                entity.HasOne(d => d.VehicleType)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.VehicleTypeId)
                    .HasConstraintName("vehicles_vehicle_type_id_foreign");
            });

            modelBuilder.Entity<VerifiedVehicles>(entity =>
            {
                entity.ToTable("verified_vehicles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.Gosnomer)
                    .IsRequired()
                    .HasColumnName("gosnomer")
                    .HasMaxLength(255);

                entity.Property(e => e.RouteId).HasColumnName("route_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp(0) without time zone");

                entity.Property(e => e.VehicleKindId)
                    .HasColumnName("vehicle_kind_id")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.VehicleSenderId)
                    .HasColumnName("vehicle_sender_id")
                    .HasDefaultValueSql("1");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.VerifiedVehicles)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("verified_vehicles_city_id_foreign");

                entity.HasOne(d => d.Route)
                    .WithMany(p => p.VerifiedVehicles)
                    .HasForeignKey(d => d.RouteId)
                    .HasConstraintName("verified_vehicles_route_id_foreign");

                entity.HasOne(d => d.VehicleKind)
                    .WithMany(p => p.VerifiedVehicles)
                    .HasForeignKey(d => d.VehicleKindId)
                    .HasConstraintName("verified_vehicles_vehicle_kind_id_foreign");

                entity.HasOne(d => d.VehicleSender)
                    .WithMany(p => p.VerifiedVehicles)
                    .HasForeignKey(d => d.VehicleSenderId)
                    .HasConstraintName("verified_vehicles_vehicle_sender_id_foreign");
            });

            modelBuilder.HasSequence("jobid_seq", "cron");

            modelBuilder.HasSequence("test_laps_id_seq");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
