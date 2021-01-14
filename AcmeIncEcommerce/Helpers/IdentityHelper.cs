using AcmeIncEcommerce.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AcmeIncEcommerce.Helpers
{
    public class IdentityHelper
    {
        internal static void SeedIdentities(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists(RoleNames.ROLE_ADMINISTRATOR))
            {
                roleManager.Create(new IdentityRole(RoleNames.ROLE_ADMINISTRATOR));
            }
            if (!roleManager.RoleExists(RoleNames.ROLE_CUSTOMER))
            {
                roleManager.Create(new IdentityRole(RoleNames.ROLE_CUSTOMER));
            }


            //Username and password details for demo user asssigned the role of Administrator 
            string userName = "admin@acme.com";
            string password = "Acme@Admin1";

            //Check to see if an Admin user has already been created in the database
            //If not a new ApplicationUser object is instantiated with account registration details required required to create the new user
            var user = userManager.FindByName(userName);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = userName,
                    Email = userName,
                    FirstName = "Acme",
                    LastName = "Admin",
                    DateOfBirth = new DateTime(1995, 2, 4),

                    Address = new Address
                    {
                        AddressLine1 = "1 Admin Street",
                        Town = "Town",
                        County = "County",
                        Postcode = "PostCode"
                    }
                };

                //Creating and Assigning the new user the role of Adminstrator
                IdentityResult userResult = userManager.Create(user, password);
                if (userResult.Succeeded)
                {
                    userManager.AddToRole(user.Id, RoleNames.ROLE_ADMINISTRATOR);
                }
            }

            //Details for demo Customer user 1 in the role of Customer
            userName = "joe@examplecustomer.com";
            password = "Example@Customer1";

            //Check to see if a Demo Customer user 1 has already been created in the database
            //If not a new ApplicationUser object is instantiated with account registration details required to create the new user
            user = userManager.FindByName(userName);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = userName,
                    Email = userName,
                    FirstName = "Joe",
                    LastName = "Black",
                    DateOfBirth = new DateTime(1984,5, 12),

                    Address = new Address
                    {
                        AddressLine1 = "53 Fox St",
                        Town = "Hartbeesfontein",
                        County = "North West",
                        Postcode = "2600"
                    }
                };

                //Creating and Assigning the new user the role of Customer
                IdentityResult userResult = userManager.Create(user, password);
                if (userResult.Succeeded)
                {
                    userManager.AddToRole(user.Id, RoleNames.ROLE_CUSTOMER);
                }
            }


            //Details for demo customer user 2 in the role of Customer
            userName = "ntobeko@examplecustomer.com";
            password = "Example@Customer2";

            //Check to see if demo Customer user 2 has already been created in the database
            //If not a new ApplicationUser object is instantiated with account registration details required to create the new user
            user = userManager.FindByName(userName);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = userName,
                    Email = userName,
                    FirstName = "Ntobeko",
                    LastName = "Nhlanhla",
                    DateOfBirth = new DateTime(1990, 21, 7),

                    Address = new Address
                    {
                        AddressLine1 = "691 Kamp St",
                        Town = "Belhar",
                        County = "Western Cape",
                        Postcode = "7504"
                    }
                };

                //Creating and Assigning the new user the role of Customer
                IdentityResult userResult = userManager.Create(user, password);
                if (userResult.Succeeded)
                {
                    userManager.AddToRole(user.Id, RoleNames.ROLE_CUSTOMER);
                }
            }

            //Details for demo customer user 3 in the role of Customer
            userName = "vilina@examplecustomer.com";
            password = "Example@Customer3";

            //Check to see if demo Customer user 2 has already been created in the database
            //If not a new ApplicationUser object is instantiated with account registration details required to create the new user
            user = userManager.FindByName(userName);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = userName,
                    Email = userName,
                    FirstName = "Vilina",
                    LastName = "Maharaj",
                    DateOfBirth = new DateTime(1999, 1, 28),

                    Address = new Address
                    {
                        AddressLine1 = "481 Main Rd",
                        Town = "Stanger",
                        County = "KwaZulu-Natal",
                        Postcode = "4453"
                    }
                };

                //Creating and Assigning the new user the role of Customer
                IdentityResult userResult = userManager.Create(user, password);
                if (userResult.Succeeded)
                {
                    userManager.AddToRole(user.Id, RoleNames.ROLE_CUSTOMER);
                }
            }


        }
    }
}
        
    
