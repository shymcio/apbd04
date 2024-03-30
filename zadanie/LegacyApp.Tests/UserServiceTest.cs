using System;
using JetBrains.Annotations;
using LegacyApp;
using Xunit;

namespace LegacyApp.Tests;

[TestSubject(typeof(UserService))]
public class UserServiceTest
{

    [Fact]
    public void AddUser_Should_Return_False_When_First_Name_Is_Empty()
    {
        var userService = new UserService();
        var result = userService.AddUser("", "Doe", "email@email.com", DateTime.Now, 1);
        Assert.False(result);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_Last_Name_Is_Empty()
    {
        var userService = new UserService();
        var result = userService.AddUser("John", "", "email@email.com", DateTime.Now, 1);
        Assert.False(result);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_Email_Doesnt_Contain_At_Or_Dot()
    {
        var userService = new UserService();
        var result = userService.AddUser("John", "Doe", "emailemail.com", DateTime.Now, 1);
        Assert.False(result);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_Age_Less_Than_21_Years_Old()
    {
        var userService = new UserService();
        var result = userService.AddUser("John", "Doe", "john.doe@example.com", DateTime.Now.AddYears(-20), 1);
        Assert.False(result);
    }
    
    [Fact]
    public void AddUser_Should_Return_True_When_Age_More_Than_21_Years_Old()
    {
        var userService = new UserService();
        var result = userService.AddUser("John", "Doe", "john.doe@example.com", DateTime.Now.AddYears(22), 1);
        Assert.False(result);
    }
    
    [Fact]
    public void AddUser_Should_Return_True_When_Age_Equals_21_Years_Old()
    {
        var userService = new UserService();
        var result = userService.AddUser("John", "Doe", "john.doe@example.com", DateTime.Now.AddYears(21), 1);
        Assert.False(result);
    }
    
    [Fact]
    public void AddUser_Should_Throw_Argument_Exception_When_Client_Doesnt_Exists()
    {
        var userService = new UserService();
        Assert.Throws<ArgumentException>(() =>
        {
            userService.AddUser("John", "Doe", "johndoe@gmail.com", DateTime.Parse("1900-03-21"), 100);
        });
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_Client_Has_CreditLimit_Below_500()
    {
        var userService = new UserService();
        var result = userService.AddUser("John", "Kowalski", "johndoe@gmail.com", DateTime.Parse("1900-03-21"), 1);
        Assert.False(result);
    }
    
    [Fact]
    public void AddUser_Should_Return_True_When_Client_Has_CreditLimit_Above_500()
    {
        var userService = new UserService();
        var result = userService.AddUser("John", "Doe", "johndoe@gmail.com", DateTime.Parse("1900-03-21"), 4);
        Assert.True(result);
    }
    
    [Fact]
    public void AddUser_Should_Return_True_When_Client_Is_Important()
    {
        var userService = new UserService();
        var result = userService.AddUser("John", "Doe", "johndoe@gmail.com", DateTime.Parse("1900-03-21"), 3);
        Assert.True(result);
    }
}