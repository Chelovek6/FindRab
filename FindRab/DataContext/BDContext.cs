﻿using FindRab.models;
using FindRab.Models;
using Microsoft.EntityFrameworkCore;

namespace FindRab.DataContext
{
    public class BDContext : DbContext
    {
        public DbSet<WorkExperience> WorkExperiencesM { get; set; }
        public DbSet<User> UserM { get; set; }
        public DbSet<SecurityModel> SecurM { get; set; }
        public DbSet<Vacancy> VacanciesM { get; set; }
        public DbSet<Skill> SkillsM { get; set; }
        public DbSet<JobCategory> JobCategoriesM { get; set; }
        public DbSet<JobApplication> JobApplicationsM { get; set; }
        public DbSet<Employer> EmployersM { get; set; }
        public DbSet<Education> EducationsM { get; set; }
        public DbSet<ApplicantSkill> ApplicantSkillsM { get; set; }
        public DbSet<Applicant> ApplicantsM { get; set; }

        public BDContext(DbContextOptions<BDContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        

    }
}
