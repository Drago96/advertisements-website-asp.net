﻿using ExamProject.Validations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExamProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [Birthday]
        public string Birthday { get; set; }

        private ICollection<Comment> writtenComments;

        public virtual ICollection<Comment> WrittenComments
        {
            get { return this.writtenComments; }
            set { this.writtenComments = value; }
        }

        private ICollection<Comment> profileComments;

        public virtual ICollection<Comment> ProfileComments
        {
            get { return this.profileComments; }
            set { this.profileComments = value; }
        }

        private ICollection<Advertisement> advertisements;

        public virtual ICollection<Advertisement> Advertisements
        {
            get { return this.advertisements; }
            set { this.advertisements = value; }
        }

        public ApplicationUser()
        {
            this.writtenComments = new HashSet<Comment>();
            this.profileComments = new HashSet<Comment>();
            this.advertisements = new HashSet<Advertisement>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}