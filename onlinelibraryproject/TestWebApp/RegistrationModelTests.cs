using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapp.Pages;

namespace TestWebApp
{
    public class RegistrationModelTests
    {
        [Fact]
        public void Model_ShouldHaveValidationErrors_WhenFieldsAreEmpty()
        {
            // Arrange
            var model = new RegistrationModel
            {
                Email = "",
                Password = "",
                ConfirmPassword = ""
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);

            // Act
            bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(validationResults);
        }

        [Fact]
        public void Model_ShouldFailValidation_WhenInvalidEmailProvided()
        {
            // Arrange
            var model = new RegistrationModel
            {
                Email = "invalid-email",
                Password = "ValidPass123!",
                ConfirmPassword = "ValidPass123!"
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);

            // Act
            bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.ErrorMessage.ToLower().Contains("почта"));
        }

        [Fact]
        public void Model_ShouldFailValidation_WhenPasswordsDoNotMatch()
        {
            // Arrange
            var model = new RegistrationModel
            {
                Email = "test@example.com",
                Password = "ValidPass123!",
                ConfirmPassword = "AnotherPass123!"
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);

            // Act
            bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.ErrorMessage.Contains("Пароли не совпадают"));
        }

        [Fact]
        public async Task OnPost_ReturnsPageResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var model = new RegistrationModel
            {
                Email = "invalid-email",
                Password = "123",
                ConfirmPassword = "123"
            };

            model.ModelState.AddModelError("Email", "Invalid email format");

            // Act
            var result = await model.OnPost(model.Email, model.Password, model.ConfirmPassword);

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}
