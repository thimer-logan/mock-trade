﻿using FluentAssertions;
using Moq;
using StocksApp.Application.Interfaces.Finnhub;
using StocksApp.Application.Services.Finnhub;
using StocksApp.Domain.Interfaces;

namespace StocksApp.UnitTests.Application.Services.Finnhub
{
    public class FinnhubCompanyProfileServiceTest
    {
        private readonly IFinnhubCompanyProfileService _service;
        private readonly Mock<IFinnhubRepository> _finnhubRepositoryMock;

        public FinnhubCompanyProfileServiceTest()
        {
            _finnhubRepositoryMock = new Mock<IFinnhubRepository>();
            _service = new FinnhubCompanyProfileService(_finnhubRepositoryMock.Object);
        }

        [Fact]
        public async Task GetCompanyProfile_WhenSymbolIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            string symbol = string.Empty;

            // Act
            Func<Task> act = async () => await _service.GetCompanyProfile(symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetCompanyProfile_WhenSymbolIsWhiteSpace_ThrowsArgumentNullException()
        {
            // Arrange
            string symbol = " ";

            // Act
            Func<Task> act = async () => await _service.GetCompanyProfile(symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetCompanyProfile_ValidSymbol_CallsGetCompanyProfileAsync()
        {
            // Arrange
            string symbol = "AAPL";

            // Act
            await _service.GetCompanyProfile(symbol);

            // Assert
            _finnhubRepositoryMock.Verify(x => x.GetCompanyProfileAsync(symbol), Times.Once);
        }
    }
}
