using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TripIt2Gether.Models;

namespace TripIt2Gether.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TourOperator> TourOperators { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Answer>(b =>
            {
                

                b.HasKey(c => new { c.QuestionId, c.ParticipantId, c.TripNumber });

                b
                .HasOne(cs => cs.Question)
                .WithMany(p => p.Answers)
                .HasForeignKey(p => p.QuestionId);

                b
                .HasOne(cs => cs.Application)
                .WithMany(p => p.Answers)
                .HasForeignKey(cs => new { cs.ParticipantId, cs.TripNumber });
            });


            builder.Entity<Application>(b =>
           {
               b
               .HasOne(cs => cs.Trip)
               .WithMany(p => p.Applications)
               .HasForeignKey(cs => cs.TripId)
               .OnDelete(DeleteBehavior.Cascade);

               b
               .HasOne(cs => cs.Participant)
               .WithMany(p => p.Applications)
               .HasForeignKey(cs => cs.UserId);

               b.HasKey(c => new { c.TripId, c.UserId });

           });


            builder.Entity<TourOperator>(b =>
            {
                b.HasKey(c => new { c.OperatorId, c.TripId });

                b.HasOne(cs => cs.Trip)
                .WithMany(p => p.TourOperators)
                .HasForeignKey(cs => cs.TripId);

                b.HasOne(cs => cs.Operator)
                .WithMany(p => p.TourOperators)
                .HasForeignKey(cs => cs.OperatorId);
            });


            builder.Entity<Trip>(b => {
                b.HasOne(cs => cs.Form)
                .WithOne(p => p.Trip)
                .OnDelete(DeleteBehavior.NoAction);
                //b.HasMany(cs => cs.Applications)
                //.WithOne(p => p.Trip)
                //.HasForeignKey(cs => new { cs.TripId, cs.UserId });
            }); 

            builder.Entity<Question>()
                .HasOne(cs => cs.Form)
                .WithMany(p => p.Questions)
                .HasForeignKey(cs => cs.FormId);
           
        }
    }
}
