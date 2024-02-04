using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class User
    {
        #region Properties

        public string UserId { get; private set; }

        public string Email { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public UserGender Gender { get; private set; }

        public DateTime? DateOfBirth { get; private set; }

        public int? Age
        {
            get
            {
                if (DateOfBirth.HasValue)
                {
                    var     today   = DateTime.Today;
                    var     bday    = DateOfBirth.Value;
                    int     age     = today.Year - bday.Year;
                    if (bday > today.AddYears(-age))
                    {
                        age--;
                    }
                    return age;
                }
                return null;
            }
        }

        public bool IsGuest { get; private set; }

        #endregion

        #region Constructors

        public User(string userId, string email = null,
            string firstName = null, string lastName = null,
            UserGender gender = UserGender.Undefined, DateTime? dob = null,
            bool isGuest = false)
        {
            // set properties
            UserId      = userId;
            Email       = email;
            FirstName   = firstName;
            LastName    = lastName;
            DateOfBirth = dob;
            Gender      = gender;
            IsGuest     = isGuest;
        }

        #endregion

        #region Nested types

        public enum UserGender
        {
            Undefined = 0,

            Male,

            Female,

            Others,
        }

        #endregion
    }
}