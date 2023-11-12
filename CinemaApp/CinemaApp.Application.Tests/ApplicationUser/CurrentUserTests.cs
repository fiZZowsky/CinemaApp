using Xunit;
using CinemaApp.Application.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace CinemaApp.Application.ApplicationUser.Tests
{
    public class CurrentUserTests
    {
        [Fact()]
        public void IsInRole_WithMatchingRole_ShouldReturnTrue()
        {
            // arrange
            var currentUser = new CurrentUser("1", "test@tests.com", new List<string> { "Admin", "User" });

            // act
            var isInRole = currentUser.IsInRole("Admin");

            // assert
            isInRole.Should().BeTrue();
        }

        [Fact()]
        public void IsInRole_WithNonMatchingRole_ShouldReturnFalse()
        {
            // arrange
            var currentUser = new CurrentUser("1", "test@tests.com", new List<string> { "Admin", "User" });

            // act
            var isInRole = currentUser.IsInRole("Test");

            // assert
            isInRole.Should().BeFalse();
        }

        [Fact()]
        public void IsInRole_WithNonMatchingCaseRole_ShouldReturnFalse()
        {
            // arrange
            var currentUser = new CurrentUser("1", "test@tests.com", new List<string> { "Admin", "User" });

            // act
            var isInRole = currentUser.IsInRole("admin");

            // assert
            isInRole.Should().BeFalse();
        }
    }
}