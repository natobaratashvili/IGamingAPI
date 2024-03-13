using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using IGaming.Core.UsersManagement.Security;
using IGaming.Infrastructure.Security.Jwt;
using System.Text;

namespace IGamingAPITests;
[TestFixture]
public class JwtProviderTests
{
    private IJwtProvider _jwtProvider;
    private JwtSettings _jwtSettings;

    [SetUp]
    public void SetUp()
    {
        _jwtSettings = new JwtSettings
        {
            Issuer = "testIssuer",
            TokenExpirationInMinutes = 10,
            Secret = "secret_key"
        };

        var mockJwtOptions = new Mock<IOptions<JwtSettings>>();
        mockJwtOptions.Setup(x => x.Value).Returns(_jwtSettings);
        _jwtProvider = new JwtProvider(mockJwtOptions.Object);
    }

    
    [Test]
    public void GenerateToken_DifferntToken()
    {
        //Arrange
        var claims = new Dictionary<string, string>
                {
                { "sub", "testSubject" },
                 };
        // Act & Assert
        var token1 = _jwtProvider.GenerateToken(claims);
        var token2 = _jwtProvider.GenerateToken(claims);
        Assert.That(token1, Is.Not.EqualTo(token2));

    }
    [Test]
    public void GenerateToken_ReturnsToken_With_3_block()
    {
        // Arrange
        var claims = new Dictionary<string, string>
            {
            { "sub", "testSubject" },
             };

        // Act
        var token = _jwtProvider.GenerateToken(claims);
        var parts = token.Split(".");
        // Assert
        Assert.That(token, Is.Not.Null);
        Assert.That(token, Is.Not.Empty);
        Assert.That(parts.Length, Is.EqualTo(3));
        Assert.That(parts[0].Length, Is.GreaterThan(0));
        Assert.That(parts[1].Length, Is.GreaterThan(0));
        Assert.That(parts[2].Length, Is.GreaterThan(0));

    }

    [Test]
    public void ValidateToken_InvalidToken_ReturnsFalse()
    {
        // Arrange
        var invalidToken = "my.invalid.token";

        // Act
        var isValid = _jwtProvider.ValidateToken(invalidToken);

        // Assert
        Assert.That(isValid, Is.False);
    }
}

